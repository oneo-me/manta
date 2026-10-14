using System.Collections.Concurrent;

namespace Manta.Reactive.Services;

public static class Service
{
    static readonly ConcurrentDictionary<Type, object> _dic = new();
    static readonly Lock _registerLock = new();

    static object GetInstance(Type type)
    {
        lock (_registerLock)
        {
            if (_dic.TryGetValue(type, out var instance))
                return instance;
        }

        throw new ServiceException($"Service {type} not registered");
    }

    static void RegisterCore(Type serviceType, Type serviceImplType, object serviceImpl)
    {
        if (serviceType.IsAssignableFrom(serviceImplType) is false)
            throw new ServiceException($"Service registration error, {serviceImplType} is not a subclass of {serviceType}");

        lock (_registerLock)
        {
            if (_dic.ContainsKey(serviceType) || _dic.ContainsKey(serviceImplType))
                throw new ServiceException($"Service registration error, {serviceType} has already been registered as {_dic[serviceType].GetType()}");

            _dic[serviceType] = serviceImpl;
            _dic[serviceImplType] = serviceImpl;
        }
    }

    public static T Register<T>(T serviceImpl) where T : class
    {
        var serviceType = typeof(T);
        var serviceImplType = serviceImpl.GetType();

        RegisterCore(serviceType, serviceImplType, serviceImpl);

        return serviceImpl;
    }

    public static TImpl Register<T, TImpl>(TImpl serviceImpl) where TImpl : class, T
    {
        var serviceType = typeof(T);
        var serviceImplType = typeof(TImpl);

        RegisterCore(serviceType, serviceImplType, serviceImpl);

        return serviceImpl;
    }

    public static T GetService<T>() where T : class
    {
        var serviceType = typeof(T);

        var instance = GetInstance(serviceType);
        if (instance is not T impl)
            throw new ServiceException($"An error occurred while obtaining the service {serviceType}, which depends on an unregistered service {serviceType}");

        return impl;
    }
}
