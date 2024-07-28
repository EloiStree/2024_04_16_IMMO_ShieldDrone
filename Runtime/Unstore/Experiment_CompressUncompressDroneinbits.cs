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
    public SNAM16K_ObjectBool                m_inputIsInGame;
    public SNAM16K_ObjectBool               m_inputIsInCollision;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_inputDroneUnity;
    public SNAM16K_ShieldDroneCompressedAsBits  m_inputDroneBits;


    public BigByteNativeArrayCompressedDrone16KMono m_inputBigByteArray;
    public BigByteArrayCompressedDrone16KMono m_bigByteArray;
    public BigByteNativeArrayCompressedDrone16KMono m_outputBigByteArray;

    [Header("Client Side")]
    public SNAM16K_ShieldDroneCompressedAsBits m_outputDroneBits;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_outputDroneUnity;
    public SNAM16K_ObjectBool m_outputIsInGame;
    public SNAM16K_ObjectBool m_outputIsInCollision;

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
    public WatchAndDateTimeActionResult m_a;
    private void TurnShieldDroneInUnityToBitsStruct()
    {
        m_a.StartCounting();
        ///
        m_a.StopCounting();

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
        m_bitsToUnityJob.WatchTheAction((Action)(() =>
        {
            STRUCT_Job_TurnDroneBitsIntoDroneUnity job = new global::STRUCT_Job_TurnDroneBitsIntoDroneUnity()
            {
                m_bigByteCompressed = m_outputBigByteArray.GetBytesNativeArray(),
                m_shieldDroneAsBits = m_outputDroneBits.GetNativeArray(),
                m_isDroneInCollision = m_outputIsInCollision.GetNativeArray(),
                m_isDroneInGameAndConnected = m_outputIsInGame.GetNativeArray(),
                m_shieldDroneInUnity = m_outputDroneUnity.GetNativeArray()
            };
            JobHandle jobHandle = job.Schedule(IMMO16K.ARRAY_MAX_SIZE, 64);
            jobHandle.Complete();
        }));
    }
}

