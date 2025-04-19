using System;

namespace DI.Runtime
{
    internal class DiTransientRegistration : IDiRegistration
    {
        private readonly Func<object> m_factory;

        internal DiTransientRegistration(Func< object> factory)
        {
            m_factory = factory;
        }
        
        public T GetRegisteredObject<T>()
        {
            return  (T)m_factory.Invoke();
        }
    }
}