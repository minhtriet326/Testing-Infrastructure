using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace testing_core.Attributes.Retry
{
    [XunitTestCaseDiscoverer("testing_core.Attributes.Retry.RetryTheoryDiscoverer", "testing-core")]
    public class RetryTheoryAttribute : TheoryAttribute
    {
        public int MaxRetries { get; set; }
    }
}
