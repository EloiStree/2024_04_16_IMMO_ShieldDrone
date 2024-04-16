using DroneIMMO;
using Unity.Collections;
using UnityEngine;

public class NativeArrayMono_BigBytesArrayOfDrones

    : MonoBehaviour
{
    public NativeArray_BigBytesArrayOfDrones m_droneAsNativeBytes= new NativeArray_BigBytesArrayOfDrones();

    public void Awake()
    {
        m_droneAsNativeBytes.Create();
    }
    public void OnDestroy()
    {
        m_droneAsNativeBytes.Destroy();
    }
}
public class NativeArray_BigBytesArrayOfDrones
{


    public NativeArray<byte> m_dronesAsBytes;

    public void Create()
    {
        m_dronesAsBytes = new NativeArray<byte>(
        NativeGeneric16KUtility.ARRAY_MAX_SIZE *
        ShieldDroneAsUShortUtility.SizeInBytes()
        , Allocator.Persistent);
    }


    public void Destroy()
    {
        m_dronesAsBytes.Dispose();
    }
}