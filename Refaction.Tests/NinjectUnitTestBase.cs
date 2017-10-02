﻿using System;
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
        protected IKernel CurrentNinjectKernel { get; set; }

        public NinjectUnitTestBase()
        {
            var kernel = new StandardKernel();

            ResetNinjectKernel(kernel);
        }

        protected void ResetNinjectKernel(IKernel kernel)
        {
            CurrentNinjectKernel = kernel;
            CurrentNinjectKernel.Load(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Removes Ninject bindings for IRefactionDbContext
        /// </summary>
        protected void RemovePriorBindings(Type service)
        {
            NinjectHelper.RemovePriorBindings(CurrentNinjectKernel, service);
        }

    }
}