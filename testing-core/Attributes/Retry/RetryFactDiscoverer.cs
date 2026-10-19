using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace testing_core.Attributes.Retry
{
    // Mặc định, với [Fact], xUnit tự tạo XunitTestCase bình thường (chạy 1 lần).
    // Attribute chỉ là dữ liệu, nó không thể tự quyết định xUnit tạo loại test case nào.
    // Cần một class đứng giữa để nói: “gặp attribute này thì tạo RetryTestCase, không phải XunitTestCase”.
    // Class đó chính là discoverer.

    // => Nhiệm vụ chính là tạo ra RetryTestCase thay vì XunitTestCase khi gặp [RetryFact].
    public class RetryFactDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink diagnosticMessageSink;

        public RetryFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<IXunitTestCase> Discover(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod,
            IAttributeInfo factAttribute)
        {
            int maxRetries = factAttribute.GetNamedArgument<int>("MaxRetries"); // Lấy giá trị MaxRetries từ attribute [RetryFact(MaxRetries = 5)].

            if (maxRetries < 1 || maxRetries > 10)
            {
                maxRetries = 3; // Default to 3 if out of range
            }

            yield return new RetryTestCase(
                diagnosticMessageSink, 
                discoveryOptions.MethodDisplayOrDefault(),
                discoveryOptions.MethodDisplayOptionsOrDefault(),
                testMethod, 
                maxRetries);
        }
    }
}
