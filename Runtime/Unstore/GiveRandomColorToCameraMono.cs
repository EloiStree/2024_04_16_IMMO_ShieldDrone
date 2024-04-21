using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveRandomColorToCameraMono : MonoBehaviour
{
    public Camera m_affected;
    public bool m_randomColorAtAwake;
    // Start is called before the first frame update
    void Awake()
    {
        if(m_randomColorAtAwake)
        RandomColor();
    }

    [ContextMenu("Random Color")]
    private void RandomColor()
    {
        m_affected.backgroundColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
    }

    void Reset()
    {
        m_affected = GetComponent<Camera>();
    }
}
