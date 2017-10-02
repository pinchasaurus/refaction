using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.UnitTests
{
    public abstract class UnitTestBase
    {
        public TestContext TestContext { get; set; } // automatically bound to the current test context at test time
    }
}
