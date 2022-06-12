using System;

namespace com.liteninja.di.Core.DI
{
    public interface IServiceProvider
    {
        //the Type this service provider is managing
        Type ServiceType { get; }

        //whether this service provider can create an instance of the service without errors (calling Get() succeeds)
        bool CanGet();

        //whether there already exists an instance of the service (calling Get() would not create an instance).
        bool Exists();

        //tear down the service (destroy the instance if possible). Used to clean up. Get() after should return a clean instance.
        void TearDown();
    }

    public interface IServiceProvider<out T> : IServiceProvider where T : class
    {
        //get an instance of the service. Might be created lazily and cached. Might throw if service cannot be created.
        T Get();
    }
}