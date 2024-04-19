using Eloi.WatchAndDate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class SplitAndPushBytesAsFrameMono : MonoBehaviour
{

    public BigByteArrayCompressedDrone16KMono m_source;

    public int m_droneSendMultipleOf2 = 32;





    [Header("Don't touch")]


    public int m_elementByteSize = 11;
    public int m_droneSendPerChunk = 32 * 32;
    public int m_chunkStartByteSize = 4 + 4 + 4 + 4 + 8 + 8+8;
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
    [ContextMenu("Push Frame")]

    public void SetDateTimeOfWhenTheFrameStartToBuild(DateTime time) {
        m_startBuildingFrameTimestamp = time.Ticks;

    }

    public byte[] m_lastPushChunk;
    private UdpClient m_udpClient = null;
    private List<IPEndPoint> m_endPoints= new List<IPEndPoint>();

    void PushFrame()
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

            m_lastPushChunk = new byte[m_bytePerChunkWithStart];
            BitConverter.GetBytes(m_frameIndex).CopyTo(m_lastPushChunk, 0);
            BitConverter.GetBytes(m_chunkIndex).CopyTo(m_lastPushChunk, 4);
            BitConverter.GetBytes(copyOffset).CopyTo(m_lastPushChunk, 8);
            BitConverter.GetBytes(copyLength ).CopyTo(m_lastPushChunk, 12);
            BitConverter.GetBytes(m_startBuildingFrameTimestamp>0? m_startBuildingFrameTimestamp: m_sendTimestampBlock).CopyTo(m_lastPushChunk, 16);
            BitConverter.GetBytes(m_sendTimestampBlock ).CopyTo(m_lastPushChunk, 24);
            BitConverter.GetBytes(m_sendTimestampChunk).CopyTo(m_lastPushChunk, 32);
            Buffer.BlockCopy(source, copyOffset, m_lastPushChunk, 40, m_bytePerChunkWithoutStart);

            try
            {
                if (m_udpClient == null) {
                    m_udpClient = new UdpClient();
                    m_endPoints.Add(new IPEndPoint(IPAddress.Parse(m_ipAddress), m_port));
                }
                foreach (var target in m_endPoints)
                {
                    m_udpClient.Send(m_lastPushChunk, m_lastPushChunk.Length, target);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending byte: " + ex.Message);
            }
           
            m_pushed = m_lastPushChunk;
            m_pushChunk.StopCounting();
            m_chunkIndex++;
        }
        m_pushFrame.StopCounting();
    }
     void OnDestroy()
    {
        if (m_udpClient != null)
            m_udpClient.Close();
    }
}
