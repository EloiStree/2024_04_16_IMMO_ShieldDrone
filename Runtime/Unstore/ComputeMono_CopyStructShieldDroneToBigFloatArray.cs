using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Eloi.WatchAndDate;
public class ComputeMono_CopyStructShieldDroneToBigFloatArray : MonoBehaviour
{
    public SNAM16K_ShieldDroneAsUnityFloatValue  m_dronesAsFloatStruct;
    public BigFloatArrayDrone16KMono m_bigFloatArray;
    public BigByteArrayDrone16KMono m_bigByteArray;


    public ComputeShader m_computeShader;
    public ComputeBuffer m_droneFloatStruct;
    public ComputeBuffer m_dronesFloatArray;
    public int m_kernelIndex;

    public float m_watchTimeRealMs;
    public float m_watchTimeMs;

    public WatchAndDateTimeActionResult m_timeTaken;


    private void Start()
    {
        m_kernelIndex = m_computeShader.FindKernel("CopyKernel");
        // Create a compute buffer and set the data
        m_droneFloatStruct = new ComputeBuffer(128 * 128, sizeof(float) * 8);
        m_droneFloatStruct.SetData(m_dronesAsFloatStruct.GetNativeArray());
        m_computeShader.SetBuffer(m_kernelIndex, "droneStructArray", m_droneFloatStruct);

        m_dronesFloatArray = new ComputeBuffer(128 * 128, sizeof(float) * 8*4);
//        m_dronesFloatArray.SetData(FloatArrayDrone16K.get);
        m_computeShader.SetBuffer(m_kernelIndex, "droneFloatArray", m_dronesFloatArray);

    }

    public void Update()
    {
        m_timeTaken.WatchTheAction(()=>{
            float[] floatArray = m_bigFloatArray.GetFloatArray();
            m_droneFloatStruct.SetData( m_dronesAsFloatStruct.GetNativeArray() );

            m_computeShader.Dispatch(m_kernelIndex, (128 * 128) / 64, 1, 1);
            //UnityEngine.Debug.Log($"Float Array : {floatArray.Length} {m_dronesFloatArray.count}");
            m_dronesFloatArray.GetData(floatArray);

        });
    }
    private void OnDestroy()
    {

        if(m_droneFloatStruct!=null)
        m_droneFloatStruct.Release();
        if (m_dronesFloatArray != null)
        m_dronesFloatArray.Release();
    }

}
