using DroneIMMO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobLayerMono_SetRandomIntegerInput : MonoBehaviour
{

    public WatchAndDateTimeResult m_takenTime;
    public SNAM16K_IntegerPlayerIndexClaim m_indexClaim;
    public SNAM16K_IntegerUserValue m_userValue;
    public SNAM16K_UpdateRandomSeed m_seeds;

    [Header("Debug dirty")]
    public bool m_useAwake;
    public bool m_useUpdate;
    private void Start()
    {
        if(m_useAwake)
         Invoke("RandomizeClaimIndex", 0.1f);
    }

    private void RandomizeClaimIndex()
    {
        Randomize(m_indexClaim.GetNativeArray());
    }

    void Update()
    {
        if(m_useUpdate)
            Randomize(m_userValue.GetNativeArray());
    }
    private  void Randomize( NativeArray<int>  target)
    {
       
        int offset= UnityEngine.Random.Range(0, 1000000);
        m_takenTime.WatchTheAction(() => { 
            JobStruct_SetRandomIntegerInput job = new JobStruct_SetRandomIntegerInput()
            {
                m_input = target,
                m_randomSeed = m_seeds.GetNativeArray(),
                m_offset = offset
            };
            job.Schedule(128*128, 64).Complete();
        });
    }
}
public class JobHandlerWatchStop16K {

    public JobHandle m_handle;


    public void Launh(JobHandle handle, out double WatchTimeTick, out double watchTimeMilliseconds ,out double dateTimeInMilliseconds)

    {
        DateTime start = DateTime.Now; 
        Stopwatch sw = new Stopwatch();
        sw.Start();
        m_handle = handle;
        sw.Stop();
        dateTimeInMilliseconds = (DateTime.Now - start).TotalMilliseconds;
        WatchTimeTick = sw.ElapsedTicks;
        watchTimeMilliseconds = sw.ElapsedMilliseconds;
    }
}
