namespace Manta.Extensions;

public static class TypeExtensions
{
    extension(Type type)
    {
        public T CreateInstance<T>()
        {
            var obj = Activator.CreateInstance(type);
            if (obj is not T instance)
                throw new InvalidOperationException($"Cannot create an instance of type {type.FullName} as {typeof(T).FullName}");

            return instance;
        }
    }
}
