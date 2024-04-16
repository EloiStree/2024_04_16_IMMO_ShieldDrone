using DroneIMMO;
using System;
using Unity.Collections;
using UnityEngine;


public class NativeArrayMono_BigFloatDroneBase16K : MonoBehaviour
{

    public NativeArray_BigFloatDroneBase16K m_floatDronesBase16K;


    public void Awake()
    {
        m_floatDronesBase16K.Create();
    }

    public void OnDestroy()
    {
        m_floatDronesBase16K.Create();
    }
}


[System.Serializable]
public struct ShieldDroneAs9Float
{
    public float m_gameSate;  //  1: | 2: | 3: | 4: | 5: | 6: | 7: | 8: 0= Undefined, 1= Player online and alive 
    public float m_vx;
    public float m_vy;
    public float m_vz;
    public float m_qx;
    public float m_qy;
    public float m_qz;
    public float m_qw;
    public float m_shield;
}


public class NativeArray_BigFloatDroneBase16K 
{

    public static int ARRAY_MAX_SIZE = 128 * 128 * 9;
    public static int ARRAY_MAX_DRONE = 128 * 128;
    public NativeArray<float> m_indexToValue;

    public void Create()
    {
        m_indexToValue = new NativeArray<float>(ARRAY_MAX_SIZE, Allocator.Persistent);
    }
    public float [] CopyInArray()
    {
        return m_indexToValue.ToArray(); 
    }

    public void Destroy()
    {
        if (m_indexToValue != null)
            m_indexToValue.Dispose();
    }


    public void SetDrone(int index, in float[] droneValues)
    {
        for (int i = 0; i < 9; i++)
        {
            m_indexToValue[index * 9 + i] = droneValues[i];
        }
    }


    public void SetDroneGameState(in int index, in float gameState)
    {
        m_indexToValue[index * 9] = gameState;
    }
    public void SetDronePosition(in int index, in float x, in float y, in float z)
    {
        m_indexToValue[index * 9 + 1] = x;
        m_indexToValue[index * 9 + 2] = y;
        m_indexToValue[index * 9 + 3] = z;
    }
    public void SetDronePosition(in int index, in Vector3 position) {
        m_indexToValue[index * 9 + 1] = position.x;
        m_indexToValue[index * 9 + 2] = position.y;
        m_indexToValue[index * 9 + 3] = position.z;
    }

    public void SetDroneRotation(in int index, in float qx, in float qy, in float qz, in float qw)
    {
        m_indexToValue[index * 9 + 4] = qx;
        m_indexToValue[index * 9 + 5] = qy;
        m_indexToValue[index * 9 + 6] = qz;
        m_indexToValue[index * 9 + 7] = qw;
    }
    public void SetDroneRotation(in int index, in Quaternion rotation)
    {
        m_indexToValue[index * 9 + 4] = rotation.x;
        m_indexToValue[index * 9 + 5] = rotation.y;
        m_indexToValue[index * 9 + 6] = rotation.z;
        m_indexToValue[index * 9 + 7] = rotation.w;
    }

    public void SetDroneShield(in int indexDrone, in float shield) {

        m_indexToValue[indexDrone * 10 + 8] = shield;
    }
}



