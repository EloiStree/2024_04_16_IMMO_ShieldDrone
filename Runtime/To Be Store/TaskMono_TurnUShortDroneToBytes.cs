//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using DroneIMMO;
//using System.Runtime.InteropServices;
//using System;
//using Unity.Collections;
//using Unity.Jobs;
//using Unity.Burst;


//public class TaskMono_TurnUShortDroneToBytes : MonoBehaviour
//{
//    public NativeArrayMono_ShieldDrone16K m_drones;
//    public NativeArrayMono_BigBytesArrayOfDrones m_droneAsBytes;
//    public float m_timeBetweenPush = 0.5f;

//    public long m_timePerUpdateJobTick = 1000;
//    public float m_timePerUpdateJob = 0.0f;
//    public int m_sizeOfElement = 0;
//    public int m_totaleByteToSend = 0;
//    public double m_totaleByteToSendAsKiloByte = 0;
//    public double m_totaleByteToSendAsMegaByte = 0;

//    public JobExe_NativeDroneToByteArray m_jobExecuter;
//    public JobHandle m_jobHandle;




//    void Start()
//    {



//        InvokeRepeating("Push", m_timeBetweenPush, m_timeBetweenPush);
        
//    }

//    public void Push()
//    {

//        m_totaleByteToSend = m_droneAsBytes.m_droneAsNativeBytes.m_dronesAsBytes.Length;
//        m_totaleByteToSendAsKiloByte = m_totaleByteToSend /1024;
//        m_totaleByteToSendAsMegaByte = m_totaleByteToSendAsKiloByte / 1024 /1024;
//        long time_start;
//        time_start = System.DateTime.Now.Ticks;
//        m_sizeOfElement = ShieldDroneAsUShortUtility.SizeInBytes();
//        m_jobExecuter = new JobExe_NativeDroneToByteArray
//        {
//            m_structSize = m_sizeOfElement,
//            m_drones = m_drones.m_nativeArray.m_indexToValue//,
//          //  m_bytes = m_droneAsBytes.m_droneAsNativeBytes.m_dronesAsBytes
//        };


//        m_jobHandle = m_jobExecuter.Schedule(NativeGeneric16KUtility.ARRAY_MAX_SIZE, 64);

//        m_jobHandle.Complete();

//        m_timePerUpdateJobTick = (System.DateTime.Now.Ticks - time_start);
//        m_timePerUpdateJob = m_timePerUpdateJobTick * 0.0001f;

//    }
//}

