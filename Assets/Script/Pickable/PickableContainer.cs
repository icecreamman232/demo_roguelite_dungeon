using System.Collections.Generic;
using UnityEngine;

namespace SGGames.Script.Data
{
    [CreateAssetMenu(fileName = "Pickable Container", menuName = "SGGames/Data/Pickable Container")]
    public class PickableContainer : IDataContainer<GameObject>
    {
        [SerializeField] private List<GameObject> m_autoPickableList;
        [SerializeField] private List<GameObject> m_manualPickableList;
        
        public List<GameObject> AutoPickableList => m_autoPickableList;
        public List<GameObject> ManualPickableList => m_manualPickableList;


        public void AddAutoPickable(GameObject item)
        {
            m_autoPickableList.Add(item);
        }

        public void AddManualPickable(GameObject item)
        {
            m_manualPickableList.Add(item);
        }

        public void ClearContainer()
        {
            m_container.Clear();
            m_autoPickableList.Clear();
            m_manualPickableList.Clear();
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
