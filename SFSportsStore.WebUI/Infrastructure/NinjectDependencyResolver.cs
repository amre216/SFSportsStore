using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Moq;
using SFSportsStore.Domain.Entities;
using SFSportsStore.Domain.Abstract;
using SFSportsStore.Domain.Concrete;

namespace SFSportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
       
        private IKernel kernel;

        //Accept a ninject kernel as param for init. of ninject resolver
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        //Get instance of object for specific type 
        //I.e this gets the concrete implementation object for a binding
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        //Get instance of objects for specific type 
        //I.e this gets the concrete implementation objects for a binding 
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //Add bindings

            ////Add temp mock I product repo:
            ////Mock object of type <IProductsRepository>
            //Mock<IProductsRepository> mockProdList = new Mock<IProductsRepository>();

            ////Init the mock prod repo, returning a list of products, implementing the IProductsRepository interface
            //mockProdList.Setup(m => m.Products).Returns(new List<Product> { 
            //    new Product { Name = "Football", Price = 25 },
            //    new Product { Name = "Surfboard", Price = 179 },
            //    new Product { Name = "Runningshoes", Price = 95 }
            //});

            ////Binding: services implementing IProductsRepository resolve as our mockProdList object.
            //kernel.Bind<IProductsRepository>().ToConstant(mockProdList.Object);

            //Binding: Services implementing IProductsRepository will resolve to the entity framework product repository
            kernel.Bind<IProductsRepository>().To<EFProductRepository>();
        }
    }
}