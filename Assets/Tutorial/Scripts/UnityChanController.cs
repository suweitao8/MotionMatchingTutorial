using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;

public class UnityChanController : MonoBehaviour
{
    public float blendAnimatorRate = 1f;

    private MxMAnimator m_MxMAnimator;

    private bool m_IsMotionMatching = true;

    private LocomotionSpeedRamp m_LocomotionSpeedRamp;
    private MxMTrajectoryGenerator m_MxMTrajectoryGenerator;

    private void Awake()
    {
        m_MxMAnimator = GetComponent<MxMAnimator>();
        m_LocomotionSpeedRamp = GetComponent<LocomotionSpeedRamp>();
        m_MxMTrajectoryGenerator = GetComponent<MxMTrajectoryGenerator>();
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_LocomotionSpeedRamp.BeginSprint();
            m_MxMTrajectoryGenerator.MaxSpeed = 10f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_LocomotionSpeedRamp.ResetFromSprint();
            m_MxMTrajectoryGenerator.MaxSpeed = 4f;
        }
    }
}
