using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ServerFrequencePushMono : MonoBehaviour
{

    public UnityEvent m_frequence_EverySecond;
    public UnityEvent m_frequence_2PerSeconds;
    public UnityEvent m_frequence_4PerSeconds;
    public UnityEvent m_frequence_8PerSeconds;
    public UnityEvent m_frequence_16PerSeconds;



    IEnumerator Start()
    {
        float timing = 1f/16f;
        ulong frame=0;
        while (true) {

            yield return new WaitForSeconds(timing);
            frame++;

            m_frequence_16PerSeconds.Invoke();
            if (frame % 2 == 0)
            {
                m_frequence_8PerSeconds.Invoke();
            }
            if (frame % 4 == 0)
            {
                m_frequence_4PerSeconds.Invoke();
            }
            if (frame % 8 == 0)
            {
                m_frequence_2PerSeconds.Invoke();
            }
            if (frame % 16 == 0)
            {
                m_frequence_EverySecond.Invoke();
            }
        } 
    }

    
}
