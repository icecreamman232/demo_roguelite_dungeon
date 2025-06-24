using System;
using System.Collections.Generic;

namespace SGGames.Script.Core
{
    public class ServiceLocator : Singleton<ServiceLocator>
    {
        private static Dictionary<Type,IGameService> m_services = new Dictionary<Type,IGameService>();

        public static void RegisterService<T>(IGameService service) where T : IGameService
        {
            if (m_services.ContainsKey(typeof(T)))
            {
                return;
            }
            m_services.Add(typeof(T),service);
        }

        public static T GetService<T>() where T : IGameService
        {
            if (m_services.ContainsKey(typeof(T)))
            {
                return (T)m_services[typeof(T)];
            }
            throw new KeyNotFoundException("Service not registered");
        }

        public static void UnregisterService<T>()
        {
            if (m_services.ContainsKey(typeof(T)))
            {
                m_services.Remove(typeof(T));
            }
        }
    }
}

