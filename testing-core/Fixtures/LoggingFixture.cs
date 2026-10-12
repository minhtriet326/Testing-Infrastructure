using AventStack.ExtentReports;
using core.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testing_core.Logging;

namespace testing_core.Fixtures
{
    public class LoggingFixture : IDisposable
    {
        /// <summary>
        /// Gets the logger instance for logging test information.
        /// </summary>
        public ITestLogger Logger { get; private set; }

        private ExtentReports extentReports;

        public LoggingFixture()
        {
            // Initialize the ExtentReports instance
            extentReports = new ExtentReports();

            // tạo một "adapter" để chuyển đổi các lệnh logging thông thường thành format mà ExtentReports hiểu được
            Logger = new ExtentTestLogger(extentReports);
        }
        public void Dispose()
        {
            // Bắt buộc phải gọi Flush() để ghi dữ liệu vào file HTML, nếu không sẽ không có dữ liệu được ghi ra
            Logger.Flush();

            // Cách nào cũng được
            //extentReports.Flush();
        }
    }
}
