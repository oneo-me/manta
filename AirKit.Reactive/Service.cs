using AirKit.Reactive.Exceptions;

namespace AirKit.Reactive;

public static class Service
{
    static readonly Dictionary<Type, object> _dic = new();

    static object GetInstance(Type type)
    {
        if (_dic.TryGetValue(type, out var instance))
            return instance;

        throw new ServiceException($"服务 {type} 未注册");
    }

    static void RegisterCore(Type serviceType, Type serviceImplType, object serviceImpl)
    {
        if (serviceType.IsAssignableFrom(serviceImplType) is false)
            throw new ServiceException($"服务注册出现错误，{serviceImplType} 不是 {serviceType} 的子类");

        if (_dic.ContainsKey(serviceType) || _dic.ContainsKey(serviceImplType))
            throw new ServiceException($"服务注册出现错误，{serviceType} 已经注册为 {_dic[serviceType].GetType()}");

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
        var instance = GetInstance(typeof(T));
        if (instance is not T impl)
            throw new ServiceException($"获取服务 {typeof(T)} 出现错误，依赖了未注册的服务 {typeof(T)}");

        return impl;
    }

    public static T CreateInstance<T>(Type type) where T : class
    {
        var constructorInfo = type.GetConstructors().MinBy(x => x.GetParameters().Length);
        if (constructorInfo is null)
            throw new ServiceException($"创建对象出现错误，{type} 没有没用可用的构造函数");

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
            throw new ServiceException($"创建对象出现错误，{type} 不是 {typeof(T)} 的子类");

        return result;
    }

    public static T CreateInstance<T>() where T : class, new()
    {
        return CreateInstance<T>(typeof(T));
    }
}
