using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public struct STRUCT_Job_SetAllInputToGamepad : IJobParallelFor
{
    [WriteOnly]
    public NativeArray<int> m_input;
    public int m_value;


    public void Execute(int index)
    {
        m_input[index] = m_value;
    }
}
