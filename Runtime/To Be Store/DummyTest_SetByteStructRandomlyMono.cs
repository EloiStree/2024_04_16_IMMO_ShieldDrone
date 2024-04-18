using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTest_SetByteStructRandomlyMono : MonoBehaviour
{
    public BigFloatArrayDrone16KMono m_bigFloatArray;
    public SNAM16K_ShieldDroneAs9Float16K m_droneAsStruct;
    public float m_timeBetweenRandomValues=1;

    public float m_numberPerChange = 100;

    public bool m_applyOnFloadArray;
    public bool m_applyOnStructArray;
    IEnumerator Start()
    {

        while (true)
        {
           
            SetRandomValues();

            if (m_timeBetweenRandomValues < 0.1f)
                yield return new WaitForEndOfFrame();
            else
                yield return new WaitForSeconds(m_timeBetweenRandomValues);
        }

    }
    public void SetRandomValues() {

        for (int j = 0; j < m_numberPerChange; j++)
        {

            int randomIndex = Random.Range(0, 128 * 128);
            if (m_applyOnFloadArray) {

                for (int i = 0; i < 9; i++) {
                    var n = m_bigFloatArray.GetFloatArray();
                    n[randomIndex * 9 + i] = RF();
                }
            }

            if (m_applyOnStructArray) {
                var na = m_droneAsStruct.GetNativeArray();
                na[randomIndex] = new ShieldDroneAs9Float(

                    )
                {   m_gameSate = RF(), 
                    m_shield = RF(),
                    m_vx = RF(),
                    m_vy = RF(),
                    m_vz = RF(),
                    m_qx = RF(),
                    m_qy = RF(), 
                    m_qz = RF(),
                    m_qw = RF()
            };
            }
        }
    }

    private float RF()
    {
        return Random.Range(float.MinValue, float.MaxValue);
    }
}
