using DroneIMMO;
using NUnit.Compatibility;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobLayerMono_IntegerValueToDroneGamepad : MonoBehaviour
{
    public SNAM_IntegerUserValue m_integerValeue;
    public SNAM_DroneController16K m_droneController;
    public SNAM_EasyDronePositionState m_droneInEasyDroneState;
    public SNAM_ShieldDroneAsUnityFloatValue m_droneInUnitySpace;

    public float m_rotationAngleSpeed = 10;
    public float m_downUpSpeed = 10;
    public float m_frontalSpeed = 10;
    public float m_lateralSpeed = 10;

    public float m_rotationTiltAngle = 65;

    public STRUCT_DroneController[]     m_droneGamepad10= new STRUCT_DroneController[10];
    public STRUCT_EasyDroneState[]      m_easyDrone10 = new STRUCT_EasyDroneState[10];
    public STRUCT_ShieldDroneAsUnity[]  m_droneInUnity10 = new STRUCT_ShieldDroneAsUnity[10];

    public void Update()
    {
        
        Job_IntegerValueToDroneGamepad job = new Job_IntegerValueToDroneGamepad
        {
            m_dronesValue = m_integerValeue.GetNativeArray(),
            m_gamepads = m_droneController.GetNativeArray()
        };
        job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64).Complete();


        Job_DroneGamepadToEasyDronePosition job2 = new Job_DroneGamepadToEasyDronePosition
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

        Job_EasyDronePositionToDoneInUnityStruct job3 = new Job_EasyDronePositionToDoneInUnityStruct
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

public class IMMO16K { 

    public static int ARRAY_MAX_SIZE = 128 * 128;
}


public struct Job_IntegerValueToDroneGamepad : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<int> m_dronesValue;

    [WriteOnly]
    public NativeArray<STRUCT_DroneController> m_gamepads;


    public void Execute(int index)
    {

        STRUCT_DroneController drone = new STRUCT_DroneController();

        int value = m_dronesValue[index];
        float value99 = 0;
        
        value99 = (value / 1000000) % 100;
        if (value99 == 0) drone.m_leftRightRotationPercent11 = 0;
        else Convert99ToPercent11(value99, out drone.m_leftRightRotationPercent11);

        value99 = (value / 10000) % 100;
        if (value99 == 0) drone.m_downUpMovementPercent11 = 0;
        else Convert99ToPercent11(value99, out drone.m_downUpMovementPercent11);

        value99 = (value / 100) % 100;
        if (value99 == 0) drone.m_leftRightMovementPercent11 = 0;
        else Convert99ToPercent11(value99, out drone.m_leftRightMovementPercent11);

        value99 = (value) % 100;
        if (value99 == 0) drone.m_backForwardMovementPercent11 = 0;
        else Convert99ToPercent11(value99, out drone.m_backForwardMovementPercent11);

        m_gamepads[index] = drone;
    }

    private static float Convert99ToPercent11(float value99, out float result)
        =>result = (((value99 - 1f) / 98f) - 0.5f) * 2f;
}

public struct Job_DroneGamepadToEasyDronePosition : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<STRUCT_DroneController> m_dronesGamepad;
    public NativeArray<STRUCT_EasyDroneState> m_droneInUnity;

    public float m_delaTime;
    public float m_rotationAngleSpeed;
    public float m_downUpSpeed;
    public float m_frontalSpeed;
    public float m_lateralSpeed;

    public void Execute(int index)
    {
        STRUCT_DroneController drone = m_dronesGamepad[index];
        STRUCT_EasyDroneState unityDrone = m_droneInUnity[index];


        unityDrone.m_panHorizontal += -drone.m_leftRightRotationPercent11
            * m_rotationAngleSpeed * m_delaTime;

        if (unityDrone.m_panHorizontal > 360f)
            unityDrone.m_panHorizontal -= 360f;
        if (unityDrone.m_panHorizontal < 0f)
            unityDrone.m_panHorizontal += 360f;

        Quaternion q = Quaternion.Euler(0, -unityDrone.m_panHorizontal, 0);

        Vector3 forward = q * Vector3.forward;
        Vector3 right = (Quaternion.Euler(0, 90, 0) * q) * Vector3.forward;
        Vector3 up = Vector3.up;

        unityDrone.m_position += forward * (drone.m_backForwardMovementPercent11 * m_delaTime * m_frontalSpeed);
        unityDrone.m_position += right * (drone.m_leftRightMovementPercent11 * m_delaTime * m_lateralSpeed);
        unityDrone.m_position += up * (drone.m_downUpMovementPercent11 * m_delaTime * m_downUpSpeed);

        m_droneInUnity[index] = unityDrone;
    }
}

public struct Job_EasyDronePositionToDoneInUnityStruct : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<STRUCT_DroneController> m_dronesGamepad;
    [ReadOnly]
    public NativeArray<STRUCT_EasyDroneState> m_dronesEasy;

    public NativeArray<STRUCT_ShieldDroneAsUnity> m_droneInUnity;
    public float m_tiltRollAngle;


    public void Execute(int index)
    {
        STRUCT_DroneController gamepad = m_dronesGamepad[index];
        STRUCT_EasyDroneState drone = m_dronesEasy[index];
        STRUCT_ShieldDroneAsUnity unityDrone = m_droneInUnity[index];

        unityDrone.m_position = drone.m_position; 
        unityDrone.m_rotation = Quaternion.Euler(0,-drone.m_panHorizontal,0);

        //unityDrone.m_rotation = Quaternion.Euler(
        //    gamepad.m_leftRightMovementPercent11*m_tiltRollAngle,
        //    0,
        //    gamepad.m_backForwardMovementPercent11 * m_tiltRollAngle)* unityDrone.m_rotation;

        m_droneInUnity[index] = unityDrone;
    }
}
