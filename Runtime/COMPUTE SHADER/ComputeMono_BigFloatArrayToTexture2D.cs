using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

using Eloi.WatchAndDate;
public class ComputeMono_BigFloatArrayToTexture2D : MonoBehaviour
{
    public BigFloatArrayDrone16KMono m_bigFloatArray;
    public ComputeShader m_computeShader;

    private ComputeBuffer m_inputBuffer;


    private RenderTexture m_resultTexture;
    public UnityEvent<Texture> m_onTextureCreated;



    public WatchAndDateTimeActionResult m_timing;
    //public double m_watchTimeMs;
    //public double m_watchTimeMsSetDate;
    //public double m_watchTimeMsSetBuffer;
    //public double m_watchTimeMsDispatch;

    public void Awake()
    {

        m_resultTexture = new RenderTexture(400, 400, 0, RenderTextureFormat.ARGBFloat);
        m_resultTexture.enableRandomWrite = true;
        m_resultTexture.Create();

        m_onTextureCreated.Invoke(m_resultTexture);
    }
    private void Start()
    {
        m_inputBuffer = new ComputeBuffer(m_bigFloatArray.Length(), sizeof(float)) ;
        m_inputBuffer.SetData(m_bigFloatArray.GetFloatArray());


    }
    public int m_kernelIndex;
    public void Update()
    {
        m_timing.WatchTheAction(() =>
        {
        
             m_kernelIndex = m_computeShader.FindKernel("ConvertToTexture");
            // Create a compute buffer for the input array
            //inputBuffer = new ComputeBuffer(inputArray.Length, sizeof(float));
            m_inputBuffer.SetData(m_bigFloatArray.GetFloatArray());


            // Set the compute shader parameters
            m_computeShader.SetTexture(0, "resultTexture", m_resultTexture);
            m_computeShader.SetBuffer(0, "inputArray", m_inputBuffer);
            m_computeShader.SetInt("widthTexture2D", m_resultTexture.width);
            m_computeShader.SetInt("maxFloatLenght", m_bigFloatArray.Length());
       
            // Dispatch the compute shader
            int threadGroupsX = Mathf.CeilToInt(m_resultTexture.width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(m_resultTexture.height / 8.0f);
            m_computeShader.Dispatch(m_kernelIndex, threadGroupsX, threadGroupsY, 1);

        });

    }

    private void OnDestroy()
    {
        // Release the compute buffer
        m_inputBuffer.Release();
    }
}