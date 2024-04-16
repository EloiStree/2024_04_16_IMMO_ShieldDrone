
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace DroneIMMO {


    //public class CreateAndDestroyGeneric15KMono : MonoBehaviour
    //{
    //    static CreateAndDestroyGeneric15KMono()
    //    {
    //        if (CreateAndDestroyGeneric15KMono.m_instanceInScene == null)
    //        {
    //            GameObject g = new GameObject("CreateAndDestroyGeneric15KMono");
    //            m_instanceInScene = g.AddComponent<CreateAndDestroyGeneric15KMono>();
    //        }
    //    }
    //    public static CreateAndDestroyGeneric15KMono m_instanceInScene;
    //    public static CreateAndDestroyGeneric15KMono InstanceInScene
    //    {
    //        get
    //        {
    //            if (m_instanceInScene == null)
    //            {
    //                GameObject g = new GameObject("CreateAndDestroyGeneric15KMono");
    //                m_instanceInScene= g.AddComponent<CreateAndDestroyGeneric15KMono>();
    //            }
    //            return m_instanceInScene;
    //        }
    //        set
    //        {
    //            m_instanceInScene = value;
    //        }
    //    }
    //    public static Action m_onAwake;
    //    public static Action m_onDestroy;
    //    private void Awake()
    //    {
    //        InstanceInScene = this;
    //        m_onAwake.Invoke();
    //    }
    //    private void OnDestroy()
    //    {
    //        m_onDestroy.Invoke();
    //    }
    //}





    public  class StaticNativeArrayMono_Generic16K<T>: MonoBehaviour where T : struct
    {
        public static Dictionary<string, NativeArray<T>> m_globalNativeArray = new Dictionary<string, NativeArray<T>>();
        public static int ARRAY_MAX_SIZE = 128 * 128;
        public static StaticNativeArrayMono_Generic16K<T> m_instanceInScene;

        public static StaticNativeArrayMono_Generic16K<T> I()
        {
           return m_instanceInScene;
            
        }
        
        public string GetStringId() {
            return m_guid;
        }
        public NativeArray<T> GetNativeArray()
        {
            if (!m_globalNativeArray.ContainsKey(GetStringId()))
                Create();
            return m_globalNativeArray[GetStringId()];
        }

      
       

        [Tooltip("Represent what is store")]
        public T m_sample;
        public string m_guid;
         public void Reset()
        {
            Guid guid = Guid.NewGuid();
            m_guid = guid.ToString();
        }
        public virtual void Awake()
        {
            m_guid = Guid.NewGuid().ToString();
            m_instanceInScene = this;
            Debug.Log("Created SNAM " + this.GetType(), this.gameObject);
            Create();
        }
        public virtual void OnDestroy()
        {
            Debug.Log("Destroy SNAM " + this.GetType(), this.gameObject);
            Destroy();
        }


        public  void Create()
        {
            Debug.Log("Create SNAM >>>" + GetStringId(), this.gameObject);
            if(!m_globalNativeArray.ContainsKey(GetStringId()))
                m_globalNativeArray.Add(GetStringId(), new NativeArray<T>(ARRAY_MAX_SIZE, Allocator.Persistent));
            
        }

        public  void Get(int index, out T valueInArray)
        {
            valueInArray = GetNativeArray()[index];
        }
        public  T Get(int index)
        {
            return GetNativeArray()[index];
        }
        public  void Set(int index, T valueInArray)
        {
            var array = GetNativeArray();
            array[index] = valueInArray;
        }
        public   void Destroy()
        {
            NativeArray<T> nativeArray = GetNativeArray();
            if (nativeArray != null && nativeArray.IsCreated)
                nativeArray.Dispose();
        }
    }


    public class NativeGeneric16KUtility {

        public static int ARRAY_MAX_SIZE = 128 * 128;
    }
    public class NativeArray_Generic16K<T>   where T: struct 
    {
        public static int ARRAY_MAX_SIZE = 128 * 128;
        [HideInInspector]
        public NativeArray<T> m_indexToValue;

        public void Create()
        {
            m_indexToValue = new NativeArray<T>(ARRAY_MAX_SIZE, Allocator.Persistent);
        }

        public void Get(int index, out T valueInArray)
        {
            valueInArray = m_indexToValue[index];
        }
        public void Set(int index, T valueInArray)
        {
            m_indexToValue[index] = valueInArray;
        }
        public void Destroy()
        {
            if(m_indexToValue!=null)
                m_indexToValue.Dispose();
        }
       
    }
}