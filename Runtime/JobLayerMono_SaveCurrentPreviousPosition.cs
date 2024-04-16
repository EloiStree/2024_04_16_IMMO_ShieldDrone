using DroneIMMO;
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

    public SNAM_ShieldDroneAsUnityFloatValue m_drones;
    public SNAM16K_ObjectVector3 m_previousPosition;
    public SNAM16K_ObjectVector3 m_currentPosition;
    public SNAM16K_BooleanFilter m_indexUsed;



    public void Update()
    {
        Job_CopyPreviousPositionToCurrentPosition job = new Job_CopyPreviousPositionToCurrentPosition
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


[BurstCompile]
public struct Job_CopyPreviousPositionToCurrentPosition: IJobParallelFor
{

    [ReadOnly]
    public NativeArray<bool> m_booleanFilter;

    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_drones;
    [WriteOnly]
    public NativeArray<Vector3> m_previousPosition;
    public NativeArray<Vector3> m_currentPosition;
    public Vector3 m_zero;
    public void Execute(int index)
    {

        m_previousPosition[index] = m_currentPosition[index];
        m_currentPosition[index] = m_drones[index].m_position;
        //if (m_booleanFilter[index])
        //{
        //    m_previousPosition[index] = m_currentPosition[index];
        //    m_currentPosition[index] = m_drones[index].m_position;
        //}
        //else { 
        //    m_previousPosition[index] = m_zero;
        //    m_currentPosition[index] = m_zero;
        //}

    }
}
