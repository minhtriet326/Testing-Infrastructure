using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace testing_core.Attributes.KnownDefect
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class KnownDefectTheoryAttribute : TheoryAttribute
    {
        public KnownDefectTheoryAttribute(string defectId = "Known defect") // defectId nhận text bên [KnownDefect("BUG-123")]
        {
            Skip = defectId;
        }
    }
}
