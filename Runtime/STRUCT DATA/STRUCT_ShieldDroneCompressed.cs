using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct STRUCT_ShieldDroneCompressed 
{
    public byte m_droneStateAndZone; //11 State Drone, (11 X 11 Y 11 Z) Extra distance 
    public ushort m_positionX;
    public ushort m_positionY;
    public ushort m_positionZ;
    public byte m_eulerX;
    public byte m_eulerY;
    public byte m_eulerZ;
}

public class ShieldDroneCompressedUtility {

    public bool IsDroneOnlineAndAlive(in STRUCT_ShieldDroneCompressed drone, out bool isOnlineAndAlive)
    =>isOnlineAndAlive= drone.m_droneStateAndZone==1;

    public void GetRotation(in STRUCT_ShieldDroneCompressed drone, out Quaternion rotation) =>
        rotation = Quaternion.Euler(
            (drone.m_eulerX / 255f) * 360f,
            (drone.m_eulerY / 255f) * 360f,
            (drone.m_eulerZ / 255f) * 360f
            );
    public void SetRotation(in Quaternion rotation ,ref STRUCT_ShieldDroneCompressed drone ) {
        Vector3 euler = rotation.eulerAngles;
       drone.m_eulerX = (byte)(((euler.x % 360) / 360f) * 255f);
       drone.m_eulerY = (byte)(((euler.y % 360) / 360f) * 255f);
       drone.m_eulerZ = (byte)(((euler.z % 360) / 360f) * 255f);
    }


}