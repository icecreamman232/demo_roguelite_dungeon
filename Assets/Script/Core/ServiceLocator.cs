using System;
using System.Collections.Generic;
using SGGames.Scripts.UI;

namespace SGGames.Scripts.Core
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
        
        public static bool HasService<T>() where T : IGameService
        {
            return m_services.ContainsKey(typeof(T));
        }

        public static void UnregisterService<T>()
        {
            if (m_services.ContainsKey(typeof(T)))
            {
                m_services.Remove(typeof(T));
            }
        }
        
        public static void ClearServices()
        {
            // Save the LoadingScreenController if it exists
            IGameService loadingScreenController = null;
            if (m_services.ContainsKey(typeof(LoadingScreenController)))
            {
                loadingScreenController = m_services[typeof(LoadingScreenController)];
            }
            
            // Clear all services
            m_services.Clear();
            
            // Re-register the LoadingScreenController if it existed
            if (loadingScreenController != null)
            {
                m_services.Add(typeof(LoadingScreenController), loadingScreenController);
            }

            
        }
    }
}

