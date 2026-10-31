using core.Configuration;
using core.Logging.Interfaces;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_browser.WebDriver
{
    public class DriverFactory
    {
        public WebDriver CreateDriver(DriverConfig config, ITestLogger logger)
        {
            switch (config.Type)
            {
                case "Chrome":
                    return new WebDriver(new ChromeDriver(GetChromeOptions(config)), logger);
                case "FireFox":
                    return new WebDriver(new FirefoxDriver(GetFirefoxOptions(config)), logger);
                default:
                    throw new ArgumentException($"Unsuppoted browser: {config.Type}.");
            }
        }
        public ChromeOptions GetChromeOptions(DriverConfig config)
        {
            // Lấy một tờ giấy trống.
            // Dòng này tạo ra một ChromeOptions rỗng, chưa có cài đặt nào.
            // Từ đây trở đi, mỗi dòng chromeOptions.Something(...) là ghi thêm một cài đặt vào tờ giấy.
            ChromeOptions options = new ChromeOptions();

            if (config.Headless)
            {
                options.AddArgument("--headless=new"); // chạy không UI
            }

            string downloadPath = GetAbsoluteDownloadPath(config);

            options.AddUserProfilePreference("download.default_directory", downloadPath);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("directory_upgrade", true);
            options.AddUserProfilePreference("safebrowsing.enabled", true);

            options.AddArguments(config.Arguments);

            return new ChromeOptions();
        }
        public FirefoxOptions GetFirefoxOptions(DriverConfig config)
        {
            var options = new FirefoxOptions();

            if (config.Headless)
            {
                options.AddArgument("-headless");
            }

            var profile = new FirefoxProfile();

            profile.SetPreference("browser.download.folderList", 2);
            profile.SetPreference(
                "browser.helperApps.neverAsk.saveToDisk",
                "application/pdf,application/octet-stream"
                );

            string downloadPath = GetAbsoluteDownloadPath(config);

            profile.SetPreference("browser.download.dir", downloadPath);

            options.Profile = profile;

            options.AddArguments(config.Arguments);

            return options;
        }
        private static string GetAbsoluteDownloadPath(DriverConfig config)
        {
            return Path.IsPathRooted(config.DownloadPath)
                ? config.DownloadPath
                : Path.Combine(Directory.GetCurrentDirectory(), config.DownloadPath);
        }
    }
}
