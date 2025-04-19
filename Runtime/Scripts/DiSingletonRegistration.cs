using System;
using UnityEngine;

namespace DI.Runtime
{
    internal class DiSingletonRegistration : IDiRegistration
    {
        private readonly Func<object> m_factory;
        private object m_instance;
        
        internal DiSingletonRegistration(Func<object> factory)
        {
            m_factory = factory;
        }
        
        public T GetRegisteredObject<T>()
        {
            m_instance ??= m_factory.Invoke();

            return (T)m_instance;
        }
    }
}