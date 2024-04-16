using System;
using UnityEngine;


namespace DroneIMMO
{
    public class NativeArrayMono_Generic16K<T> : MonoBehaviour where T : struct
    {
        [Tooltip("Unused info to represent what is store inside.")]
        public T m_sample;

        /// <summary>
        /// Allows to ahve native array that are persistent and can be used in jobs. With 128x128 elements in it for all.
        /// </summary>
        [HideInInspector]
        public NativeArray_Generic16K<T> m_nativeArray = new NativeArray_Generic16K<T>();

        public NativeArray_Generic16K<T> GetGenericNativeArray()
        {
            return m_nativeArray;
        }

        public void Awake()
        {
            m_nativeArray.Create();
        }
        public  void GetRandom(out int index, out T drone)
        {
            index = UnityEngine.Random.Range(0, NativeArray_Generic16K<T>.ARRAY_MAX_SIZE);
            Get(index, out drone);
        }
        public void Get(int index, out T valueInArray)
        {
            m_nativeArray.Get(index, out valueInArray);
        }
        public void Set(int index, T valueInArray)
        {
            m_nativeArray.Set(index, valueInArray);
        }
        public void OnDestroy()
        {
            m_nativeArray.Destroy();
        }
    }
}