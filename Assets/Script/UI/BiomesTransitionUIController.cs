using System.Collections;
using SGGames.Script.Core;
using SGGames.Script.Data;
using SGGames.Script.Events;
using SGGames.Script.TweenSystem;
using UnityEngine;

namespace SGGames.Script.UI
{
    public class BiomesTransitionUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private BiomesTransitionUIEvent m_biomesTransitionUIEvent;
        [SerializeField] private GameEvent m_gameEvent;
        [SerializeField] private Transform m_indicator;
        [SerializeField] private Transform[] m_biomesIcons;

        private Transform m_targetTransform;
        private readonly float m_delayBeforeClosing = 1;
        private readonly float m_movingDurationPerBiomes = 0.5f;
        private readonly float m_scaleDuration = 0.5f;
        private readonly float m_finalScale = 1.5f;
        private Vector2 m_targetPos;
        private Vector2 m_startPos;

        private void Awake()
        {
            m_biomesTransitionUIEvent.AddListener(OnReceiveTransitionEvent);
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Test")]
        private void Test()
        {
            OnReceiveTransitionEvent(2);
        }
        #endif

        private void OnTweenPosition(Vector3 updatePos)
        {
            m_indicator.position = updatePos;
        }

        private void OnTweenScale(Vector3 updateScale)
        {
            m_targetTransform.localScale = updateScale;
        }
        
        private void TweenIconScale()
        {
            TweenManager.Instance.TweenVector3(Vector3.one, Vector3.one * m_finalScale, OnTweenScale,
                m_scaleDuration, Global.EaseType.EaseOutBack)
                .OnComplete(OnCompleteAllTween);
        }

        private void OnCompleteAllTween()
        {
            StartCoroutine(OnClosingBiomeTransitionUI());
        }

        private IEnumerator OnClosingBiomeTransitionUI()
        {
            yield return new WaitForSeconds(m_delayBeforeClosing);
            m_canvasGroup.alpha = 0;
            m_gameEvent.Raise(Global.GameEventType.LoadNextBiomes);
        }
        
        private void SetTargetPosition(Transform target)
        {
            var nextPos = m_indicator.position;
            nextPos.x = target.position.x;
            m_targetPos = nextPos;
        }
        
        private void OnReceiveTransitionEvent(int nextBiomes)
        {
            m_canvasGroup.alpha = 1;
            
            m_startPos = m_indicator.position;
            SetTargetPosition(m_biomesIcons[nextBiomes]);
            m_targetTransform = m_biomesIcons[nextBiomes];

            var movingDuration = m_movingDurationPerBiomes * nextBiomes;
            
            TweenManager.Instance.TweenVector3(m_startPos, m_targetPos, OnTweenPosition,
                    movingDuration ,Global.EaseType.EaseOutCubic)
                .OnComplete(TweenIconScale);
        }
    }
}
