using System.Collections.Generic;
using UnityEngine;
using DroneIMMO;

public class IndexIntegerDateQueueInputMono :MonoBehaviour{

    public IndexIntegerDateStruct m_last;
    public IndexIntegerDateStruct [] m_lasts= new IndexIntegerDateStruct[10];
    public Queue<IndexIntegerDateStruct> m_queue = new Queue<IndexIntegerDateStruct>();
    public SNAM16K_IntegerPlayerIndexClaim m_userIndexClaim;

    public void Enqueue(IndexIntegerDateStruct item)
    {
        m_queue.Enqueue(item);
    }

    public void Dequeue(out IndexIntegerDateStruct item)
    {
        item = m_queue.Dequeue();
    }
    public void Update()
    {
        while (m_queue.Count > 0)
        {
           
            //move in array to next position
            for (int i = m_lasts.Length - 1; i > 0; i--)
            {
                m_lasts[i] = m_lasts[i - 1];
            }


            Dequeue(out m_last);
            m_lasts[0] = m_last;


            m_userIndexClaim.GetFromIntegerIndex( m_last.index,
                out bool found, out int index);
            if(found)
            {
                SNAM16K_IntegerUserValue.I().Set(index,m_last.value );
            }
            else
            {
                m_userIndexClaim.GetNextFree(out int indexFree);
                if (indexFree >= 0) {
                    m_userIndexClaim.ClaimArrayWithUserIntegerIndex(indexFree, m_last.index);
                    SNAM16K_IntegerUserValue.I().Set(indexFree,  m_last.value );
                }
            }
         
        }
    }
}
