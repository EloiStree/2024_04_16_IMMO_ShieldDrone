using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

[BurstCompile]
public struct STRUCT_Job_CopyPreviousPositionToCurrentDronePosition : IJobParallelFor
{

    [ReadOnly]
    public NativeArray<bool> m_booleanFilter;

    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_dronesCurrent;
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_dronesPrevious;
    public bool m_allowSamePosition;
    public void Execute(int index)
    {


        if (m_allowSamePosition) {

            m_dronesPrevious[index] = m_dronesCurrent[index];
        }
        else
        {
             if (m_dronesPrevious[index].m_position != m_dronesCurrent[index].m_position)
            {
                m_dronesPrevious[index] = m_dronesCurrent[index];
            }
        
        }

    }
}
