using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class QuickDummy_CopyStructToArrayFloatShieldDrone : MonoBehaviour
{
    public NativeArrayMono_FloatShieldDrone16k m_dronesAsFloatStruct;


    public ComputeShader m_computeShader;
    public ComputeBuffer m_droneFloatStruct;
    public ComputeBuffer m_dronesFloatArray;
    public int m_kernelIndex;

    public float m_watchTimeRealMs;
    public float m_watchTimeMs;

    private void Start()
    {
        m_kernelIndex = m_computeShader.FindKernel("CopyKernel");
        // Create a compute buffer and set the data
        m_droneFloatStruct = new ComputeBuffer(128 * 128, sizeof(float) * 9);
        m_droneFloatStruct.SetData(m_dronesAsFloatStruct.m_nativeArray.m_indexToValue);
        m_computeShader.SetBuffer(m_kernelIndex, "droneStructArray", m_droneFloatStruct);

        m_dronesFloatArray = new ComputeBuffer(128 * 128, sizeof(float) * 9);
//        m_dronesFloatArray.SetData(FloatArrayDrone16K.get);
        m_computeShader.SetBuffer(m_kernelIndex, "droneFloatArray", m_dronesFloatArray);

    }

    public void Update()
    {
        m_watchTimeRealMs = Time.realtimeSinceStartup; 
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        m_computeShader.Dispatch(m_kernelIndex, (128 * 128) / 64, 1, 1);
        m_dronesFloatArray.GetData(FloatArrayDrone16K.GetAsArray());
        m_watchTimeRealMs = Time.realtimeSinceStartup - m_watchTimeMs;
        stopwatch.Stop();
        m_watchTimeMs = stopwatch.ElapsedMilliseconds; 
    }
    private void OnDestroy()
    {

        if(m_droneFloatStruct!=null)
        m_droneFloatStruct.Release();
        if (m_dronesFloatArray != null)
        m_dronesFloatArray.Release();
    }

}
