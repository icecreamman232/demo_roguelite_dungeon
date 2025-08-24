using System;
using SGGames.Scrips.Events;
using TMPro;
using UnityEngine;

namespace SGGames.Scrips.UI
{
    public class CheatCodeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_roomName;
        [SerializeField] private ShowCheatCodeUIEvent m_showCheatCodeUIEvent;

        private void Awake()
        {
            m_showCheatCodeUIEvent.AddListener(OnCheatCodeUIChanged);
            m_roomName.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            m_showCheatCodeUIEvent.RemoveListener(OnCheatCodeUIChanged);
        }

        private void OnCheatCodeUIChanged(CheatCodeUIData cheatCodeUIData)
        {
            #if DEVELOPMENT_BUILD
            if (!string.IsNullOrEmpty(cheatCodeUIData.RoomName))
            {
                m_roomName.gameObject.SetActive(true);
                m_roomName.text = "<color=yellow>Debug Info</color>\n" + "Room: " + cheatCodeUIData.RoomName;
            }
            #endif
        }
    }
}
