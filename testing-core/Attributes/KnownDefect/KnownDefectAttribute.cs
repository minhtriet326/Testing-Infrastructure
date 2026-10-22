using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace testing_core.Attributes.KnownDefect
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)] // Chỉ cho gán trên method và không cho phép gán nhiều lần trên cùng một method.
    // Attribute to mark a [Fact] test as a known defect, allowing it to be skipped during test execution.
    public class KnownDefectAttribute : FactAttribute
    {
        public KnownDefectAttribute(string defectId = "Known defect") // defectId nhận text bên [KnownDefect("BUG-123")]
        {
            // Provided that Skip is set to a non-null value, the test will be skipped and the provided defect ID will be displayed in the test results.
            Skip = defectId;
        }
    }
}
