using System;
using UnityEngine;

public class TransformModifier : MonoBehaviour
{
    [SerializeField] private Vector3 m_position;
    [SerializeField] private Quaternion m_rotation;
    [SerializeField] private Vector3 m_scalar;

    
    private Vector3 m_originalPosition;
    private Quaternion m_originalRotation;
    private Vector3 m_originalScale;

    private void Start()
    {
        m_originalPosition = transform.position;
        m_originalRotation = transform.rotation;
        m_originalScale = transform.localScale;
    }

    public void Reset()
    {
        transform.localPosition = m_originalPosition;
        transform.rotation = m_originalRotation;
        transform.localScale = m_originalScale;
    }
    
    public void Apply()
    {
        transform.localPosition = m_position;
        transform.rotation = m_rotation;
        transform.localScale = m_scalar;
    }
}
