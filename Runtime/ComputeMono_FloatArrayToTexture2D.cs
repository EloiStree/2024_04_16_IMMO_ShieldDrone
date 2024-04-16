using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ComputeMono_FloatArrayToTexture2D : MonoBehaviour
{
    public ComputeShader m_computeShader;

    private ComputeBuffer m_inputBuffer;


    private RenderTexture m_resultTexture;
    public UnityEvent<Texture> m_onTextureCreated;


    public double m_watchTimeMs;

    public double m_watchTimeMsSetDate;
    public double m_watchTimeMsSetBuffer;
    public double m_watchTimeMsDispatch;

    public void Awake()
    {

        m_resultTexture = new RenderTexture(400, 400, 0, RenderTextureFormat.ARGBFloat);
        m_resultTexture.enableRandomWrite = true;
        m_resultTexture.Create();

        m_onTextureCreated.Invoke(m_resultTexture);
    }
    private void Start()
    {
        NativeArray<float> inputArray = FloatArrayDrone16K.GetNative();
        // Create a compute buffer for the input array
        m_inputBuffer = new ComputeBuffer(inputArray.Length, sizeof(float));
        m_inputBuffer.SetData(inputArray);


    }
    public int m_kernelIndex;
    public void Update()
    {
        Stopwatch swt = new Stopwatch();
        swt.Reset();
        swt.Start();
        m_kernelIndex = m_computeShader.FindKernel("ConvertToTexture");
        NativeArray<float> inputArray = FloatArrayDrone16K.GetNative();
        // Create a compute buffer for the input array
        //inputBuffer = new ComputeBuffer(inputArray.Length, sizeof(float));
        m_inputBuffer.SetData(inputArray);


        StoreTiming(swt, ref m_watchTimeMsSetDate);
        // Set the compute shader parameters
        m_computeShader.SetTexture(0, "resultTexture", m_resultTexture);
        m_computeShader.SetBuffer(0, "inputArray", m_inputBuffer);
        m_computeShader.SetInt("widthTexture2D", m_resultTexture.width);
        m_computeShader.SetInt("maxFloatLenght", inputArray.Length);
        StoreTiming(swt, ref m_watchTimeMsSetBuffer);

        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(m_resultTexture.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(m_resultTexture.height / 8.0f);
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
        m_inputBuffer.Release();
    }
}