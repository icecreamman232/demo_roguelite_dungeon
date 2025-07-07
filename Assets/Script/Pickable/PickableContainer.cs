using System.Collections.Generic;
using SGGames.Script.Core;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Pickable Container", menuName = "SGGames/Data/Pickable Container")]
    public class PickableContainer : IDataContainer<GameObject>
    {
        [SerializeField] private List<GameObject> m_autoPickableList;
        [SerializeField] private List<GameObject> m_manualPickableList;
        
        private Dictionary<Global.ItemID, GameObject> m_dictionary = new Dictionary<Global.ItemID, GameObject>();
        
        
        public List<GameObject> AutoPickableList => m_autoPickableList;
        public List<GameObject> ManualPickableList => m_manualPickableList;


        public GameObject GetPrefabWithID(Global.ItemID id)
        {
            m_dictionary.TryGetValue(id, out GameObject prefab);
            return prefab;
        }
        
        public void AddPickable(GameObject pickable, Global.ItemID itemID)
        {
            m_container.Add(pickable);
            m_dictionary.Add(itemID, pickable);
        }
        

        public void AddAutoPickable(GameObject item)
        {
            m_autoPickableList.Add(item);
        }

        public void AddManualPickable(GameObject item)
        {
            m_manualPickableList.Add(item);
        }

        public void ClearAllContainer()
        {
            m_container.Clear();
            m_autoPickableList.Clear();
            m_manualPickableList.Clear();
            m_dictionary.Clear();
        }

        public void ClearAutoContainer()
        {
            m_autoPickableList.Clear();
        }
        
        public void ClearManualContainer()
        {
            m_manualPickableList.Clear();
        }
    }
}
