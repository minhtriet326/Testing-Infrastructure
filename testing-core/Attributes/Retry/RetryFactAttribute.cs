using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace testing_core.Attributes.Retry
{
    // Class này là một custom attribute(~ annotation) để đánh dấu các test method trong xUnit mà bạn muốn retry khi test thất bại.
    // Nó bảo xUnit: “khi gặp [RetryFact], đừng dùng discoverer mặc định, hãy dùng class này”
    // [XunitTestCaseDiscoverer(...)] trên attribute bắt buộc trỏ tới một discoverer
    [XunitTestCaseDiscoverer("testing_core.Attributes.Retry.RetryFactDiscoverer", "testing-core")]
    public class RetryFactAttribute : FactAttribute
    {
        public int MaxRetries { get; }
        public RetryFactAttribute(int maxRetries = 3)
        {
            MaxRetries = maxRetries;
        }
    }
}
