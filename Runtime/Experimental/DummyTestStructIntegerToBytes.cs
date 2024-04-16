using DroneIMMO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

public class DummyTestStructIntegerToBytes : MonoBehaviour
{

    public NativeArrayMono_BigFloatDroneBase16K m_playerBaseAsNativeArray;


    float[] m_dronesAsFloats= new float[10*(128*128)];
    byte[] m_droneAsBytes = new byte[10 * (128 * 128)*4];

    public int m_bytesTotaleSize = 10 * (128 * 128) * 4;
    public int m_floatsTotalSize = 10 * (128 * 128);
    public float m_bytesSizeAsMB = (10 * (128 * 128) * 4) / 1024f / 1024f;
    public float m_floatSizeAsMB = (10 * (128 * 128)) / 1024f / 1024f;

    public double m_timeToConvertDateMs;
    public double m_timeToConvertWatchMs;


    public float[] m_sampleFloatIn = new float[10];
    public byte[]  m_sampleIn = new byte[40];
    public float[] m_sampleFloatOut = new float[ 10];


    public void Awake()
    {
        ConvertToBytes();
    }

    public void Update()
    {
        ConvertToBytes();
    }

    [ContextMenu("Convert")]
    public void ConvertToBytes()
    {
       m_bytesTotaleSize = 10* (128 * 128) * 4;
       m_floatsTotalSize = 10 * (128 * 128);
       m_bytesSizeAsMB = (10 * (128 * 128) * 4) / 1024f / 1024f;
       m_floatSizeAsMB = (10 * (128 * 128)) / 1024f / 1024f;
       

        for (int i = 0; i < 10; i++)
        {
            m_dronesAsFloats[i] = UnityEngine.Random.Range(-float.MaxValue, float.MaxValue);
        }

        m_nativeToFloatBytes = new TimeDateWatch(() => {
            m_dronesAsFloats = m_playerBaseAsNativeArray.m_floatDronesBase16K.m_indexToValue.ToArray();
        });
        m_nativeToFloatBytes.DoAndWatch();



        Array.Copy(m_dronesAsFloats, 0, m_sampleFloatIn,0,10);

        m_floatToBytes = new TimeDateWatch(() => {
            Buffer.BlockCopy(m_dronesAsFloats, 0, m_droneAsBytes, 0, m_dronesAsFloats.Length * sizeof(float));
        });
        m_floatToBytes.DoAndWatch();

        Array.Copy(m_droneAsBytes, 0, m_sampleIn, 0, 4*10);



        m_byteToFloat = new TimeDateWatch(() => {

            Buffer.BlockCopy(m_droneAsBytes, 0, m_dronesAsFloats, 0, m_dronesAsFloats.Length / sizeof(float));
        });
        Array.Copy(m_dronesAsFloats, 0, m_sampleFloatOut, 0, 10);

        m_byteToFloat.DoAndWatch();

    }

    public TimeDateWatch m_nativeToFloatBytes;
    public TimeDateWatch m_floatToBytes;
    public TimeDateWatch m_bytesToUdpPack;
    public TimeDateWatch m_byteToFloat;

    [System.Serializable]
    public class TimeDateWatch
    {
        public double m_timeDateMs;
        public double m_timeWatchMs;
        public Action m_whatToDo;

        public TimeDateWatch(Action whatToDo  )
        {
            m_whatToDo = whatToDo;
        }

        public void DoAndWatch()
        {
            DoAndWatchTime(m_whatToDo, out m_timeDateMs, out m_timeWatchMs);
        }

        public static void DoAndWatchTime(Action action, out double timeDateMs, out double timeWatchMs)
        {
            long timeStart = DateTime.Now.Ticks;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();
            long timeStop = DateTime.Now.Ticks;
            timeWatchMs = stopwatch.Elapsed.TotalMilliseconds;
            timeDateMs = (timeStop - timeStart) / 10000f;
        }
    }
 
}



