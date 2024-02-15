using Mirror;
using Mirror.Examples.Basic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class SetNameOnIcon : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text m_name;

    // Start is called before the first frame update
    void Start()
    {
        DisplayIconName();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void DisplayIconName()
    {
        m_name.text = connectionToClient.m_name;
        CMDDisplayIconName(connectionToClient.m_name);
    }

    [Command(requiresAuthority =false)]
    private void CMDDisplayIconName(string name)
    {
        m_name.text = name;
        RPCDisplayIconName(name);
    }

    [ClientRpc]
    private void RPCDisplayIconName(string name)
    {
        m_name.text = name;
    }
}
