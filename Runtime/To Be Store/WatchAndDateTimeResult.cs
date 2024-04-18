using System;
using System.Diagnostics;

[System.Serializable]
public class WatchAndDateTimeResult { 

    [UnityEngine.SerializeField]string m_watchTimeLabel = "Watch Time of";
    [UnityEngine.SerializeField] double m_WatchTimeInTick;
    [UnityEngine.SerializeField] double m_watchTimeInMilliseconds;
    [UnityEngine.SerializeField] double m_dateTimeInMilliseconds;

    public void WatchTheAction(Action action)
    {
        WatchAndDateTimeResult result = this;
        WatchTheAction(action, ref result);
    }
    public void WatchTheAction(Action action, ref WatchAndDateTimeResult result)
    {
        if (result == null) result = new WatchAndDateTimeResult();

        DateTime start = DateTime.Now;
        Stopwatch sw = new Stopwatch();
        sw.Start();
        action.Invoke();
        sw.Stop();
        result.m_dateTimeInMilliseconds = (DateTime.Now - start).TotalMilliseconds;
        result.m_WatchTimeInTick = sw.ElapsedTicks;
        result.m_watchTimeInMilliseconds = sw.ElapsedMilliseconds;
    }
}
