using Eloi.WatchAndDate;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class ClientReadShieldDroneFromBytesArray : MonoBehaviour
{

    [Header("Client Side")]

    public BigByteArrayCompressedDrone16KMono m_bigByteArray;
    public BigByteNativeArrayCompressedDrone16KMono m_outputBigByteArray;
    public SNAM16K_ShieldDroneCompressedAsBits m_outputDroneBits;
    public SNAM16K_ShieldDroneAsUnityFloatValue m_outputDroneUnity;
    public SNAM16K_ObjectBoolean m_outputIsInGame;
    public SNAM16K_ObjectBoolean m_outputIsInCollision;


    public WatchAndDateTimeActionResult m_bytesToStructJob;
    public WatchAndDateTimeActionResult m_bitsToUnityJob;

    [ContextMenu("Refresh fetch")]
    public void RefreshFetch() {

        TurnBigBytesArrayToBitsDroneStruct();
        TurnBitsDroneStructToUnityDrone();
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
