using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct STRUCT_Job_DroneGamepadToEasyDronePosition : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<STRUCT_DroneGamepad> m_dronesGamepad;
    public NativeArray<STRUCT_EasyDroneState> m_droneInUnity;

    public float m_delaTime;
    public float m_rotationAngleSpeed;
    public float m_downUpSpeed;
    public float m_frontalSpeed;
    public float m_lateralSpeed;

    public void Execute(int index)
    {
        STRUCT_DroneGamepad drone = m_dronesGamepad[index];
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
