using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;

public class UnityChanController : MonoBehaviour
{
    public float blendAnimatorRate = 1f;
    public bool isPistol = false;

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

        // pistol switch
        if (Input.GetKeyDown(KeyCode.F))
        {
            isPistol = !isPistol;
            if (isPistol)
            {
                m_MxMAnimator.AddRequiredTags(ETags.Tag1);
                m_MxMTrajectoryGenerator.TrajectoryMode = ETrajectoryMoveMode.Strafe;
                m_MxMAnimator.AngularErrorWarpMethod = EAngularErrorWarpMethod.TrajectoryFacing;
                m_MxMAnimator.AngularErrorWarpRate = 360f;
                m_MxMAnimator.AngularErrorWarpThreshold = 180f;

            }
            else
            {
                m_MxMAnimator.RemoveRequiredTags(ETags.Tag1);
                m_MxMTrajectoryGenerator.TrajectoryMode = ETrajectoryMoveMode.Normal;
                m_MxMAnimator.AngularErrorWarpMethod = EAngularErrorWarpMethod.CurrentHeading;
                m_MxMAnimator.AngularErrorWarpRate = 45f;
                m_MxMAnimator.AngularErrorWarpThreshold = 60f;

            }
        }
    }
}
