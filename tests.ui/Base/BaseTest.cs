using core.Configuration;
using core.Logging.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using testing_browser.Fixtures;
using testing_browser.Services;
using testing_browser.Services.Dtos;
using testing_browser.Utils;
using testing_browser.WebDriver;
using testing_core.Fixtures;

namespace tests.ui.Base
{
    /// <summary>
    /// Serves as the abstract base class for all UI test classes, providing shared access to the browser,
    /// configuration, and logging. Automatically manages WebDriver lifecycle.
    /// </summary>
    public abstract class BaseTest : IDisposable, IAsyncLifetime
    {
        /// <summary>
        /// Gets the browser automation driver used for UI interactions.
        /// </summary>
        protected WebBrowser Driver { get; private set; }

        /// <summary>
        /// Gets the logger used for reporting and diagnostics.
        /// </summary>
        protected ITestLogger Logger { get; private set; }

        /// <summary>
        /// Gets the test configuration.
        /// </summary>
        protected TestConfig Config { get; private set; }

        /// <summary>
        /// Gets the username used for authentication in tests.
        /// </summary>
        /// <remarks>
        /// This property retrieves the default username from the configuration.
        /// It can be overridden in derived test classes to provide custom credentials.
        /// </remarks>
        protected virtual string Username => Config.Auth.Username; // Đây là expression-bodied property, viết gọn get { return Config.Auth.Username; }

        /// <summary>
        /// Gets the password used for authentication in tests.
        /// </summary>
        /// <remarks>
        /// This property retrieves the default password from the configuration.
        /// It can be overridden in derived test classes to provide custom credentials.
        /// </remarks>
        protected virtual string Password => Config.Auth.Password;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTest"/> class,
        /// using injected fixtures to initialize browser, configuration, and logging.
        /// </summary>
        public BaseTest(
            ConfigFixture configFixture,
            LoggingFixture loggingFixture,
            DriverFixture driverFixture)
        {
            Driver = driverFixture.DriverFactory.CreateDriver(configFixture.Config.Driver, loggingFixture.Logger);

            Logger = loggingFixture.Logger;

            Config = configFixture.Config;
        }

        /// <summary>
        /// Authenticates the test session by logging in via the API and injecting the JWT token as a browser cookie.
        /// </summary>
        /// <param name="username">The username to use for authentication.</param>
        /// <param name="password">The password to use for authentication.</param>
        /// <returns>A task representing the asynchronous authentication operation.</returns>
        /// <remarks>
        /// This method performs an asynchronous login using the provided credentials via <c>AuthService</c>,
        /// navigates the browser to the authentication domain to allow cookie injection,
        /// and sets the JWT token as a session cookie named <c>JWT_TOKEN</c>.
        /// </remarks>
        public async Task Authenticate(string username, string password)
        {
            AuthService authService = new AuthService(Config.Api);
            Task<AuthResponse> responseTask = authService.LoginAsync(username, password); // không await nên chương trình đi tiếp dòng kế tiếp mà không đứng chờ

            Driver.NavigateTo(Config.Auth.Url); // dòng này và dòng trên chạy song song, không chờ nhau, tiết kiệm thời gian

            AuthResponse response = await responseTask; // Chỉ khi gặp await thì chương trình mới dừng lại để lấy kết quả

            // Thêm cookie chứa JWT token vào trình duyệt
            // mỗi lần trình duyệt mở một trang của ứng dụng, nó tự động gửi cookie này kèm theo yêu cầu
            Driver.AddCookie(Constants.AUTH_COOKIE_NAME, response.Token);
        }

        /// <summary>
        /// Phương thức này tạo ra một RestClient đã mang sẵn token của phiên đăng nhập hiện tại, 
        /// nên các request API được xem là cùng người dùng với trình duyệt
        /// </summary>
        protected virtual RestClient GetRestClient()
        {
            // Lấy cookie chứa token từ trình duyệt (cookie mà method Authenticate đã thêm). Vì vậy phải gọi Authenticate trước, nếu không cookie sẽ là null
            var jwtTokenCookie = Driver.GetCookie(Constants.AUTH_COOKIE_NAME);

            // Tạo “hộp đựng cookie”. RestClient sẽ tự gửi các cookie trong hộp này kèm mỗi request
            var cookieContainer = new CookieContainer();

            // Sao chép cookie từ trình duyệt sang hộp: tên, giá trị (token), path và domain
            cookieContainer.Add(new Cookie(Constants.AUTH_COOKIE_NAME, jwtTokenCookie?.Value, jwtTokenCookie?.Path, jwtTokenCookie?.Domain));

            var client = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri(Config.Api.BaseUrl), // địa chỉ gốc của API
                CookieContainer = cookieContainer, // hộp cookie vừa tạo, để request mang theo token
            });

            return client;
        }

        // xUnit biết phải gọi hàm này ngay sau khi object test class tạo xong
        public async Task InitializeAsync() // IAsyncLifetime
        {
            await Authenticate(Username, Password);
        }

        public virtual void TearDown() { }

        public virtual Task TearDownAsync()
        {
            return Task.CompletedTask; // là một Task “đã xong sẵn”, dùng khi không có gì để làm nhưng vẫn phải trả về Task
        }

        public async Task DisposeAsync() // IAsyncLifetime
        {
            await TearDownAsync(); 
            // phải await vì TeardownAsync() trả về một Task, tức là “lời hứa” rằng việc dọn dẹp sẽ xong trong tương lai,
            // chưa chắc đã xong lúc hàm trả về.
        }

        public void Dispose()
        {
            TearDown();
            Driver.Quit();
        }
    }

    //new OrderTests(...)
    //   │
    //   ├─ BaseTest constructor     → mở trình duyệt, gán Logger/Config
    //   ├─ OrderTests constructor   → (thân con)
    //   │
    //   ▼
    //InitializeAsync()              → đăng nhập, thêm cookie
    //   │
    //   ▼
    //[test ShowOrders chạy]
    //   │
    //   ▼
    //DisposeAsync()                 → TeardownAsync() (hook, mặc định rỗng)
    //   │
    //   ▼
    //Dispose()                      → Teardown() (hook) + đóng trình duyệt
}
