using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TDD_PlayerRandomInputController : MonoBehaviour
{
    public IndexIntegerDateQueueInputMono m_waitingInput;
    public int m_minMilliseconds = 100;
    public int m_maxMilliseconds = 100;
    public int m_minInput = 1;
    public int m_maxInput = 30;
    public int m_ignoreFirstIndex = 1000;
    public int m_maxIndex =(128*128)-1000;
    public void Start()
    {
        random = new System.Random();
        Thread thread = new Thread(new ThreadStart(RandomAction));
        thread.Start();

        if (m_initWithFullPlayerIn) {

            for (int i = 0; i < m_maxIndex; i++)
            {
                EnqueueFirst16KoIndexPlayer();
            }

        }

    }
    System.Random random = new System.Random();
    public int RandomInt(int min, int max) {
        return random.Next(min, max);
    }
    public int m_index;
    public bool m_initWithFullPlayerIn;

    


    void RandomAction()
    {
        
        while (true)
        {
            for (int i = 0; i < RandomInt(m_minInput, m_maxInput); i++)
            {
                EnqueueFirst16KoIndexPlayer();
            }

            int waitMilliseconds = RandomInt(m_minMilliseconds, m_maxMilliseconds);
            Thread.Sleep(waitMilliseconds);
        }
        
    }

    private void EnqueueFirst16KoIndexPlayer()
    {
        m_index++;
        IndexIntegerDateStruct item = new IndexIntegerDateStruct();
        item.index = m_index % m_maxIndex; //RandomInt(m_ignoreFirstIndex, m_maxIndex);
        item.value = (
            600000000
            + RandomInt(1, 99)
            + RandomInt(1, 99) * 100
            + RandomInt(1, 99) * 10000
            + RandomInt(1, 99) * 1000000);

        item.date = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds();
        m_waitingInput.Enqueue(item);
    }
}



[System.Serializable]
public struct IndexIntegerStruct
{
    public int index;
    public int value;
}
[System.Serializable]
public struct IndexIntegerDateStruct
{
    public int index;
    public int value;
    public ulong date;
}
