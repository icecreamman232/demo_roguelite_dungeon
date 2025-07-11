using TMPro;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class KeyUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_keyText;
        
        public void UpdateCoinView(int amount)
        {
            m_keyText.text = amount.ToString();
        }
    }
}