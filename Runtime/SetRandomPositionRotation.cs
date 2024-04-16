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

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

//    private JobHandle m_jobHandle;
//    public long m_timePerUpdateJobTick = 1000;

//    private void Start()
//    {
//        Stopwatch sw = new Stopwatch();
//        sw.Start();
//        DrawPoints();
//        sw.Stop(); 
//        m_timePerUpdateJobTick = sw.ElapsedTicks;
//    }

//    private void DrawPoints()
//    {

//        var setRandomPointsJob = new SetRandomPositionRotation
//        {
//            m_drones = ShieldDroneAsUnity16K.GetNative()
//        };

//        m_jobHandle = setRandomPointsJob.Schedule(Utility16K.ARRAY_MAX_SIZE, 64);

//        m_jobHandle.Complete();
//    }

//}



public class SetRandomPositionRotation { }

[BurstCompile]
public struct 
    Job_SetRandomPositionRotation : IJobParallelFor
{
    [WriteOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_drones;
    public void Execute(int index)
    {
        Unity.Mathematics.Random r = Unity.Mathematics.Random.CreateFromIndex((uint)index);
        STRUCT_ShieldDroneAsUnity drone = new STRUCT_ShieldDroneAsUnity();
        drone.m_position = new Vector3(
            r.NextFloat(-65f, 65f),
            r.NextFloat(-65f, 65f),
            r.NextFloat(-65f, 65f));
        drone.m_rotation = new Quaternion(
            r.NextFloat(-1, 1),
            r.NextFloat(-1, 1),
            r.NextFloat(-1, 1),
            r.NextFloat(-1, 1));
        m_drones[index] = drone;
    }

}



