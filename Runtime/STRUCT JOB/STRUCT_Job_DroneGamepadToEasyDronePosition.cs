using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct STRUCT_Job_DroneGamepadToEasyDronePosition : IJobParallelFor
{


   // public NativeArray<byte> m_gamepadType;
    [ReadOnly]
    public NativeArray<STRUCT_DroneGamepad> m_dronesGamepad;
    public NativeArray<STRUCT_EasyDroneState> m_droneInUnity;

    public STRUCT_DroneMoveRotateConfigSpeed m_configSpeed;


    public float m_delaTime;

    public void Execute(int index)
    {
        float maxDistance = 0b111111111111111111 *0.001f;
        byte byteTYpe = 0;

        //0 Default easy drone
        //1 Drone classic no gravity
        //2 Drone classic with gravity
        //3 Space ship
        //if (byteTYpe == 0)
        {
            STRUCT_DroneGamepad drone = m_dronesGamepad[index];
            STRUCT_EasyDroneState unityDrone = m_droneInUnity[index];


            unityDrone.m_panHorizontal += -drone.m_leftRightRotationPercent11
                * m_configSpeed.m_rotationAngleSpeed * m_delaTime;

            if (unityDrone.m_panHorizontal > 360f)
                unityDrone.m_panHorizontal -= 360f;
            if (unityDrone.m_panHorizontal < 0f)
                unityDrone.m_panHorizontal += 360f;

            Quaternion q = Quaternion.Euler(0, -unityDrone.m_panHorizontal, 0);

            Vector3 forward = q * Vector3.forward;
            Vector3 right = (Quaternion.Euler(0, 90, 0) * q) * Vector3.forward;
            Vector3 up = Vector3.up;

            unityDrone.m_position += forward * (drone.m_backForwardMovementPercent11 * m_delaTime * m_configSpeed.m_frontalSpeed);
            unityDrone.m_position += right * (drone.m_leftRightMovementPercent11 * m_delaTime * m_configSpeed.m_lateralSpeed);
            unityDrone.m_position += up * (drone.m_downUpMovementPercent11 * m_delaTime * m_configSpeed.m_downUpSpeed);

            if (unityDrone.m_position.x < 0f) unityDrone.m_position.x = 0;
            if (unityDrone.m_position.y < 0f) unityDrone.m_position.y = 0;
            if (unityDrone.m_position.z < 0f) unityDrone.m_position.z = 0;
            if (unityDrone.m_position.x > maxDistance ) unityDrone.m_position.x = maxDistance;
            if (unityDrone.m_position.y > maxDistance ) unityDrone.m_position.y = maxDistance;
            if (unityDrone.m_position.z > maxDistance) unityDrone.m_position.z = maxDistance;

            m_droneInUnity[index] = unityDrone;
        }


        if (byteTYpe == 1 || byteTYpe == 2)
        {
           
        }


        if (byteTYpe == 2)
        {
         
        }



       

    }
}


[System.Serializable]
public struct STRUCT_DroneMoveRotateConfigSpeed {
    // Easy Drone
    public float m_rotationAngleSpeed;
    public float m_downUpSpeed;
    public float m_frontalSpeed;
    public float m_lateralSpeed;



    // HardDrone
    public float m_upSpeed;
    public float m_leftRightRotation;
    public float m_leftRightRoll;
    public float m_backforwardTilt;


    //Space Ship move
    public float m_droneUpSpeed;
    public float m_spaceShipMaxSpeed;
    public float m_leftRightFPVRotate;
    public float m_leftRightFPVRoll;
    public float m_backforwardFPVTilt;

}
