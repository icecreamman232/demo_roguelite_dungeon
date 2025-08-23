using SGGames.Script.Modules;
using SGGames.Scripts.Core;
using SGGames.Scripts.Data;
using UnityEngine;

namespace SGGames.Scripts.Modules
{
    public class FloatingTextSpawner : MonoBehaviour
    {
        [SerializeField] private float m_textLifeTime;
        [SerializeField] private ObjectPooler m_textPooler;
        [SerializeField] private FloatingTextEvent m_floatingTextEvent;

        private void Start()
        {
            m_floatingTextEvent.AddListener(OnReceiveSpawnEvent);
        }
        
        private void OnDestroy()
        {
            m_floatingTextEvent.RemoveListener(OnReceiveSpawnEvent);
        }
        
        private void OnReceiveSpawnEvent(FloatingTextData floatingTextData)
        {
            var textObject = m_textPooler.GetPooledGameObject();
            var floatingText = textObject.GetComponent<FloatingText>();
            floatingText.SetupFloatingText(floatingTextData.Content, floatingTextData.Position, m_textLifeTime, floatingTextData.Color);
        }
    }
}
