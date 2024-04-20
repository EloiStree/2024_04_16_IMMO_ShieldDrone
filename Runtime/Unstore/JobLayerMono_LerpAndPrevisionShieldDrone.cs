using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobLayerMono_LerpAndPrevisionShieldDrone : MonoBehaviour
{

    public SNAM16K_ShieldDroneAsUnityFloatValue m_droneInUnity;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_droneInUnityLerped;


    public long m_lastUpdateDate;
    public long m_lastUpdateDelayTick;
    public float m_percentFromLastUpdate;

    public void NotifyAsPackageReceived()
    {
        long now = DateTime.UtcNow.Ticks;
        m_lastUpdateDelayTick = now - m_lastUpdateDate;
        m_lastUpdateDate = now;
    }

    private JobHandle m_jobHandle;
    public void Update()
    {
        if (Time.time < 3)
            return;
        long now = DateTime.UtcNow.Ticks;
        long delay = now - m_lastUpdateDate;
        if (m_lastUpdateDelayTick > 0)
        {
            m_percentFromLastUpdate = delay / (float)m_lastUpdateDelayTick;
        }
        else { m_percentFromLastUpdate = 1f; }


        STRUCT_Job_ShieldDroneUnityLerpPrevision job = new STRUCT_Job_ShieldDroneUnityLerpPrevision()
        {
            m_dronesCurrent = m_droneInUnity.GetNativeArray(),
            m_dronesLerped = m_droneInUnityLerped.GetNativeArray(),
            m_percentBetweenUpdate = m_percentFromLastUpdate
        };
        m_jobHandle = job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64);

        m_jobHandle.Complete();
    }
}

[BurstCompile]
public struct STRUCT_Job_ShieldDroneUnityLerpPrevision : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_dronesCurrent;

    public NativeArray<STRUCT_ShieldDroneAsUnity> m_dronesLerped;

    public float m_percentBetweenUpdate;
    public Quaternion m_zeroQuaternion;
    public void Execute(int index)
    {
        STRUCT_ShieldDroneAsUnity current = m_dronesCurrent[index];
        STRUCT_ShieldDroneAsUnity display = m_dronesLerped[index];


        if (IsNaN(display.m_rotation))
            display.m_rotation = Quaternion.identity;
        if (IsNaN(current.m_rotation))
            current.m_rotation = Quaternion.identity;

        if (m_percentBetweenUpdate < 0.05f)
        {
            display.m_position = current.m_position;
            display.m_rotation = current.m_rotation;
            //display.m_shield = current.m_shield;
        }
        else if ((current.m_position.magnitude < 1000 && current.m_position.magnitude < 1000))
        {
            display.m_position = Vector3.Lerp(display.m_position, current.m_position, m_percentBetweenUpdate);
            display.m_rotation = Quaternion.Lerp(display.m_rotation, current.m_rotation, m_percentBetweenUpdate);

            // display.m_shield =Mathf.Lerp(display.m_shield, current.m_shield, m_percentBetweenUpdate);
        }
        else
        {

            display.m_position = current.m_position;
            display.m_rotation = current.m_rotation;
        }

        m_dronesLerped[index] = display;
    }
    private bool IsNaN(Quaternion q)
    {
        if (q.x == 0f && q.y == 0f && q.z == 0f && q.w == 0f)
            return true;

        return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
    }
}

