using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct STRUCT_Job_TurnDoneBitsIntoDroneUnity : IJobParallelFor
{
    [NativeDisableParallelForRestriction]
    [ReadOnly]
    public NativeArray<byte> m_bigByteCompressed;


    [WriteOnly]
    public NativeArray<STRUCT_ShieldDroneCompressedBits> m_shieldDroneAsBits;
    [WriteOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_shieldDroneInUnity;

    [WriteOnly]
    public NativeArray<bool> m_isDroneInGameAndConnected;

    [WriteOnly]
    public NativeArray<bool> m_isDroneInCollision;

    public void Execute(int index)
    {
        
        STRUCT_ShieldDroneCompressedBits bits = new STRUCT_ShieldDroneCompressedBits();
          int offsetStart = index*11;
          bits.m_droneState= m_bigByteCompressed[offsetStart + 0] ; 
          bits.m_positionUShortX1= m_bigByteCompressed[offsetStart + 1] ; 
          bits.m_positionUShortX0= m_bigByteCompressed[offsetStart + 2] ; 
          bits.m_positionUShortY1= m_bigByteCompressed[offsetStart + 3] ; 
          bits.m_positionUShortY0= m_bigByteCompressed[offsetStart + 4] ; 
          bits.m_positionUShortZ1= m_bigByteCompressed[offsetStart + 5] ; 
          bits.m_positionUShortZ0= m_bigByteCompressed[offsetStart + 6] ; 
          bits.m_rotationEulerX= m_bigByteCompressed[offsetStart + 7] ; 
          bits.m_rotationEulerY= m_bigByteCompressed[offsetStart + 8] ; 
          bits.m_rotationEulerZ= m_bigByteCompressed[offsetStart + 9] ;
          bits.m_shieldState= m_bigByteCompressed[offsetStart + 10] ;
















        STRUCT_ShieldDroneAsUnity drone = new STRUCT_ShieldDroneAsUnity();
        byte dronestate = bits.m_droneState;



        m_isDroneInGameAndConnected[index] = (dronestate & (0b10000000)) > 0;
        m_isDroneInCollision[index] = (dronestate & (0b01000000)) > 0;
        byte add = (byte)((dronestate & 0b00000011));
        FloatToArray(out drone.m_position.x, in add, in bits.m_positionUShortX0, in bits.m_positionUShortX1);
        add = (byte)((dronestate & 0b00001100) >> 2);
        FloatToArray(out drone.m_position.y, in add, in bits.m_positionUShortY0, in bits.m_positionUShortY1);
        add = (byte)((dronestate & 0b00110000) >> 4);
        FloatToArray(out drone.m_position.z, in add, in bits.m_positionUShortZ0, in bits.m_positionUShortZ1);


        drone.m_rotation = Quaternion.Euler(
            (bits.m_rotationEulerX / 255f) * 360f,
            (bits.m_rotationEulerY / 255f) * 360f,
            (bits.m_rotationEulerZ / 255f) * 360f);
        drone.m_shield = bits.m_shieldState;
        m_shieldDroneInUnity[index] = drone;
    }

    private static void FloatToArray(out float numberFloat, in byte b0, in byte b1, in byte b2)
    {
        int value = (b0 << 16) | (b1 << 8) | (b2);
        numberFloat =Mathf.Clamp( value * 0.001f,0, 262.143f);
    }
}

