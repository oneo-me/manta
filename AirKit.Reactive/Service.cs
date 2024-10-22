using AirKit.Reactive.Exceptions;
using AirKit.Reactive.Logging;

namespace AirKit.Reactive;

public static class Service
{
    static readonly Dictionary<Type, object> _dic = new();

    static object GetInstance(Type type)
    {
        if (_dic.TryGetValue(type, out var instance))
            return instance;

        throw new ServiceException($"Service {type} not registered");
    }

    static void RegisterCore(Type serviceType, Type serviceImplType, object serviceImpl)
    {
        Log.Verbose("Service", "Register service {serviceType} as {serviceImplType}", serviceType, serviceImplType);

        if (serviceType.IsAssignableFrom(serviceImplType) is false)
            throw new ServiceException($"Service registration error, {serviceImplType} is not a subclass of {serviceType}");

        if (_dic.ContainsKey(serviceType) || _dic.ContainsKey(serviceImplType))
            throw new ServiceException($"Service registration error, {serviceType} has already been registered as {_dic[serviceType].GetType()}");

        _dic[serviceType] = serviceImpl;
        _dic[serviceImplType] = serviceImpl;
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

        Log.Verbose("Service", "Get service {serviceType}", serviceType);

        var instance = GetInstance(serviceType);
        if (instance is not T impl)
            throw new ServiceException($"An error occurred while obtaining the service {serviceType}, which depends on an unregistered service {serviceType}");

        return impl;
    }

    public static T CreateInstance<T>(Type type) where T : class
    {
        Log.Verbose("Service", "Create instance {type}", type);

        var constructorInfo = type.GetConstructors().MinBy(x => x.GetParameters().Length);
        if (constructorInfo is null)
            throw new ServiceException($"Error creating object, {type} has no available constructor");

        var parameterInfos = constructorInfo.GetParameters();
        var args = new List<object?>();

        foreach (var parameterInfo in parameterInfos)
        {
            var parameterType = parameterInfo.ParameterType;
            var instance = GetInstance(parameterType);
            args.Add(instance);
        }

        var obj = Activator.CreateInstance(type, args.ToArray());
        if (obj is not T result)
            throw new ServiceException($"An error occurred while creating the object, {type} is not a subclass of {typeof(T)}");

        return result;
    }

    public static T CreateInstance<T>() where T : class, new()
    {
        return CreateInstance<T>(typeof(T));
    }
}
