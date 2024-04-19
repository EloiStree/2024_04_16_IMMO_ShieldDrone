using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HasReceivedChangedUpdateReference : MonoBehaviour{

    public bool m_hasReceivedChangedDuringThisUpdate;
    public DateTime m_lastUpdateUTC;
    public long m_lastUpdateUtcTick;
    public UnityEvent m_onNotify;
    public void NotifyChangeHappened() {

        m_hasReceivedChangedDuringThisUpdate = true;
        m_lastUpdateUTC = DateTime.UtcNow;
        m_lastUpdateUtcTick = m_lastUpdateUTC.Ticks;
        m_onNotify.Invoke();
    }

    public IEnumerator Start()
    {
        while (true) {

            yield return new  WaitForEndOfFrame();
            m_hasReceivedChangedDuringThisUpdate = false;
        }
    }

    public bool HasChangedThisUpdate()
    {
        return m_hasReceivedChangedDuringThisUpdate;
    }
}