using Unity.Jobs;
using UnityEngine;

public class JobLayerMono_SaveCurrentPreviousShieldDrone : MonoBehaviour
{

    public SNAM16K_ShieldDroneAsUnityFloatValue m_currentPosition;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_previousPosition;
    public SNAM16K_ObjectBoolean m_indexUsed;
    public bool m_allowSamePosition;

    public bool m_useUpdate;
    public void Update()
    {
        if(m_useUpdate)
            ProcessJob();
    }

    public void ProcessJob()
    {
        STRUCT_Job_CopyPreviousPositionToCurrentDronePosition job = new STRUCT_Job_CopyPreviousPositionToCurrentDronePosition
        {
            m_booleanFilter = m_indexUsed.GetNativeArray(),
            m_dronesCurrent = m_currentPosition.GetNativeArray(),
            m_dronesPrevious = m_previousPosition.GetNativeArray(),
            m_allowSamePosition = m_allowSamePosition
        };
        JobHandle jobHandle = job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64);
        jobHandle.Complete();
    }
}
