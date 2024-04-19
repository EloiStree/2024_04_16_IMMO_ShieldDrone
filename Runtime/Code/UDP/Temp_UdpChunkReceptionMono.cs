using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Temp_UdpChunkReceptionMono : MonoBehaviour
{

    public int m_lastFrame;
    public int m_lastChunkIndex;

    public long m_nowUtc;
    public long m_lastStartPushing;
    public long m_tickServerSideProduction; 
    public double m_millisecondsServerSideProduction;
    public long m_tickFromBuildToReceived;
    public double m_millisecondsfromBuildToReceived;


    public int m_currentFrameIndex;
    public int m_chunkCurrentMaxIndex;
    public List<PackageChunkReceived> m_receivedPackage = new List<PackageChunkReceived>();

    public BigByteArrayCompressedDrone16KMono m_bigByteArray;

    public UnityEvent m_lastChunkReceived;
    public bool m_storeChunkBytesToDebugInStruct;

    [System.Serializable]
    public class PackageChunkReceived {
        public int m_lastFrame;
        public int m_lastChunkIndex;
        public int m_offsetInArray;
        public int m_offsetLenght;
        public long m_receivedPackageTime;
        public long m_sendTimeFrame;
        public long m_sendTimeBlock;
        public long m_sendTimeChunk;
        public byte[] m_lastReceivedFullChunk;

    }

    public byte[] m_lastReceivedFullChunk;
    
    public void PushIn(byte [] receivedChunk)
    {
        m_lastReceivedFullChunk = receivedChunk;

        if (receivedChunk!=null && receivedChunk.Length > 16)
        {
            int copyLenght = BitConverter.ToInt32(receivedChunk, 12);
            if (copyLenght + 40 == receivedChunk.Length) { 
                int indexFrame=BitConverter.ToInt32(receivedChunk, 0);
                int indexChunk= BitConverter.ToInt32(receivedChunk, 4);
                
                //if (indexFrame > m_lastFrame)
                   m_lastFrame = indexFrame;
                //if (indexChunk > m_lastChunkIndex)
                   m_lastChunkIndex = indexChunk;

                while (m_receivedPackage.Count <=indexChunk)
                {
                    m_receivedPackage.Add(new PackageChunkReceived());
                }
                PackageChunkReceived p = m_receivedPackage[indexChunk];

                p.m_lastChunkIndex = indexChunk;
                p.m_lastFrame = indexFrame;
                p.m_offsetInArray = BitConverter.ToInt32(receivedChunk, 8);
                p.m_offsetLenght =copyLenght;
                p.m_sendTimeFrame = BitConverter.ToInt64(receivedChunk, 16);
                p.m_sendTimeBlock = BitConverter.ToInt64(receivedChunk, 24);
                p.m_sendTimeChunk = BitConverter.ToInt64(receivedChunk, 32);
                p.m_receivedPackageTime = DateTime.UtcNow.Ticks;
                byte[] bytes = m_bigByteArray.GetBytesArray();
                Buffer.BlockCopy(receivedChunk, 40, bytes, p.m_offsetInArray, p.m_offsetLenght);
                if (m_storeChunkBytesToDebugInStruct) { 
                    m_lastReceivedFullChunk = receivedChunk;
                    p.m_lastReceivedFullChunk = receivedChunk;
                }

                if ( indexChunk ==m_receivedPackage.Count-1 && p.m_sendTimeFrame > m_lastStartPushing) {

                    m_nowUtc = DateTime.UtcNow.Ticks;
                    m_lastStartPushing = p.m_sendTimeFrame;
                    m_tickFromBuildToReceived = m_nowUtc - m_lastStartPushing;
                    m_tickServerSideProduction = p.m_sendTimeChunk - p.m_sendTimeFrame;
                    m_millisecondsServerSideProduction = (double)m_tickServerSideProduction / TimeSpan.TicksPerMillisecond;
                    m_millisecondsfromBuildToReceived = (double)m_tickFromBuildToReceived / TimeSpan.TicksPerMillisecond;
                    m_lastChunkReceived.Invoke();
                }

            }

        }
    }

}
