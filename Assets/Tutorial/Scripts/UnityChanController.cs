using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;

public class UnityChanController : MonoBehaviour
{
    public float blendAnimatorRate = 1f;

    private MxMAnimator m_MxMAnimator;

    private bool m_IsMotionMatching = true;

    private void Awake()
    {
        m_MxMAnimator = GetComponent<MxMAnimator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_IsMotionMatching = !m_IsMotionMatching;
            if (m_IsMotionMatching)
            {
                m_MxMAnimator.BlendOutController(blendAnimatorRate);
            }
            else
            {
                m_MxMAnimator.BlendInController(blendAnimatorRate);
            }
        }
    }
}
