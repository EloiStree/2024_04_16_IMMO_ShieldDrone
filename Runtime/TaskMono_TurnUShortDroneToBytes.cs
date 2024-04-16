using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DroneIMMO;
using System.Runtime.InteropServices;
using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;


public class TaskMono_TurnUShortDroneToBytes : MonoBehaviour
{
    public NativeArrayMono_ShieldDrone16K m_drones;
    public NativeArrayMono_BigBytesArrayOfDrones m_droneAsBytes;
    public float m_timeBetweenPush = 0.5f;

    public long m_timePerUpdateJobTick = 1000;
    public float m_timePerUpdateJob = 0.0f;
    public int m_sizeOfElement = 0;
    public int m_totaleByteToSend = 0;
    public double m_totaleByteToSendAsKiloByte = 0;
    public double m_totaleByteToSendAsMegaByte = 0;

    public JobExe_NativeDroneToByteArray m_jobExecuter;
    public JobHandle m_jobHandle;




    void Start()
    {



        InvokeRepeating("Push", m_timeBetweenPush, m_timeBetweenPush);
        
    }

    public void Push()
    {

        m_totaleByteToSend = m_droneAsBytes.m_droneAsNativeBytes.m_dronesAsBytes.Length;
        m_totaleByteToSendAsKiloByte = m_totaleByteToSend /1024;
        m_totaleByteToSendAsMegaByte = m_totaleByteToSendAsKiloByte / 1024 /1024;
        long time_start;
        time_start = System.DateTime.Now.Ticks;
        m_sizeOfElement = ShieldDroneAsUShortUtility.SizeInBytes();
        m_jobExecuter = new JobExe_NativeDroneToByteArray
        {
            m_structSize = m_sizeOfElement,
            m_drones = m_drones.m_nativeArray.m_indexToValue//,
          //  m_bytes = m_droneAsBytes.m_droneAsNativeBytes.m_dronesAsBytes
        };


        m_jobHandle = m_jobExecuter.Schedule(NativeGeneric16KUtility.ARRAY_MAX_SIZE, 64);

        m_jobHandle.Complete();

        m_timePerUpdateJobTick = (System.DateTime.Now.Ticks - time_start);
        m_timePerUpdateJob = m_timePerUpdateJobTick * 0.0001f;

    }
}


//[BurstCompile(CompileSynchronously = true)]
public struct JobExe_NativeDroneToByteArray : IJobParallelFor
{
    public int m_structSize ;
    public NativeArray<ShieldDroneAsUShort> m_drones;
    public NativeArray<ShieldDroneAsUShortBytes> m_bytes;

    public static void ConvertToBytes(in ushort value, ref byte b1, ref byte b2)
    {
        b1 = (byte)(value & 0xFF);
        b2 = (byte)((value >> 8) & 0xFF);
    }
    public static void ConvertToBytes(ref ushort value,  in byte b1,  byte b2)
    {
        value = (ushort)((b2 << 8) | b1);
    }

    public static void ConvertToBytes(in short value, ref byte b1, ref byte b2)
    {
        b1 = (byte)(value & 0xFF);
        b2 = (byte)((value >> 8) & 0xFF);
    }
    public static void ConvertToBytes(ref short value, in byte b1, byte b2)
    {
        value = (short)((b2 << 8) | b1);
    }
    public void Execute(int index)
    {
        ShieldDroneAsUShort drone = m_drones[index];
        ShieldDroneAsUShortBytes droneByte= m_bytes[index];

        byte []byteTemp;
        int offsetStart = index * m_structSize;

        droneByte.B0_gameState = drone.m_gameState;
        droneByte.B0_quadrantIndex = drone.m_quadrantIndex;

        ConvertToBytes(drone.m_quadrantRightX, ref droneByte.B0_quadrantRightX, ref droneByte.B1_quadrantRightX);
        ConvertToBytes(drone.m_quadrantHeightY, ref droneByte.B0_quadrantHeightY, ref droneByte.B1_quadrantHeightY);
        ConvertToBytes(drone.m_quadrantDepthZ, ref droneByte.B0_quadrantDepthZ, ref droneByte.B1_quadrantDepthZ);
        ConvertToBytes(drone.m_angleLeftRight360, ref droneByte.B0_angleLeftRight360, ref droneByte.B1_angleLeftRight360);


        //droneByte.B0_quadrantIndex = drone.m_quadrantIndex;
        //droneByte.B0_quadrantIndex = drone.m_quadrantIndex;
        //byteTemp = BitConverter.GetBytes(drone.m_percentDronePitch);
        //m_bytes[offsetStart + 10] = drone.m_percentDronePitch;
        //byteTemp = BitConverter.GetBytes(drone.m_percentDroneRoll);
        //m_bytes[offsetStart + 11] = drone.m_percentDroneRoll;

        //ConvertToBytes(drone.m_percentShieldState, ref droneByte.B0_percentShieldState, ref droneByte.B1_percentShieldState);
        //ConvertToBytes(drone.m_percentShieldState);
        //m_bytes[offsetStart + 12] = byteTemp[0];
        //m_bytes[offsetStart + 13] = byteTemp[1];
    }
}






public class AA {

    public byte[] ConvertStructToByteArray(ShieldDroneAsUShort data)
    {
        int size = Marshal.SizeOf(data);
        byte[] byteArray = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(data, ptr, true);
        Marshal.Copy(ptr, byteArray, 0, size);
        Marshal.FreeHGlobal(ptr);

        return byteArray;
    }

    public void D() {


        /// QUESTION: WOULD IT BE MORE FAST THAT BYTE TO ARRAY TO BYTE ?
        /// DOES MARSHAL WORK IN JOBS SYSTEM? IT IT FASTER ? 

        NativeArray<ShieldDroneAsUShort> m_drones = new NativeArray<ShieldDroneAsUShort>(128 * 128, Allocator.Persistent);
        // Convert m_drones to a byte array
        int sizeOfStruct = Marshal.SizeOf<ShieldDroneAsUShort>();
        int byteCount = sizeOfStruct * m_drones.Length;
        byte[] byteArray = new byte[byteCount];
        GCHandle handle = GCHandle.Alloc(m_drones, GCHandleType.Pinned);
        try
        {
            IntPtr ptr = handle.AddrOfPinnedObject();
            Marshal.Copy(ptr, byteArray, 0, byteCount);
        }
        finally
        {
            handle.Free();
        }
    }

    public static byte[] StructArrayToByteArray(ShieldDroneAsUShort[] array)
    {
        //int structSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(ShieldDroneAsUShort));
        int structSize = ShieldDroneAsUShortUtility.SizeInBytes();
        byte[] byteArray = new byte[array.Length * structSize];

        for (int i = 0; i < array.Length; i++)
        {
            byte[] structBytes = StructToByteArray(array[i]);
            Array.Copy(structBytes, 0, byteArray, i * structSize, structSize);
        }

        return byteArray;
    }

    public static byte[] StructToByteArray(ShieldDroneAsUShort str)
    {
        int size = System.Runtime.InteropServices.Marshal.SizeOf(str);
        byte[] arr = new byte[size];

        IntPtr ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
        System.Runtime.InteropServices.Marshal.StructureToPtr(str, ptr, true);
        System.Runtime.InteropServices.Marshal.Copy(ptr, arr, 0, size);
        System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);

        return arr;
    }
}