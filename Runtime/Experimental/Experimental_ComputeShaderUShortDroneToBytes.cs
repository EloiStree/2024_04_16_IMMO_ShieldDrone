//using DroneIMMO;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Collections;
//using UnityEngine;

//public class Experimental_ComputeShaderUShortDroneToBytes : MonoBehaviour
//{
//    public ComputeShader m_computeShader;
//    public NativeArrayMono_ShieldDrone16K m_drones;
//    public NativeArrayMono_BigBytesArrayOfDrones m_droneAsBytes;

//    private ComputeBuffer m_shieldDroneBufferDrones;
//    private ComputeBuffer m_shieldDroneBufferBytes;

//    public int bufferSizeX = 128;
//    public int bufferSizeY = 128;
//    NativeArray<ShieldDroneAsUShort> nativeArrayDrones;
//    NativeArray<byte> nativeArrayBytes;
    

//    void Update()
//    {
//        if (m_shieldDroneBufferDrones == null) {
//            nativeArrayDrones = m_drones.GetGenericNativeArray().m_indexToValue;
//            nativeArrayBytes = m_droneAsBytes.m_droneAsNativeBytes.m_dronesAsBytes;
//            m_shieldDroneBufferDrones = new ComputeBuffer(NativeGeneric16KUtility.ARRAY_MAX_SIZE,16);
//            m_shieldDroneBufferBytes = new ComputeBuffer(NativeGeneric16KUtility.ARRAY_MAX_SIZE, 16);
//        }

//        m_shieldDroneBufferDrones.SetData(nativeArrayDrones);
//        m_shieldDroneBufferBytes.SetData(nativeArrayBytes);
//        m_computeShader.SetBuffer(kernelIndex: 0, name: "shieldDroneBuffer", buffer: m_shieldDroneBufferDrones);
//        m_computeShader.SetBuffer(kernelIndex: 0, name: "byteArrayBuffer", buffer: m_shieldDroneBufferBytes);

//        int numGroupsX = Mathf.CeilToInt(bufferSizeX / 8f); 
//        int numGroupsY = Mathf.CeilToInt(bufferSizeY / 8f); 
//        m_computeShader.Dispatch(kernelIndex: 0, numGroupsX, numGroupsY, 1);





//    }
//}
