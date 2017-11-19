using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using API.Operations;
using Models;
using Models.Operations;
using Ninject;
using Ninject.Web.Common;

namespace API.Common
{
    public class NinjectDependencyResolver : IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<OrderOperations>().ToSelf().InRequestScope();
            kernel.Bind<CommentOperations>().ToSelf().InRequestScope();
            kernel.Bind<FileOperations>().ToSelf().InRequestScope();
            kernel.Bind<PaymentOperations>().ToSelf().InRequestScope();
            kernel.Bind<UserOperations>().ToSelf().InRequestScope();
            kernel.Bind<LrdrContext>().ToSelf().InRequestScope();
        }

        public void Dispose()
        {
      
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(this.kernel.BeginBlock());
        }
    }
}