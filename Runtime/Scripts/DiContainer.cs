using System;
using System.Collections.Generic;

namespace DI.Runtime
{
    public class DiContainer
    {
        private readonly DiContainer m_parentContainer;
        private readonly Dictionary<(string tag, Type type), IDiRegistration> m_registrations = new();
        private readonly HashSet<(string tag, Type type)> m_currentResolutions = new();

        public DiContainer(DiContainer parentContainer = null)
        {
            m_parentContainer = parentContainer;
        }

        public void RegisterSingleton<T>(Func<T> factory, string tag = null)
        {
            var key = (tag, typeof(T));
            ValidateRegistrationAttempt(key);
            m_registrations[key] = new DiSingletonRegistration(() => factory());
        }

        public void RegisterTransient<T>(Func<T> factory, string tag = null)
        {
            var key = (tag, typeof(T));
            ValidateRegistrationAttempt(key);
            m_registrations[key] = new DiTransientRegistration(() => factory());
        }

        public void RegisterInstance<T>(T instance, string tag = null)
        {
            var key = (tag, typeof(T));
            ValidateRegistrationAttempt(key);
            m_registrations[key] = new DiInstanceRegistration(instance);
        }
        
        public T Resolve<T>(string tag = null)
        {
            (string tag, Type type) key = (tag, typeof(T));

            if (!m_currentResolutions.Add(key))
            {
                throw new Exception($"Circular references found. Tag: {key.tag}. Type: ${key.type.FullName}.");
            }

            try
            {
                if (m_registrations.TryGetValue(key, out var registration))
                {
                    return registration.GetRegisteredObject<T>();
                }

                if (m_parentContainer != null)
                {
                    return m_parentContainer.Resolve<T>(tag);
                }
            }
            finally
            {
                m_currentResolutions.Remove(key);
            }
            
            throw new KeyNotFoundException($"Dependency was not found. Tag: {key.tag},  Type: {key.type.FullName}.");
        }

        private void ValidateRegistrationAttempt((string tag, Type type) key)
        {
            if (m_registrations.ContainsKey(key))
            {
                throw new Exception($"Duplicate registration for {key.tag} and type {key.type.FullName}.");
            }
        }
    }
}