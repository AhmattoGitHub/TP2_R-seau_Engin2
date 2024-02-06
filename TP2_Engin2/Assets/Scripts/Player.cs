using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkConnectionToClient
{
    public Player(int networkConnectionId) : base(networkConnectionId)
    {
    }

    public bool m_isARunner;
    public bool m_isReady;
    public string m_name;

    public string GetName()
    {
        return m_name;
    }
    public bool IsARunner()
    {
        return m_isARunner;
    }
    public bool IsReady()
    {
        return m_isReady;
    }
    public void SetReadyStatus(bool isReady)
    {
        m_isReady = isReady;
    }
    public void SetName(string name)
    {
        m_name = name;
    }
    public void SetTeam(bool isARunner)
    {
        m_isARunner = isARunner;
    }
}
