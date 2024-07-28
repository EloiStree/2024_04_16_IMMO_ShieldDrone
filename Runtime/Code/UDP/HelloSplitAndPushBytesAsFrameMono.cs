using Eloi.WatchAndDate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class HelloSplitAndPushBytesAsFrameMono : MonoBehaviour
{

    public UnityEvent<byte[]> m_onByteChunkPush;

    public UnityEvent<string , byte[]> m_onNamedByteChunkPush;

    public BigByteArrayCompressedDrone16KMono m_source;

    public byte m_arrayUniqueIdInProject=1;
    public int m_droneSendMultipleOf2 = 32;

    public string m_arrayName="DroneByteArray";



    [Header("Don't touch")]
    int m_elementByteSize = 11;
    int m_droneSendPerChunk = 32 * 32;
    int m_chunkStartByteSize =1 + 4 + 4 + 4 + 4 + 8 + 8+8;
    public int m_bytePerChunkWithoutStart;
    public int m_bytePerChunkWithStart;

    [Header("Debug")]
    public uint m_frameIndex = 0;
    public uint m_chunkIndex;
    public long m_sendTimestampBlock;
    public long m_sendTimestampChunk;
    public long m_startBuildingFrameTimestamp;

    private void OnValidate()
    {
        RefreshComputeValue();
    }

    private void RefreshComputeValue()
    {
        m_droneSendPerChunk = m_droneSendMultipleOf2 * m_droneSendMultipleOf2;
        //FRAME INDEX    ,  CHUNK INDEX, Block TIME A UTC NOW, Chunk TIME A UTC NOW
        m_bytePerChunkWithoutStart =  (m_droneSendPerChunk * m_elementByteSize);
        m_bytePerChunkWithStart = m_chunkStartByteSize + m_bytePerChunkWithoutStart;
        m_chunkToSend = IMMO16K.ARRAY_MAX_SIZE / m_droneSendPerChunk;
    }

    public bool m_usePushRepeatDefault = true;
    public float m_timeBetweenPush=1f/4f;
    public bool m_useStoreByteToDebug;
    public bool m_sendOnlyFirstChunkForTesting=true;
    private void Start()
    {

        InvokeRepeating("PushFrame", m_timeBetweenPush, m_timeBetweenPush);
    }

    public WatchAndDateTimeActionResult m_pushFrame;
    public WatchAndDateTimeActionResult m_pushChunk;
    public int m_chunkToSend;

    public string m_ipAddress= "81.240.94.97";
    public int m_port=3715;

    public byte[] m_pushed;

    public void SetDateTimeOfWhenTheFrameStartToBuild(DateTime time)
    {
        m_startBuildingFrameTimestamp = time.Ticks;

    }
    public void SetDateTimeOfWhenTheFrameStartToBuildNow()
    {
        m_startBuildingFrameTimestamp = DateTime.UtcNow.Ticks;

    }

    public byte[] m_lastPushChunk;
    public byte[] m_firstPushChunk;
    private UdpClient m_udpClient = null;
    private List<IPEndPoint> m_endPoints= new List<IPEndPoint>();

    public List<ChunkOfByte> m_chunkSent = new List<ChunkOfByte>();

    [System.Serializable]
    public class ChunkOfByte {
        public ChunkOfByte(int size) { 
        
            m_lastChunkSent = new byte[size];
        }
        public byte[] m_lastChunkSent;
    }
    [ContextMenu("Push Frame")]
    public void PushFrame()
    {
        RefreshComputeValue();
        byte [] source = m_source.GetBytesArray();
        m_pushFrame.StartCounting();
        m_frameIndex++;
        m_chunkIndex=0;
        m_sendTimestampBlock = DateTime.UtcNow.Ticks;
        
        for (int i = 0; i < m_chunkToSend; i++)
        {
            int copyOffset = i * m_bytePerChunkWithoutStart;
            int copyLength = m_bytePerChunkWithoutStart;
            m_sendTimestampChunk = DateTime.UtcNow.Ticks;
            m_pushChunk.StartCounting();
            while (i >= m_chunkSent.Count)
                m_chunkSent.Add(new ChunkOfByte( m_bytePerChunkWithStart));

            byte[] inProcessChunk = m_chunkSent[i].m_lastChunkSent ;
            inProcessChunk[0] = m_arrayUniqueIdInProject;
            BitConverter.GetBytes(m_frameIndex).CopyTo(inProcessChunk, 1);
            BitConverter.GetBytes(m_chunkIndex).CopyTo(inProcessChunk, 5);
            BitConverter.GetBytes(copyOffset).CopyTo(inProcessChunk, 9);
            BitConverter.GetBytes(copyLength ).CopyTo(inProcessChunk, 13);
            BitConverter.GetBytes(m_startBuildingFrameTimestamp>0? m_startBuildingFrameTimestamp: m_sendTimestampBlock).CopyTo(inProcessChunk, 17);
            BitConverter.GetBytes(m_sendTimestampBlock ).CopyTo(inProcessChunk, 25);
            BitConverter.GetBytes(m_sendTimestampChunk).CopyTo(inProcessChunk, 33);
            Buffer.BlockCopy(source, copyOffset, inProcessChunk, 41, m_bytePerChunkWithoutStart);


            m_onByteChunkPush.Invoke(inProcessChunk);
            m_onNamedByteChunkPush.Invoke(m_arrayName, inProcessChunk);


            m_pushed = inProcessChunk;
            m_pushChunk.StopCounting();
            m_chunkIndex++;
            if (m_sendOnlyFirstChunkForTesting)
                break;
        }
        m_pushFrame.StopCounting();
    }
    
}
