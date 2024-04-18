using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMono_ParseFloatToByteShieldDrone : MonoBehaviour
{
    public BigFloatArrayDrone16KMono m_bigFloatArray;
    public BigByteArrayDrone16KMono m_bigByteArray;
    public WatchAndDateTimeResult m_copyBufferTime;
   
    void Update()
    {
        m_copyBufferTime.WatchTheAction(() => { 
            float[] floatArray = m_bigFloatArray.GetFloatArray();
            byte[] byteArray = m_bigByteArray.GetBytesArray();
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
        });
    }
}
