using Eloi.WatchAndDate;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;

public class ServerPushShieldDroneToBytesArray : MonoBehaviour
{
    [Header("Server Side")]
    public SNAM16K_ObjectBoolean m_inputIsInGame;
    public SNAM16K_ObjectBoolean m_inputIsInCollision;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_inputDroneUnity;
    public SNAM16K_ShieldDroneCompressedAsBits m_inputDroneBits;

    public BigByteNativeArrayCompressedDrone16KMono m_inputBigByteArray;
    public BigByteArrayCompressedDrone16KMono m_bigByteArray;
    // Start is called before the first frame update
    public float m_shieldMaxSize = ushort.MaxValue;
    public UnityEvent m_pushStart;
    public WatchAndDateTimeActionResult m_unityToBitsJob;
    public WatchAndDateTimeActionResult m_structToByteJob;
    [ContextMenu("Refresh push")]
    public void RefreshPush()
    {
        m_pushStart.Invoke();
        TurnShieldDroneInUnityToBitsStruct();
        TurnBitsStructToBytesBigArray();

    }

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
}
