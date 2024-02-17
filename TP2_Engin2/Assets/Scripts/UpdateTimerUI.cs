using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTimerUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_timerText;

    // Update is called once per frame
    void Update()
    {
        m_timerText.text = NetworkMatchManager._Instance.GetGameTimer().ToString();
    }
}
