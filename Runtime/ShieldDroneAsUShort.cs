using System;

[System.Serializable]
public struct ShieldDroneAsUShort
{
    public byte m_gameState; //0= Undefined, 1=Playing and alive.  Is it Alive, Death, Playig, Connected...
    public byte m_quadrantIndex;
    public ushort m_quadrantRightX;
    public ushort m_quadrantHeightY;
    public ushort m_quadrantDepthZ;
    public short m_angleLeftRight360;
    public sbyte m_percentDronePitch;
    public sbyte m_percentDroneRoll;
    public ushort m_percentShieldState;
}

[System.Serializable]
public struct ShieldDroneAsUShortBytes
{
    public byte B0_gameState;
    public byte B0_quadrantIndex;
    public byte B0_quadrantRightX;
    public byte B1_quadrantRightX;
    public byte B0_quadrantHeightY;
    public byte B1_quadrantHeightY;
    public byte B0_quadrantDepthZ;
    public byte B1_quadrantDepthZ;
    public byte B0_angleLeftRight360;
    public byte B1_angleLeftRight360;
    public byte B0_percentDronePitch;
    public byte B0_percentDroneRoll;
    public byte B0_percentShieldState;
    public byte B1_percentShieldState;
}




public class ShieldDroneAsUShortUtility {



    public static byte[] ToByteArray(in ShieldDroneAsUShort drone)
    {
        byte[] bytes = new byte[SizeInBytes()];
        int offset = 0;

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_gameState), 0, bytes, offset, sizeof(byte));
        offset += sizeof(byte);

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_quadrantIndex), 0, bytes, offset, sizeof(byte));
        offset += sizeof(byte);

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_quadrantRightX), 0, bytes, offset, sizeof(ushort));
        offset += sizeof(ushort);

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_quadrantHeightY), 0, bytes, offset, sizeof(ushort));
        offset += sizeof(ushort);

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_quadrantDepthZ), 0, bytes, offset, sizeof(ushort));
        offset += sizeof(ushort);

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_angleLeftRight360), 0, bytes, offset, sizeof(ushort));
        offset += sizeof(ushort);

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_percentDronePitch), 0, bytes, offset, sizeof(byte));
        offset += sizeof(byte);

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_percentDroneRoll), 0, bytes, offset, sizeof(byte));
        offset += sizeof(byte);

        Buffer.BlockCopy(BitConverter.GetBytes(drone.m_percentShieldState), 0, bytes, offset, sizeof(ushort));

        return bytes;
    }

    public static void FromByteArray(byte[] bytes ,ref ShieldDroneAsUShort drone)
    {
        int offset = 0;

        drone.m_gameState = bytes[0];
        offset += sizeof(byte);
        drone.m_quadrantIndex = bytes[1];
        offset += sizeof(byte);

        drone.m_quadrantRightX = BitConverter.ToUInt16(bytes, offset);
        offset += sizeof(ushort);

        drone.m_quadrantHeightY = BitConverter.ToUInt16(bytes, offset);
        offset += sizeof(ushort);

        drone.m_quadrantDepthZ = BitConverter.ToUInt16(bytes, offset);
        offset += sizeof(ushort);

        drone.m_angleLeftRight360 = BitConverter.ToInt16(bytes, offset);
        offset += sizeof(ushort);

        drone.m_percentDronePitch = Convert.ToSByte(bytes[offset]);
        offset += sizeof(byte);

        drone.m_percentDroneRoll = Convert.ToSByte(bytes[offset]);
        offset += sizeof(byte);

        drone.m_percentShieldState = BitConverter.ToUInt16(bytes, offset);
    }

    public static int SizeInBytes()
    {
        return  2 * 5 +  4;
    }


    public static void SetRandom(ref ShieldDroneAsUShort drone)
    {
        SetRandomPosition(ref drone);
        SetRandomTilt(ref drone);
        SetRandomShield(ref drone);
    }

    public static void SetRandomPosition(ref ShieldDroneAsUShort drone)
    {

        drone.m_quadrantRightX = (ushort)UnityEngine.Random.Range(0, ushort.MaxValue);
        drone.m_quadrantHeightY = (ushort)UnityEngine.Random.Range(0, ushort.MaxValue);
        drone.m_quadrantDepthZ = (ushort)UnityEngine.Random.Range(0, ushort.MaxValue);
        drone.m_angleLeftRight360 = (short)UnityEngine.Random.Range(-18000, 18000);

    }
    public static void SetRandomTilt(ref ShieldDroneAsUShort drone)
    {


        drone.m_percentDronePitch = (sbyte)UnityEngine.Random.Range(-128, 127);
        drone.m_percentDroneRoll = (sbyte)UnityEngine.Random.Range(-128, 127);

    }
    public static void SetRandomShield(ref ShieldDroneAsUShort drone)
    {
        drone.m_percentShieldState = (ushort)UnityEngine.Random.Range(0, ushort.MaxValue);
    }
}