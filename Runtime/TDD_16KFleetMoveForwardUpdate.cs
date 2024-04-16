using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_16KFleetMoveForwardUpdate : MonoBehaviour
{
    public NativeArrayMono_BigFloatDroneBase16K m_dronesFleet;


    void Start()
    {
        
    }

    public float forwardValue = 0.1f;
    public float forwardSpeed = 0.1f;

    void Update()
    {
        forwardValue += forwardSpeed * Time.deltaTime;
        for (int j = 0; j < 128; j++)
        {
            for (int i = 0; i < 128 * 128; i++)
            {
                int index= j * 128 + i;
                m_dronesFleet.m_floatDronesBase16K.SetDronePosition(index
                    , new Vector3(j, i, forwardValue));
                m_dronesFleet.m_floatDronesBase16K.SetDroneShield(index,(j/128f) * 32000 + (i / 128f) * 32000);
                
            }
        }
    }
}
