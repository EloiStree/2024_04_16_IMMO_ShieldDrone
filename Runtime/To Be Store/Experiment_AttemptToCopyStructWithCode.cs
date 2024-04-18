using System.Runtime.InteropServices;
using System;
using Unity.Collections;

public class Experiment_AttemptToCopyStructWithCode {

    public byte[] ConvertStructToByteArray(STRUCT_ShieldDroneAsUnity data)
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

        NativeArray<STRUCT_ShieldDroneAsUnity> m_drones = new NativeArray<STRUCT_ShieldDroneAsUnity>(128 * 128, Allocator.Persistent);
        // Convert m_drones to a byte array
        int sizeOfStruct = Marshal.SizeOf<STRUCT_ShieldDroneAsUnity>();
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

    public static byte[] StructArrayToByteArray(STRUCT_ShieldDroneAsUnity[] array)
    {
        //int structSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(ShieldDroneAsUShort));
        int structSize = sizeof(float)*9;
        byte[] byteArray = new byte[array.Length * structSize];

        for (int i = 0; i < array.Length; i++)
        {
            byte[] structBytes = StructToByteArray(array[i]);
            Array.Copy(structBytes, 0, byteArray, i * structSize, structSize);
        }

        return byteArray;
    }

    public static byte[] StructToByteArray(STRUCT_ShieldDroneAsUnity str)
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