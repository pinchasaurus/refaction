using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Ninject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refaction.Common;

namespace Refaction.UnitTests
{
    /// <summary>
    /// Creates a Ninject Kernel for derived test classes
    /// </summary>
    public abstract partial class NinjectTestBase : TestBase
    {
        private IKernel _ninjectKernel;

        protected IKernel NinjectKernel
        {
            get { return _ninjectKernel; }
        }

        public NinjectTestBase()
        {
            _ninjectKernel = new StandardKernel();
            _ninjectKernel.Load(Assembly.GetExecutingAssembly());
        }
    }
}
