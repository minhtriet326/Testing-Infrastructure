using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Logging.Interfaces
{
    public interface ITestLogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(Exception ex);
        void LogPass(string message);
        void LogFail(string message);

        /// <summary>
        /// Starts a new log session for the specified test case.
        /// </summary>
        /// <param name="testName">The name of the test method.</param>
        void NewLog(string testName);
        void LogScreenshot(string base64EncodedScreenshotImage);
        void Flush();
    }
}
