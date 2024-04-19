using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

[BurstCompile]
public struct STRUCT_Job_CopyPreviousPositionToCurrentPosition: IJobParallelFor
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
        if (m_currentPosition[index] != m_drones[index].m_position)
        {
            m_previousPosition[index] = m_currentPosition[index];
            m_currentPosition[index] = m_drones[index].m_position;
        }
        else { 
        
        }
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
