using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

using Eloi.WatchAndDate;

public partial class Experiment_CompressUncompressDroneinbits : MonoBehaviour
{


    [Header("Server Side")]
    public SNAM16K_ObjectBoolean                m_inputIsInGame;
    public SNAM16K_ObjectBoolean                m_inputIsInCollision;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_inputDroneUnity;
    public SNAM16K_ShieldDroneCompressedAsBits  m_inputDroneBits;


    public BigByteNativeArrayCompressedDrone16KMono m_inputBigByteArray;
    public BigByteArrayCompressedDrone16KMono m_bigByteArray;
    public BigByteNativeArrayCompressedDrone16KMono m_outputBigByteArray;

    [Header("Client Side")]
    public SNAM16K_ShieldDroneCompressedAsBits m_outputDroneBits;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_outputDroneUnity;
    public SNAM16K_ObjectBoolean m_outputIsInGame;
    public SNAM16K_ObjectBoolean m_outputIsInCollision;

    [Header("Debug Compare")]
    public bool m_isInputInGame;
    public bool m_isInputInCollision;
    public STRUCT_ShieldDroneAsUnity        m_inputDebugUnityOne;
    public STRUCT_ShieldDroneCompressedBits m_inputDebugBitsOne;
    public STRUCT_ShieldDroneCompressedBits m_outputDebugBitsOne;
    public STRUCT_ShieldDroneAsUnity        m_outputDebugUnityOne;
    public bool m_isOutputInGameDebug;
    public bool m_isOutputInCollisionDebug;

    public int m_test;
    void Start()
    {

    }

    void Update()
    {
        m_fullJob.WatchTheAction(() => {
            m_inputIsInGame.Set(0,m_isInputInGame);
            m_inputIsInCollision.Set(0, m_isInputInCollision) ;
            m_inputDroneUnity.Set(0, m_inputDebugUnityOne) ;
            m_test = m_inputDroneUnity.GetNativeArray().Length;
       
            TurnShieldDroneInUnityToBitsStruct();
            TurnBitsStructToBytesBigArray();

            TurnBigBytesArrayToBitsDroneStruct();
            TurnBitsDroneStructToUnityDrone();
    

            m_isOutputInCollisionDebug = m_outputIsInCollision.Get(0);
            m_isOutputInGameDebug = m_outputIsInGame.Get(0); 
            m_outputDebugBitsOne=(m_outputDroneBits.Get(0)); 
            m_outputDebugUnityOne = m_outputDroneUnity.Get(0);
        });
    }
    public float m_shieldMaxSize = ushort.MaxValue;
    public WatchAndDateTimeActionResult m_fullJob;
    public WatchAndDateTimeActionResult m_unityToBitsJob;
    public WatchAndDateTimeActionResult m_structToByteJob;
    public WatchAndDateTimeActionResult m_bytesToStructJob;
    public WatchAndDateTimeActionResult m_bitsToUnityJob;
    private void TurnShieldDroneInUnityToBitsStruct()
    {
        m_unityToBitsJob.WatchTheAction(() =>
        {
            STRUCT_Job_TurnDroneUnityIntoDroneBits job = new STRUCT_Job_TurnDroneUnityIntoDroneBits()
            {
                m_isDroneInCollision = m_inputIsInCollision.GetNativeArray(),
                m_isDroneInGameAndConnected = m_inputIsInGame.GetNativeArray(),
                m_shieldDroneInUnity = m_inputDroneUnity.GetNativeArray(),
                m_shieldDroneAsBits = m_inputDroneBits.GetNativeArray(),
                m_bigByteCompressed = m_inputBigByteArray.GetBytesNativeArray(),
                m_shieldSizeInGame = m_shieldMaxSize
            };
            JobHandle jobHandle = job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64);
            jobHandle.Complete();
        });
    }
    private void TurnBitsStructToBytesBigArray()
    {
        m_structToByteJob.WatchTheAction(() => {

            byte[] b = m_bigByteArray.GetBytesArray();
            m_inputBigByteArray.GetBytesNativeArray().CopyTo(b);
        });
    }

    private void TurnBigBytesArrayToBitsDroneStruct()
    {
        
        m_bytesToStructJob.WatchTheAction(() => {

        byte[] b = m_bigByteArray.GetBytesArray();
        m_outputBigByteArray.GetBytesNativeArray().CopyFrom(b);
        
  
        });
    }

    private void TurnBitsDroneStructToUnityDrone()
    {
        m_bitsToUnityJob.WatchTheAction(() =>
        {
            STRUCT_Job_TurnDoneBitsIntoDroneUnity job = new STRUCT_Job_TurnDoneBitsIntoDroneUnity()
            {
                m_bigByteCompressed = m_outputBigByteArray.GetBytesNativeArray(),
                m_shieldDroneAsBits = m_outputDroneBits.GetNativeArray(),
                m_isDroneInCollision = m_outputIsInCollision.GetNativeArray(),
                m_isDroneInGameAndConnected = m_outputIsInGame.GetNativeArray(),
                m_shieldDroneInUnity = m_outputDroneUnity.GetNativeArray()
            };
            JobHandle jobHandle = job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64);
            jobHandle.Complete();
        });
    }
}




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
[System.Serializable]
public struct STRUCT_ShieldDroneCompressedBits
{
    public byte m_droneState;
    public byte m_positionUShortX1;
    public byte m_positionUShortX0;
    public byte m_positionUShortY1;
    public byte m_positionUShortY0;
    public byte m_positionUShortZ1;
    public byte m_positionUShortZ0;
    public byte m_rotationEulerX;
    public byte m_rotationEulerY;
    public byte m_rotationEulerZ;
    public byte m_shieldState;

   
}

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
        numberFloat = value * 0.001f;
    }
}

