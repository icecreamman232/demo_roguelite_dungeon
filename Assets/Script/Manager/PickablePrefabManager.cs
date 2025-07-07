using SGGames.Script.Core;
using SGGames.Script.Data;
using UnityEngine;

namespace SGGames.Script.Managers
{
    public class PickablePrefabManager : MonoBehaviour, IGameService
    {
        [SerializeField] private PickableContainer m_container;
        [SerializeField] private ObjectPooler m_coinPooler;
        
        private void Awake()
        {
            ServiceLocator.RegisterService<PickablePrefabManager>(this);
        }

        public GameObject GetPrefabWith(Global.ItemID id)
        {
            if (id == Global.ItemID.Coin)
            {
                return m_coinPooler.GetPooledGameObject();
            }
            return m_container.GetPrefabWithID(id);
        }
    }
}
    
