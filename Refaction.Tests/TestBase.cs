using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.UnitTests
{
    /// <summary>
    /// Creates a TestContext property for derived test classes
    /// </summary>
    public abstract class TestBase : IDisposable
    {
        public TestContext TestContext { get; set; } // automatically bound to the current test context at test time

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            // do nothing, allow derived classes to override dispose method
        }

        ~TestBase() {
           Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
