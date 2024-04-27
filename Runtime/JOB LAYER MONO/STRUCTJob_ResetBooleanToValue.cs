using Unity.Collections;
using Unity.Jobs;

public struct STRUCTJob_ResetBooleanToValue : IJobParallelFor
{


    [WriteOnly]
    public NativeArray<bool> m_inCollision;


    public bool m_newValue;
    public void Execute(int index)
    {
            m_inCollision[index] = m_newValue;
    }
}