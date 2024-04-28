//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using Unity.Burst;
//using Unity.Collections;
//using Unity.Jobs;
//using Unity.Mathematics;
//using UnityEngine;

//public static class Utility16K
//{
//    public const int ARRAY_MAX_SIZE = 16384;
//}

using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobLayerMono_SetRandomPositionToAll : MonoBehaviour
{
    public SNAM_Generic16K<uint> m_randomSeed;
    public SNAM16K_EasyDronePositionState m_easyDrone;
    public float m_cubeRange = 10;
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        Job_SetRandomPositionToAll job = new Job_SetRandomPositionToAll
        {
            m_seeds = m_randomSeed.GetNativeArray(),
            m_easyDrone = m_easyDrone.GetNativeArray(),
            m_cubeRange = m_cubeRange
        };
        job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64).Complete();
    }

}



[BurstCompile]
public struct Job_SetRandomPositionToAll : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<uint> m_seeds;
    [WriteOnly]
    public NativeArray<STRUCT_EasyDroneState> m_easyDrone;
    public float m_cubeRange;

    public void Execute(int index)
    {
        uint seed = m_seeds[index];
        STRUCT_EasyDroneState drone = new STRUCT_EasyDroneState();
        Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed);

        drone.m_position.x = random.NextFloat(-0, m_cubeRange);
        drone.m_position.y = random.NextFloat(-0, m_cubeRange);
        drone.m_position.z = random.NextFloat(-0, m_cubeRange);

        m_easyDrone[index] = drone;
    }
}



