using DroneIMMO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

public class JobLayerMono_SaveCurrentPreviousPosition : MonoBehaviour
{

    public SNAM16K_ShieldDroneAsUnityFloatValue  m_drones;
    public SNAM16K_ObjectVector3 m_previousPosition;
    public SNAM16K_ObjectVector3 m_currentPosition;
    public SNAM16K_ObjectBoolean m_indexUsed;



    public void Update()
    {

        ProcessJob();
    }

    private void ProcessJob()
    {
        STRUCT_Job_CopyPreviousPositionToCurrentPosition job = new STRUCT_Job_CopyPreviousPositionToCurrentPosition
        {
            m_booleanFilter = m_indexUsed.GetNativeArray(),
            m_drones = m_drones.GetNativeArray(),
            m_previousPosition = m_previousPosition.GetNativeArray(),
            m_currentPosition = m_currentPosition.GetNativeArray(),
            m_zero = Vector3.zero
        };
        JobHandle jobHandle = job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64);
        jobHandle.Complete();
    }
}
