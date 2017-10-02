using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refaction.Common
{
    public static class NinjectHelper
    {
        /// <summary>
        /// Removes Ninject bindings for IRefactionDbContext
        /// </summary>
        public static void RemovePriorBindings(IKernel kernel, Type service)
        {
            var bindings = kernel.GetBindings(service);
            foreach (var binding in bindings)
            {
                kernel.RemoveBinding(binding);
            }
        }
    }
}
