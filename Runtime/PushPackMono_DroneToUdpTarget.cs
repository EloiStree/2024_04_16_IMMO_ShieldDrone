using Eloi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPackMono_DroneToUdpTarget : MonoBehaviour
{
     public UdpTargetRelay m_udpTargetRelay;
}




[System.Serializable]
public class WantedInformation {

    public int m_frequenceForAll;
    public List<int> m_integersOwned = new List<int>();
}



[System.Serializable]
public class UdpTargetRelay
{
    public string m_ip;
    public int m_port;
    public WantedInformation wantedInformation;
}

[System.Serializable]
public class WebsocketTargetRelay
{
    public string m_websocketServerUrl="ws://";
    public WantedInformation wantedInformation;
}

