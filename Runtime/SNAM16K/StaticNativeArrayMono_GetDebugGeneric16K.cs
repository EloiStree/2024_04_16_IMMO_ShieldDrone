using DroneIMMO;
using UnityEngine;

public class StaticNativeArrayMono_GetDebugGeneric16K<T> :
    MonoBehaviour
     where T : struct
{

    public StaticNativeArrayMono_Generic16K<T> m_targetSNAM;

    public T[] m_arrayFetched= new T[10];
    public bool m_useUpdateFetch=true;

    private void Update()
    {
        if(m_useUpdateFetch)
            Fetch();
    }

    [ContextMenu("Fetch")]
    public void Fetch()
    {

        if (m_targetSNAM != null)
        {
            for (int i = 0; i < m_arrayFetched.Length; i++)
            {
                m_arrayFetched[i]= m_targetSNAM.Get(i);
            }
        }

    }

    //private void Reset()
    //{
    //    m_targetSNAM = (StaticNativeArrayMono_Generic16K<T>)GetComponent(typeof(StaticNativeArrayMono_Generic16K<T>));
    //}

}