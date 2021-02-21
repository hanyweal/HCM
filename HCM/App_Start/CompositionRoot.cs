using HCM.DI;
using HCM.DI.Ninject;
using HCM.DI.Ninject.Modules;
using Ninject;

internal class CompositionRoot
{
    public static IDependencyInjectionContainer Compose()
    {
        // Create the DI container
        var container = new StandardKernel();

        // Setup configuration of DI
        container.Load(new MvcSiteMapProviderModule());

        // Return our DI container wrapper instance
        return new NinjectDependencyInjectionContainer(container);
    }
}

