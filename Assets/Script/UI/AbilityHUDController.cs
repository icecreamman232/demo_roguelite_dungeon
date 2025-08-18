using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Managers;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class AbilityHUDController : MonoBehaviour
    {
        [SerializeField] private AbilityHUDView m_view;

        private IEnumerator Start()
        {
            yield return new WaitUntil(HasPlayerCreated);
            Initialize();
        }

        public void DisplayRange()
        {
            
        }

        private bool HasPlayerCreated()
        {
            if (ServiceLocator.HasService<LevelManager>())
            {
                var levelManager = ServiceLocator.GetService<LevelManager>();
                return levelManager.Player != null;
            }
            return false;
        }
        
        private void Initialize()
        {
            
        }
    }
}
