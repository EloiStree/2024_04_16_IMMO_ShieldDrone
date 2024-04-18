using DroneIMMO;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobLayerMono_IntegerValueToDroneGamepad : MonoBehaviour
{
    public SNAM16K_IntegerUserValue m_integerValeue;
    public SNAM16K_DroneGamepad16K m_droneController;
    public SNAM16K_EasyDronePositionState m_droneInEasyDroneState;
    public SNAM16K_ShieldDroneAsUnityFloatValue  m_droneInUnitySpace;

    public float m_rotationAngleSpeed = 10;
    public float m_downUpSpeed = 10;
    public float m_frontalSpeed = 10;
    public float m_lateralSpeed = 10;

    public float m_rotationTiltAngle = 65;

    public STRUCT_DroneGamepad[]     m_droneGamepad10= new STRUCT_DroneGamepad[10];
    public STRUCT_EasyDroneState[]      m_easyDrone10 = new STRUCT_EasyDroneState[10];
    public STRUCT_ShieldDroneAsUnity[]  m_droneInUnity10 = new STRUCT_ShieldDroneAsUnity[10];

    public void Update()
    {
        
        STRUCT_Job_IntegerValueToDroneGamepad job = new STRUCT_Job_IntegerValueToDroneGamepad
        {
            m_dronesValue = m_integerValeue.GetNativeArray(),
            m_gamepads = m_droneController.GetNativeArray()
        };
        job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64).Complete();


        STRUCT_Job_DroneGamepadToEasyDronePosition job2 = new STRUCT_Job_DroneGamepadToEasyDronePosition
        {
            m_dronesGamepad = m_droneController.GetNativeArray(),
            m_droneInUnity = m_droneInEasyDroneState.GetNativeArray(),
            m_delaTime = Time.deltaTime,
            m_rotationAngleSpeed = m_rotationAngleSpeed,
            m_downUpSpeed = m_downUpSpeed,
            m_frontalSpeed = m_frontalSpeed,
            m_lateralSpeed = m_lateralSpeed
        };
        job2.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64).Complete();

        STRUCT_Job_EasyDronePositionToDoneInUnityStruct job3 = new STRUCT_Job_EasyDronePositionToDoneInUnityStruct
        {
            m_dronesGamepad = m_droneController.GetNativeArray(),
            m_dronesEasy = m_droneInEasyDroneState.GetNativeArray(),
            m_droneInUnity = m_droneInUnitySpace.GetNativeArray(),
            m_tiltRollAngle = m_rotationTiltAngle
        };
        job3.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64).Complete();

        for (int i = 0; i < 10; i++)
        {
            m_droneGamepad10[i] = m_droneController.GetNativeArray()[i];
            m_easyDrone10[i] = m_droneInEasyDroneState.GetNativeArray()[i];
            m_droneInUnity10[i] = m_droneInUnitySpace.GetNativeArray()[i];
        }
    }

}
