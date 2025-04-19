using System;

namespace DI.Runtime
{
    internal class DiInstanceRegistration : IDiRegistration 
    {
        private readonly object m_instance;
        
        internal DiInstanceRegistration(object instance)
        {
            m_instance = instance;
        }
        
        public T GetRegisteredObject<T>()
        {
            return (T)m_instance;
        }
    }
}