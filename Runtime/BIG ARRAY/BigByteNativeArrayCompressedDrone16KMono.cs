using Unity.Collections;
using UnityEngine;

public class BigByteNativeArrayCompressedDrone16KMono : MonoBehaviour
{

    public NativeArray<byte> GetBytesNativeArray() {
        return BigByteNativeArrayCompressedDrone16K.GetBytesNativeArray(); }
}
