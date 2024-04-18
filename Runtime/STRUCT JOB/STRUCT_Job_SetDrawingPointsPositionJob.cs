using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct STRUCT_Job_SetDrawingPointsPositionJob : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_drones;

    [WriteOnly]
    public NativeArray<STRUCT_DrawLineDuoPoint> m_points;
    
    public float m_lineDistance;
    
    public void Execute(int index)
    {

        m_points[index] = new STRUCT_DrawLineDuoPoint
        {
            m_pointA = m_drones[index].m_position,
            m_pointB = m_drones[index].m_position + (  m_drones[index].m_rotation *( Vector3.forward * m_lineDistance))
        };
    }
}



