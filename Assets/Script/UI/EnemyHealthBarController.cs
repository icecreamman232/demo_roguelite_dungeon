using UnityEngine;

namespace SGGames.Script.UI
{
    public class EnemyHealthBarController : MonoBehaviour
    {
        [SerializeField] private EnemyHealthBarView m_view;

        public void UpdateHealthBar(float currentHealth,float maxHealth)
        {
            m_view.UpdateHealthBarVisual(currentHealth, maxHealth);
        }
    }
}

