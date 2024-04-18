using System;
using UnityEngine;

public class Experiment_DroneBitCompression: MonoBehaviour
{

    public bool m_gameState0;
    public bool m_gameState1;

    public Vector3 m_position;
    public byte m_gameState;
    public string m_gameStateAsString;
    public STRUCT_DroneStateAndExtraDistance m_test;
    public byte[] m_intValueX = new byte[3];
    public byte[] m_intValueY = new byte[3];
    public byte[] m_intValueZ = new byte[3];

    [System.Serializable]
    public struct STRUCT_DroneStateAndExtraDistance
    {

        public bool m_isDroneActive;
        public bool m_isInCollision;
        public bool m_isXOverEqual196607;
        public bool m_isXOverEqual131071;
        public bool m_isYOverEqual196607;
        public bool m_isYOverEqual131071;
        public bool m_isZOverEqual196607;
        public bool m_isZOverEqual131071;
    }

    private void OnValidate()
    {

        FloatToArray(m_position.x, m_intValueX);
        FloatToArray(m_position.y, m_intValueY);
        FloatToArray(m_position.z, m_intValueZ);

        m_gameState = 0;

        m_gameState |= (byte)((m_test.m_isDroneActive ? 1 : 0) << 7);
        m_gameState |= (byte)((m_test.m_isInCollision ? 1 : 0) << 6);
        m_gameState |= (byte)((Over131071(m_position.x) ? 1 : 0) << 5);
        m_gameState |= (byte)((BetWeen65536To131071Or196608To262143(m_position.x) ? 1 : 0) << 4);
        m_gameState |= (byte)((Over131071(m_position.y) ? 1 : 0) << 3);
        m_gameState |= (byte)((BetWeen65536To131071Or196608To262143(m_position.y) ? 1 : 0) << 2);
        m_gameState |= (byte)((Over131071(m_position.z) ? 1 : 0) << 1);
        m_gameState |= (byte)((BetWeen65536To131071Or196608To262143(m_position.z) ? 1 : 0));

        m_gameStateAsString = Convert.ToString(m_gameState, 2).PadLeft(8, '0');
    }

    private bool Over131071(float value)
    {
        return value > 131071;
    }
    private bool BetWeen65536To131071Or196608To262143(float value)
    {
        if (value > 262143)
            value = 262143;
        return (value >= 65536 && value <= 131071) || (value >= 196608 && value <= 262143);
    }

    private static void FloatToArray(float numberFloat, byte[] byteArray)
    {
        int number = (int)(numberFloat * 1000f);
        byteArray[0] = (byte)((number >> 16) & 0xFF);
        byteArray[1] = (byte)((number >> 8) & 0xFF);
        byteArray[2] = (byte)(number & 0xFF);



    }
}


