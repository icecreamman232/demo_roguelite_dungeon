using System.Collections;
using SGGames.Scripts.Core;
using SGGames.Scripts.Entities;
using SGGames.Scripts.Events;
using SGGames.Script.Managers;
using SGGames.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class ActionPointUI : MonoBehaviour
    {
        [SerializeField] private UpdateActionPointUIEvent m_updateActionPointUIEvent;
        [SerializeField] private TextMeshProUGUI m_actionPointText;
        private PlayerController m_playerController;

        private void Awake()
        {
            m_updateActionPointUIEvent.AddListener(OnUpdateActionPointText);
            var inputManager = ServiceLocator.GetService<InputManager>();
            inputManager.OnPressEndTurn += EndTurn;
        }

        private void OnDestroy()
        {
            m_updateActionPointUIEvent.RemoveListener(OnUpdateActionPointText);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => ServiceLocator.HasService<LevelManager>());
            yield return new WaitUntil(() => ServiceLocator.GetService<LevelManager>().Player);
            var playerGO = ServiceLocator.GetService<LevelManager>().Player;
            m_playerController = playerGO.GetComponent<PlayerController>();
        }

        private void OnUpdateActionPointText(ActionPointUIData actionPointUIData)
        {
            m_actionPointText.text = $"AP: {actionPointUIData.CurrentActionPoint}/{actionPointUIData.MaxActionPoint}";
        }

        public void EndTurn()
        {
            if(m_playerController == null) return;
            m_playerController.FinishedTurn();
        }
    }
}
