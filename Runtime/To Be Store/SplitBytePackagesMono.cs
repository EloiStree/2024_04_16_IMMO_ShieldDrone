using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBytePackagesMono : MonoBehaviour
{

    public byte[][] m_playerBase = new byte[64][];

    public void Init()
    {
        m_playerBase = new byte[64][];
        for (int i = 0; i < 16; i++)
        {

            m_playerBase[i] = new byte[1+4+10240];
        }
    }

    public void PushNativeArray(int frame, int groupOfset) { 
    

    
    }

}
