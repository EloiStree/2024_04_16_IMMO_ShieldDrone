//public class JobLayerMono_SetTransformFromDrones16K : MonoBehaviour
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using System.Collections.Generic;
using System.Linq;

class JobLayerMono_SetTransformFromDrones16K : MonoBehaviour
{
    public SNAM16K_ShieldDroneAsUnityFloatValue  m_droneInUnitySpace;

    public Transform m_transformParent;
    
    [SerializeField] public List<MonoTag_DroneTransformRoot> m_transforms;
    TransformAccessArray m_AccessArray;

    void Awake()
    {

         m_transformParent.GetComponentsInChildren<MonoTag_DroneTransformRoot>(m_transforms);
        m_AccessArray = new TransformAccessArray(m_transforms.Select(k=>k.transform).ToArray());
    }

    void OnDestroy()
    {
    
        m_AccessArray.Dispose();
    }

    public void Update()
    {
     
        var job = new STRUCT_Job_SetTransformPositionWithDronesUnityFloat()
        {
            m_drones = m_droneInUnitySpace.GetNativeArray(),
        };

        JobHandle jobHandle = job.Schedule(m_AccessArray);

        jobHandle.Complete();

    }
}
