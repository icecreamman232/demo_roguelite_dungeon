using TMPro;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class CoinUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_coinText;
        
        public void UpdateCoinView(int amount)
        {
            m_coinText.text = amount.ToString();
        }
    }
}

