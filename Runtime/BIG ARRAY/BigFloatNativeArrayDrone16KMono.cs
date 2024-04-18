using Unity.Collections;
using UnityEngine;

public class BigFloatNativeArrayDrone16KMono : MonoBehaviour{

    public NativeArray<float> GetNativeArray() {
        return BigFloatNativeArrayDrone16K.GetNativeArray();
    }
}
