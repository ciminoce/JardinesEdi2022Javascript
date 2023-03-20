[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(JardinesEdi2022.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(JardinesEdi2022.Web.App_Start.NinjectWebCommon), "Stop")]

namespace JardinesEdi2022.Web.App_Start
{
    using System;
    using System.Web;
    using JardinesEdi2022.Datos;
    using JardinesEdi2022.Datos.Facades;
    using JardinesEdi2022.Datos.Repositorios;
    using JardinesEdi2022.Servicios.Facades;
    using JardinesEdi2022.Servicios.Servicios;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IPaisesRepositorio>().To<PaisesRepositorio>().InRequestScope();
            kernel.Bind<IPaisesServicios>().To<PaisesServicios>().InRequestScope();

            kernel.Bind<ICiudadesRepositorio>().To<CiudadesRepositorio>().InRequestScope();
            kernel.Bind<ICiudadesServicios>().To<CiudadesServicios>().InRequestScope();

            kernel.Bind<ICategoriasRepositorio>().To<CategoriasRepositorio>().InRequestScope();
            kernel.Bind<ICategoriasServicios>().To<CategoriasServicios>().InRequestScope();

            kernel.Bind<IClientesRepositorio>().To<ClientesRepositorio>().InRequestScope();
            kernel.Bind<IClientesServicios>().To<ClientesServicios>().InRequestScope();

            kernel.Bind<IUsuariosRepositorio>().To<UsuariosRepositorio>().InRequestScope();
            kernel.Bind<IUsuariosServicios>().To<UsuariosServicios>().InRequestScope();

            kernel.Bind<IProductosRepositorio>().To<ProductosRepositorio>().InRequestScope();
            kernel.Bind<IProductosServicios>().To<ProductosServicios>().InRequestScope();

            kernel.Bind<IProveedoresRepositorio>().To<ProveedoresRepositorio>().InRequestScope();
            kernel.Bind<IProveedoresServicios>().To<ProveedoresServicios>().InRequestScope();

            kernel.Bind<ICarritosRepositorio>().To<CarritosRepositorio>().InRequestScope();
            kernel.Bind<ICarritosServicios>().To<CarritosServicios>().InRequestScope();

            kernel.Bind<IOrdenesRepositorio>().To<OrdenesRepositorio>().InRequestScope();
            kernel.Bind<IOrdenesServicios>().To<OrdenesServicios>().InRequestScope();

            kernel.Bind<IDetalleOrdenesRepositorio>().To<DetalleOrdenesRepositorio>().InRequestScope();


            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<ViveroSqlDbContext>().ToSelf().InThreadScope();

        }
    }
}