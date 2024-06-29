using Eloi.WatchAndDate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNAM16K_UpdateRandomSeed : DroneIMMO.StaticNativeArrayMono_Generic16K<uint> {



    public WatchAndDateTimeActionResult m_useTime;
    public float m_refreshDelay = 0.1f;

    private IEnumerator Start()
    {
        while(true)
        {

            ChangeSeeds();
            yield return new WaitForSeconds(m_refreshDelay);
            yield return new WaitForEndOfFrame();

        }
    }

    [ContextMenu("Change Seeds")]
    public void ChangeSeeds() {

        m_useTime.StartCounting();
        var array = SNAM16K_UpdateRandomSeed.I().GetNativeArray();
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (uint)Random.Range(0, uint.MaxValue);
        }
        m_useTime.StopCounting();
    }

}
//TO D


