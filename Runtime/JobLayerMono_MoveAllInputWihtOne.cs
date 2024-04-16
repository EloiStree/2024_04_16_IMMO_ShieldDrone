using DroneIMMO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobLayerMono_MoveAllInputWihtOne : MonoBehaviour
{
    public WatchAndDateTimeResult m_takenTime;
    public SNAM_IntegerUserValue m_userValue;

    [Range(-1f, 1f)] public float m_leftHorizontalAxis;
    [Range(-1f, 1f)] public float m_leftVerticalAxis;
    [Range(-1f, 1f)] public float m_rightHorizontalAxis;
    [Range(-1f, 1f)] public float m_rightVerticalAxis;

    public int m_startId = 1600000000;
    public int m_value = 0;

    public bool m_useJoystick;
    public float m_deathZone = 0.1f;

    void Update()
    {
        if (m_useJoystick) {


            // Read joystick input values
            m_leftHorizontalAxis = Input.GetAxis("Horizontal");
            m_leftVerticalAxis = Input.GetAxis("Vertical");
            m_rightHorizontalAxis = Input.GetAxis("HorizontalRotation");
            m_rightVerticalAxis = Input.GetAxis("VerticalRotation");

        }

        int value = m_startId;
        if (Mathf.Abs(m_leftHorizontalAxis )>= m_deathZone) value += Get99FromPercent(m_leftHorizontalAxis) * 1000000;
        if (Mathf.Abs(m_leftVerticalAxis)>= m_deathZone) value += Get99FromPercent(m_leftVerticalAxis) * 10000;
        if (Mathf.Abs(m_rightHorizontalAxis)>= m_deathZone) value += Get99FromPercent(m_rightHorizontalAxis) * 100;
        if (Mathf.Abs(m_rightVerticalAxis)>= m_deathZone) value += Get99FromPercent(m_rightVerticalAxis) ;


        m_takenTime.WatchTheAction(() => {
            JobStruct_SetAllInputToGamepad job = new JobStruct_SetAllInputToGamepad()
            {
                m_input = m_userValue.GetNativeArray(),
                m_value = value 
            };
            job.Schedule(128 * 128, 64).Complete();
        });
        m_value = value;
    }

    private int Get99FromPercent(float percent)
    {
        return (int) ((((percent + 1f) * 0.5f) * 98f) + 1f);
    }
}



[BurstCompile]
public struct JobStruct_SetAllInputToGamepad: IJobParallelFor
{
    [WriteOnly]
    public NativeArray<int> m_input;
    public int m_value;


    public void Execute(int index)
    {
        m_input[index] = m_value;
    }
}
