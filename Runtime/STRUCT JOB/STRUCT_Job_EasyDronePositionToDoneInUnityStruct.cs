using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct STRUCT_Job_EasyDronePositionToDoneInUnityStruct : IJobParallelFor
{


    [ReadOnly]
    public NativeArray<STRUCT_DroneGamepad> m_dronesGamepad;
    [ReadOnly]
    public NativeArray<STRUCT_EasyDroneState> m_dronesEasy;

    public NativeArray<STRUCT_ShieldDroneAsUnity> m_droneInUnity;
    public float m_tiltRollAngle;


    public void Execute(int index)
    {
        STRUCT_DroneGamepad gamepad = m_dronesGamepad[index];
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
