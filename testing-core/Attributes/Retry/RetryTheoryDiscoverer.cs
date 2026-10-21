using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace testing_core.Attributes.Retry
{
    public class RetryTheoryDiscoverer : TheoryDiscoverer
    {
        public RetryTheoryDiscoverer(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
            // trong class cha TheoryDiscoverer, nó cũng có 1 property IMessageSink diagnosticMessageSink, nên ta không cần khai báo lại như bên RetryFactDiscoverer, mà chỉ cần gọi base constructor để gán giá trị cho property đó.
        }

        // method này được gọi khi xUnit phát hiện một test method có attribute [RetryTheory] và nó sẽ gọi method này 1 lần với mỗi row data.
        protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(
            ITestFrameworkDiscoveryOptions discoveryOptions, 
            ITestMethod testMethod, 
            IAttributeInfo theoryAttribute, 
            object[] dataRow
        )
        {
            var maxRetries = theoryAttribute.GetNamedArgument<int>("MaxRetries");

            if (maxRetries < 1 || maxRetries > 10)
            {
                maxRetries = 3;
            }

            yield return new RetryTestCase(
                DiagnosticMessageSink,
                discoveryOptions.MethodDisplayOrDefault(),
                discoveryOptions.MethodDisplayOptionsOrDefault(),
                testMethod,
                maxRetries,
                dataRow
            );
        }
    }
}
