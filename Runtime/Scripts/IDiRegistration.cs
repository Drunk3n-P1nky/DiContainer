namespace DI.Runtime
{
    internal interface IDiRegistration
    {
        T GetRegisteredObject<T>();
    }
}