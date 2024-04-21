using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct STRUCT_Job_TurnDroneUnityIntoDroneBits : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<bool> m_isDroneInGameAndConnected;
    [ReadOnly]
    public NativeArray<bool> m_isDroneInCollision;
    [ReadOnly]
    public NativeArray<STRUCT_ShieldDroneAsUnity> m_shieldDroneInUnity;

    [NativeDisableParallelForRestriction]
    [WriteOnly]
    public NativeArray<byte> m_bigByteCompressed;

   
    public NativeArray<STRUCT_ShieldDroneCompressedBits> m_shieldDroneAsBits;
    public float m_shieldSizeInGame;


    public void Execute(int index)
    {

        STRUCT_ShieldDroneCompressedBits bits = new STRUCT_ShieldDroneCompressedBits();
        STRUCT_ShieldDroneAsUnity drone = m_shieldDroneInUnity[index];
        Vector3 euler = drone.m_rotation.eulerAngles;
        byte b0,b1,b2;
        float usignDistance = drone.m_position.x;

        byte m_droneState = 0;
        if (m_isDroneInGameAndConnected[index])
            m_droneState |= 0b10000000;
        if (m_isDroneInCollision[index])
            m_droneState |= 0b01000000;
    

        FloatToArray(usignDistance, out b0, out b1, out b2);
        m_droneState |= (byte)(b0 << 4);
        bits.m_positionUShortX0 = b1;
        bits.m_positionUShortX1 = b2;

        usignDistance = drone.m_position.y;
        FloatToArray(usignDistance, out b0, out b1, out b2);
        m_droneState |= (byte)(b0 << 2);
        bits.m_positionUShortY0 = b1;
        bits.m_positionUShortY1 = b2;

        usignDistance = drone.m_position.z;
        FloatToArray(usignDistance, out b0, out b1, out b2);
        m_droneState |= b0;
        bits.m_positionUShortZ0 = b1;
        bits.m_positionUShortZ1 = b2;

        bits.m_rotationEulerX = (byte)(((euler.x % 360f) / 360f) * 255f);
        bits.m_rotationEulerY = (byte)(((euler.y % 360f) / 360f) * 255f);
        bits.m_rotationEulerZ = (byte)(((euler.z % 360f) / 360f) * 255f);

        bits.m_droneState = m_droneState;
        bits.m_shieldState = (byte)((drone.m_shield / m_shieldSizeInGame) * 255f);
        m_shieldDroneAsBits[index] = bits;

        int offsetStart= index*11;
        m_bigByteCompressed[offsetStart +  0] = bits. m_droneState;
        m_bigByteCompressed[offsetStart +  1] = bits. m_positionUShortX1;
        m_bigByteCompressed[offsetStart +  2] = bits. m_positionUShortX0;
        m_bigByteCompressed[offsetStart +  3] = bits. m_positionUShortY1;
        m_bigByteCompressed[offsetStart +  4] = bits. m_positionUShortY0;
        m_bigByteCompressed[offsetStart +  5] = bits. m_positionUShortZ1;
        m_bigByteCompressed[offsetStart +  6] = bits. m_positionUShortZ0;
        m_bigByteCompressed[offsetStart +  7] = bits. m_rotationEulerX;
        m_bigByteCompressed[offsetStart +  8] = bits. m_rotationEulerY;
        m_bigByteCompressed[offsetStart +  9] = bits. m_rotationEulerZ;
        m_bigByteCompressed[offsetStart + 10] = bits. m_shieldState;

    }
    private static void FloatToArray(float numberFloat, out byte b0, out byte b1, out byte b2)
    {
        int number = (int)(numberFloat * 1000f);
        b0 = (byte)((number >> 16) & 0xFF);
        b1 = (byte)((number >> 8) & 0xFF);
        b2 = (byte)(number & 0xFF);
    }
}

