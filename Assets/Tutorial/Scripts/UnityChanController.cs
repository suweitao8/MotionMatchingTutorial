using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MxM;
using MxMGameplay;

public class UnityChanController : MonoBehaviour
{
    public enum UnityChanState
    {
        Normal,
        Slide,
    }

    public float blendAnimatorRate = 1f;
    public bool isPistol = false;
    public MxMEventDefinition eventDefSlide;
    public UnityChanState state = UnityChanState.Normal;
    public MxMEventDefinition eventDefEquip;
    public MxMEventDefinition eventDefUnEquip;
    public MxMEventDefinition eventDefVault;
    public Transform contact1;
    public Transform contact2;

    private MxMAnimator m_MxMAnimator;

    private bool m_IsMotionMatching = true;
    private float height;
    private float centerY;

    private LocomotionSpeedRamp m_LocomotionSpeedRamp;
    private MxMTrajectoryGenerator m_MxMTrajectoryGenerator;
    private GenericControllerWrapper m_GenericControllerWrapper;

    private void Awake()
    {
        m_MxMAnimator = GetComponent<MxMAnimator>();
        m_LocomotionSpeedRamp = GetComponent<LocomotionSpeedRamp>();
        m_MxMTrajectoryGenerator = GetComponent<MxMTrajectoryGenerator>();
        m_GenericControllerWrapper = GetComponent<GenericControllerWrapper>();

        height = m_GenericControllerWrapper.Height;
        centerY = m_GenericControllerWrapper.Center.y;
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
                // play equip animation
                m_MxMAnimator.BeginEvent(eventDefEquip);
                m_MxMAnimator.AddRequiredTags(ETags.Tag1);
                m_MxMTrajectoryGenerator.TrajectoryMode = ETrajectoryMoveMode.Strafe;
                m_MxMAnimator.AngularErrorWarpMethod = EAngularErrorWarpMethod.TrajectoryFacing;
                m_MxMAnimator.AngularErrorWarpRate = 360f;
                m_MxMAnimator.AngularErrorWarpThreshold = 180f;

            }
            else
            {
                // play unequip animation
                m_MxMAnimator.BeginEvent(eventDefUnEquip);
                m_MxMAnimator.RemoveRequiredTags(ETags.Tag1);
                m_MxMTrajectoryGenerator.TrajectoryMode = ETrajectoryMoveMode.Normal;
                m_MxMAnimator.AngularErrorWarpMethod = EAngularErrorWarpMethod.CurrentHeading;
                m_MxMAnimator.AngularErrorWarpRate = 45f;
                m_MxMAnimator.AngularErrorWarpThreshold = 60f;

            }
        }

        switch (state)
        {
            case UnityChanState.Normal:
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    state = UnityChanState.Slide;
                    m_MxMAnimator.BeginEvent(eventDefSlide);
                    m_GenericControllerWrapper.Height = height / 3f;
                    m_GenericControllerWrapper.Center = Vector3.up * centerY / 3f;
                }
                break;
            case UnityChanState.Slide:
                if (m_MxMAnimator.IsEventComplete)
                {
                    state = UnityChanState.Normal;
                    m_GenericControllerWrapper.Height = height;
                    m_GenericControllerWrapper.Center = Vector3.up * centerY;
                }
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventDefVault.ClearContacts();
            eventDefVault.AddEventContact(contact1.position, transform.rotation.eulerAngles.y);
            eventDefVault.AddEventContact(contact2.position, transform.rotation.eulerAngles.y);
            m_MxMAnimator.BeginEvent(eventDefVault);
        }

        if (m_MxMAnimator.QueryUserTags(EUserTags.UserTag1))
        {
            m_MxMAnimator.RootMotion = EMxMRootMotion.On;
        }
        else
        {
            m_MxMAnimator.RootMotion = EMxMRootMotion.RootMotionApplicator;
        }
    }
}
