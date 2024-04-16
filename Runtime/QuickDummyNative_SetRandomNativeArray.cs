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

    public double m_timeUsedInTick;
    public double m_timeUsedInMilliseconds;
    public double m_timeUsedInMillisecondsB;
    private void Update()
    {

        Stopwatch swt = new Stopwatch();
        swt.Reset();
        swt.Start();

        // Create a job and schedule it
        ParseStructToBigFloatArray randomizeJob = new ParseStructToBigFloatArray
        { 
            m_allDronesUnity= ShieldDroneAsUnity16K.GetNative(),
            m_allDrones9=  FloatArrayDrone16K.GetNative()
        };
        JobHandle jobHandle = randomizeJob.Schedule(128*128, 1);

        // Wait for the job to complete
        jobHandle.Complete();
        swt.Stop();
        m_timeUsedInTick = swt.ElapsedTicks;
        m_timeUsedInMilliseconds = swt.ElapsedTicks * 0.00001;
        m_timeUsedInMillisecondsB = swt.ElapsedMilliseconds;
    }

}



    [BurstCompile]
    public struct ParseStructToBigFloatArray : IJobParallelFor
    {
   

    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_allDronesUnity;


    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<float> m_allDrones9;


    public void Execute(int index)
        {

        STRUCT_ShieldDroneAsUnity drone = m_allDronesUnity[index];

            int offset = index * 9;
            
            for (int i = 0; i < 9; i++)
            {
                 m_allDrones9[offset] = drone.m_gameSate;
                m_allDrones9[offset + 1] = drone.m_position.x;
                m_allDrones9[offset + 2] = drone.m_position.y;
                m_allDrones9[offset + 3] = drone.m_position.z;
                m_allDrones9[offset + 4] = drone.m_rotation.x;
                m_allDrones9[offset + 5] = drone.m_rotation.y;
                m_allDrones9[offset + 6] = drone.m_rotation.z;
                m_allDrones9[offset + 7] = drone.m_rotation.w;
                m_allDrones9[offset + 8] = drone.m_shield;
            }
        }

        
    }



[BurstCompile]
public struct RandomizeArrayJob : IJobParallelFor
{
    //[ReadOnly]
    //public NativeArray<Vector3> m_positions;
    //[ReadOnly]
    //public NativeArray<Quaternion> m_rotations;
    //[ReadOnly]
    //public NativeArray<float> m_shield;
    //[ReadOnly]
    //public NativeArray<float> m_state;
     public uint seed;



    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<float> m_allDrones9;
    
    
    public void Execute(int index)
    {

        Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed + (uint)seed);    

        int offset = index * 9;
        //Vector3 r = new Vector3(RDistance(ref random), RDistance(ref random), RDistance(ref random));
        //Quaternion q = new Quaternion(Random.value, Random.value, Random.value, Random.value) ;
        //m_shield[index] = Random.value * 65000f;
        //m_allDrones9[index] = Random.value * 65000f;
        //m_positions[index] = r;
        //m_rotations[index] = q;

        //for (int i = 0; i < 9; i++)
        //{
        //    m_allDrones9[offset] = R1(ref random, 255f);
        //    m_allDrones9[offset + 1] = RDistance(ref random);
        //    m_allDrones9[offset + 2] = RDistance(ref random);
        //    m_allDrones9[offset + 3] = RDistance(ref random);
        //    m_allDrones9[offset + 4] = R1(ref random);
        //    m_allDrones9[offset + 5] = R1(ref random);
        //    m_allDrones9[offset + 6] = R1(ref random);
        //    m_allDrones9[offset + 7] = R1(ref random);
        //    m_allDrones9[offset + 8] = R1(ref random, 65000);
        //}
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

    //private static float RDistance(ref System.Random r)
    //{
    //    return r.Next() * 9999f;
    //}
    //private static float R1(ref System.Random r, in float multiplicator)
    //{
    //    return r.Next() * multiplicator;
    //}
    //private static float R1(ref System.Random r)
    //{
    //    return r.Next() ;
    //}

}