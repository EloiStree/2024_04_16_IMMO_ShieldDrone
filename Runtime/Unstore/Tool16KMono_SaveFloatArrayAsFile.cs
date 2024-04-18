using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tool16KMono_SaveFloatArrayAsFile : MonoBehaviour
{

    public Eloi.AbstractMetaAbsolutePathDirectoryMono m_whereToStore;
    public BigByteArrayDrone16KMono m_playerBaseAsByte;


    [ContextMenu("Capture")]
    public void Capture() {

       string path= Path.Combine(m_whereToStore.GetPath(), $"Drone16K_{ DateTime.UtcNow.ToString("yyyyMMddhhmmsssffff") }");
       Directory.CreateDirectory(m_whereToStore.GetPath());
       if (!File.Exists(path)) {
            File.WriteAllBytes(path, m_playerBaseAsByte.GetBytesArray() );
       }
    }
}
