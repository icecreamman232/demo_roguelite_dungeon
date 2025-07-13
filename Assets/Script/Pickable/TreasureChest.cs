using SGGames.Script.Modules;
using UnityEngine;

namespace SGGames.Script.Pickable
{
    public class TreasureChest : MonoBehaviour, IInteractable
    {
        [SerializeField] private DropLootController m_dropController;
        
        private void OpenChest()
        {
            m_dropController.SpawnLoot();
            this.gameObject.SetActive(false);
        }
        
        public void Interact()
        {
            OpenChest();
        }
    }
}
