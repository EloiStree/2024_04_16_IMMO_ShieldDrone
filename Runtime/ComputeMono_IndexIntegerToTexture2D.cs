using DroneIMMO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ComputeMono_IndexIntegerToTexture2D : MonoBehaviour
{

    public ComputeShader m_computeShader;

    private ComputeBuffer m_inputBufferIndex;
    private ComputeBuffer m_inputBufferValue;


    public RenderTexture m_resultTextureIndex;
    public UnityEvent<Texture> m_onTextureCreatedIndex;

    public RenderTexture m_resultTextureValue;
    public UnityEvent<Texture> m_onTextureCreatedValue;


    public double m_watchTimeMs;

    public double m_watchTimeMsSetDate;
    public double m_watchTimeMsSetBuffer;
    public double m_watchTimeMsDispatch;

    public SNAM_IntegerPlayerIndexClaim m_userIndexClaim;
    public SNAM_IntegerUserValue m_userValue;

    public void Awake()
    {

        m_resultTextureIndex = new RenderTexture(128, 128, 0, RenderTextureFormat.ARGBFloat);
        m_resultTextureIndex.enableRandomWrite = true;
        m_resultTextureIndex.Create();

        m_onTextureCreatedIndex.Invoke(m_resultTextureIndex);

        m_resultTextureValue = new RenderTexture(128, 128, 0, RenderTextureFormat.ARGBFloat);
        m_resultTextureValue.enableRandomWrite = true;
        m_resultTextureValue.Create();

        m_onTextureCreatedValue.Invoke(m_resultTextureValue);
    }
    private void Start()
    {


        m_inputBufferIndex = new ComputeBuffer(128 * 128, sizeof(int));
        m_inputBufferValue = new ComputeBuffer(128 * 128, sizeof(int));


    }
    public int m_kernelIndex;
    public void Update()
    {
        Stopwatch swt = new Stopwatch();
        swt.Reset();
        swt.Start();
        m_kernelIndex = m_computeShader.FindKernel("ConvertToTexture");




        StoreTiming(swt, ref m_watchTimeMsSetDate);
        // Set the compute shader parameters
        m_computeShader.SetTexture(0, "resultTextureIndex", m_resultTextureIndex);
        m_computeShader.SetTexture(0, "resultTextureValue", m_resultTextureValue);

        var naIndex = m_userIndexClaim.GetNativeArray();
        var naValue = m_userValue.GetNativeArray();
        m_inputBufferIndex.SetData(naIndex);
        m_computeShader.SetBuffer(0, "inputArrayIndex", m_inputBufferIndex);

        m_inputBufferValue.SetData(naValue);
        m_computeShader.SetBuffer(0, "inputArrayValue", m_inputBufferValue);

        if (naIndex == naValue)
            UnityEngine.Debug.LogError("naIndex==naValue");


        m_computeShader.SetInt("widthTexture2D", m_resultTextureIndex.width);

        StoreTiming(swt, ref m_watchTimeMsSetBuffer);

        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(m_resultTextureIndex.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(m_resultTextureIndex.height / 8.0f);
        m_computeShader.Dispatch(m_kernelIndex, threadGroupsX, threadGroupsY, 1);

        StoreTiming(swt, ref m_watchTimeMsDispatch);


    }

    private void StoreTiming(Stopwatch swt, ref double toAffect)
    {
        swt.Stop();
        toAffect = swt.Elapsed.TotalMilliseconds;
        swt.Reset();
        swt.Start();
    }

    private void OnDestroy()
    {
        // Release the compute buffer
        m_inputBufferIndex.Release();
        m_inputBufferValue.Release();
    }
}
