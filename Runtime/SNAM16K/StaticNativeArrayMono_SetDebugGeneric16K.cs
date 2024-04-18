using DroneIMMO;
using UnityEngine;

public abstract class StaticNativeArrayMono_SetDebugGeneric16K<T> :
    MonoBehaviour
     where T : struct
{
  
    public StaticNativeArrayMono_Generic16K<T> m_targetSNAM ;

    public T[] m_arrayToSet = new T[10];
    public bool m_useValidated=true;
    public bool m_randomizeInputAtUpdate=true;
    public int m_numberOfRandomPerUpdate=2;

    public void Update()
    {
        if (m_randomizeInputAtUpdate &&  m_arrayToSet.Length>0) {
            for (int i = 0; i < m_numberOfRandomPerUpdate; i++)
            {
                m_arrayToSet[UnityEngine.Random.Range(0, m_arrayToSet.Length)] = GenerateRandomValue();
            }
            Push();
        }
    }
    public abstract T GenerateRandomValue();

    private void OnValidate()
    {
        if (m_useValidated)
            Push();
    }
    [ContextMenu("Push")]
    public void Push() {

        if (m_targetSNAM != null) { 
            for (int i = 0; i < m_arrayToSet.Length; i++)
            {
                m_targetSNAM.Set(i, m_arrayToSet[i]);
            }
        }
    }
    //private void Reset()
    //{
    //    m_targetSNAM =(StaticNativeArrayMono_Generic16K<T>) GetComponent(typeof(StaticNativeArrayMono_Generic16K<T>));
    //}


}
