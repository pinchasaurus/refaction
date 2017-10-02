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
    public abstract class NinjectUnitTestBase : UnitTestBase
    {
        private IKernel _ninjectKernel;

        protected IKernel NinjectKernel
        {
            get { return _ninjectKernel; }
        }

        public NinjectUnitTestBase()
        {
            _ninjectKernel = new StandardKernel();
            _ninjectKernel.Load(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Removes Ninject bindings for IRefactionDbContext
        /// </summary>
        protected void RemovePriorBindings(Type service)
        {
            NinjectHelper.RemovePriorBindings(NinjectKernel, service);
        }

    }
}
