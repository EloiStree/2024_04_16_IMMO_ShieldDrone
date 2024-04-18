//public class JobLayerMono_SetTransformFromDrones16K : MonoBehaviour
using Unity.Collections;
using UnityEngine.Jobs;

public struct STRUCT_Job_SetTransformPositionWithDronesUnityFloat : IJobParallelForTransform
{

    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_drones;


    // The code actually running on the job
    public void Execute(int index, TransformAccess transform)
    {
        transform.position = m_drones[index].m_position;
        transform.rotation = m_drones[index].m_rotation;
    }
}
