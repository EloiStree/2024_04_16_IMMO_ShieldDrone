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
    public SNAM16K_ShieldDroneAsUnityFloatValue  m_droneInUnity;
    public SNAM16K_ObjectBoolean m_isGameIndexUse;
    public SNAM16K_DebugDrawLine m_linesDuoPoints;

    public float m_timedeltaMutiplicator=10;

    private void Update()
    {
        long time_start = 0;

        time_start = System.DateTime.Now.Ticks;

        DrawPoints();
        m_timePerUpdateJobTick = (System.DateTime.Now.Ticks - time_start);

        time_start = System.DateTime.Now.Ticks;

        NativeArray<STRUCT_DrawLineDuoPoint> line = m_linesDuoPoints.GetNativeArray();
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

        var setRandomPointsJob = new STRUCT_Job_SetDrawingPointsPositionJob
        {
            m_points = m_linesDuoPoints.GetNativeArray(),
            m_drones = m_droneInUnity.GetNativeArray(),
            m_lineDistance = m_lineDistance
        };

        m_jobHandle = setRandomPointsJob.Schedule(NativeGeneric16KUtility.ARRAY_MAX_SIZE, 64);

        m_jobHandle.Complete();
    }

}



