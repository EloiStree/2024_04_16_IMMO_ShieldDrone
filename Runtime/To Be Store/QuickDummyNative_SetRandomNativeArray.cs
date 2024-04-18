using System.Collections;
using Unity.Mathematics;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using System;

public class QuickDummyNative_SetRandomNativeArray : MonoBehaviour
{
    public SNAM16K_ShieldDroneAsUnityFloatValue  m_shieldDrones;
    public BigFloatNativeArrayDrone16KMono m_bigNativeArray;
    public BigFloatArrayDrone16KMono m_bigFloatArray;


    public WatchAndDateTimeResult m_jobCopy;
    public WatchAndDateTimeResult m_nativeToByte;

    private void Update()
    {

        m_jobCopy.WatchTheAction(() => {

            ParseStructToBigFloatArray randomizeJob = new ParseStructToBigFloatArray
            {
                m_allDronesUnity = m_shieldDrones.GetNativeArray(),
                m_allDrones8 = m_bigNativeArray.GetNativeArray()
            };
            JobHandle jobHandle = randomizeJob.Schedule(128*128, 1);

            jobHandle.Complete();

        });
        m_nativeToByte.WatchTheAction(()=> {

              float[] array =  m_bigFloatArray.GetFloatArray();
              m_bigNativeArray.GetNativeArray().CopyTo(array);
                // IS IT BETTER THAT 
        });

    }

}



    [BurstCompile]
    public struct ParseStructToBigFloatArray : IJobParallelFor
    {
   

    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_allDronesUnity;


    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<float> m_allDrones8;


    public void Execute(int index)
        {

        STRUCT_ShieldDroneAsUnity drone = m_allDronesUnity[index];

            int offset = index * 8;
            
            for (int i = 0; i < 8; i++)
            {
                m_allDrones8[offset + 0] = drone.m_position.x;
                m_allDrones8[offset + 1] = drone.m_position.y;
                m_allDrones8[offset + 2] = drone.m_position.z;
                m_allDrones8[offset + 3] = drone.m_rotation.x;
                m_allDrones8[offset + 4] = drone.m_rotation.y;
                m_allDrones8[offset + 5] = drone.m_rotation.z;
                m_allDrones8[offset + 6] = drone.m_rotation.w;
                m_allDrones8[offset + 7] = drone.m_shield;
            }
        }

        
    }



[BurstCompile]
public struct RandomizeArrayJob : IJobParallelFor
{

     public uint seed;

    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<float> m_allDrones9;
    
    
    public void Execute(int index)
    {

        Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed + (uint)seed);    

        int offset = index * 9;
        
        for (int i = 0; i < 9; i++)
        {
            m_allDrones9[offset] =    random.NextFloat();
            m_allDrones9[offset + 1] =random.NextFloat();
            m_allDrones9[offset + 2] =random.NextFloat();
            m_allDrones9[offset + 3] =random.NextFloat();
            m_allDrones9[offset + 4] =random.NextFloat();
            m_allDrones9[offset + 5] =random.NextFloat();
            m_allDrones9[offset + 6] =random.NextFloat();
            m_allDrones9[offset + 7] =random.NextFloat();
            m_allDrones9[offset + 8] = random.NextFloat();
        }

    }


}