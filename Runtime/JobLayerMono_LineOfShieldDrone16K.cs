using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DroneIMMO;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

public class JobLayerMono_LineOfShieldDrone16K : MonoBehaviour
{

    public float m_lineDistance = 1.0f;
    private JobHandle m_jobHandle;
    public long m_timePerUpdateJobTick = 1000;
    public long m_timePerUpdateDrawTick = 1000;
    public float m_timePerUpdateJob = 0.0f;
    public float m_timePerUpdateDraw = 0.0f;
    public SNAM_ShieldDroneAsUnityFloatValue m_droneInUnity;
    public SNAM_IsGameIndexUse m_isGameIndexUse;
    public SNAM_DebugDrawLine m_linesDuoPoints;

    public float m_timedeltaMutiplicator=10;

    private void Update()
    {
        long time_start = 0;

        time_start = System.DateTime.Now.Ticks;

        DrawPoints();
        m_timePerUpdateJobTick = (System.DateTime.Now.Ticks - time_start);

        time_start = System.DateTime.Now.Ticks;

        NativeArray<DrawLineDuoPoint> line = m_linesDuoPoints.GetNativeArray();
        NativeArray<bool> isBool = m_isGameIndexUse.GetNativeArray();
        for (int i=0; i< line.Length; i++)
        {
            //if (isBool[i])
            Debug.DrawLine(line[i].m_pointA, line[i].m_pointB, Color.green, Time.deltaTime * m_timedeltaMutiplicator);
        }
    
      
        m_timePerUpdateDrawTick = (System.DateTime.Now.Ticks - time_start);
        m_timePerUpdateJob = m_timePerUpdateJobTick * 0.0001f;
        m_timePerUpdateDraw = m_timePerUpdateDrawTick * 0.0001f;
    }

    private void DrawPoints()
    {

        var setRandomPointsJob = new Job_SetDrawingPointsPositionJob
        {
            m_points = m_linesDuoPoints.GetNativeArray(),
            m_drones = m_droneInUnity.GetNativeArray(),
            m_lineDistance = m_lineDistance
        };

        m_jobHandle = setRandomPointsJob.Schedule(NativeGeneric16KUtility.ARRAY_MAX_SIZE, 64);

        m_jobHandle.Complete();
    }

}


[System.Serializable]
public struct DrawLineDuoPoint {

    public Vector3 m_pointA;
    public Vector3 m_pointB;

    
}


[BurstCompile]
public struct Job_SetDrawingPointsPositionJob : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_drones;

    [WriteOnly]
    public NativeArray<DrawLineDuoPoint> m_points;
    
    public float m_lineDistance;
    
    public void Execute(int index)
    {

        m_points[index] = new DrawLineDuoPoint
        {
            m_pointA = m_drones[index].m_position,
            m_pointB = m_drones[index].m_position + (  m_drones[index].m_rotation *( Vector3.forward * m_lineDistance))
        };
    }
}



