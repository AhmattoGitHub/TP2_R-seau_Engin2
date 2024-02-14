using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DisplayLoseScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject m_player;
    [SerializeField]
    private Image m_panelImage;
    [SerializeField]
    private TMP_Text m_deadText;
    [SerializeField]
    private float m_boundaryY = -10f;
    [SerializeField]
    private GameObject m_staminaBar;

    private int m_targetFontSize = 120;
    private Color m_targetedColor = Color.black;
    private float m_lerpValue = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        m_panelImage.color = Color.clear;
        m_deadText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(DidPlayerLose()) //TODO: Change condition to if the player lost the game
        {
            ActivateLoseScreen();
        }
    }

    private bool DidPlayerLose()
    {
        if (m_player.transform.position.y < m_boundaryY)
        {
            m_staminaBar.gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    private void ActivateLoseScreen()
    {
        m_panelImage.color = Color.Lerp(m_panelImage.color, m_targetedColor, m_lerpValue);

        m_deadText.gameObject.SetActive(m_panelImage.color == m_targetedColor);

        if (m_deadText.IsActive())
        {
            m_deadText.fontSize = Mathf.Lerp(m_deadText.fontSize, m_targetFontSize, m_lerpValue);
        }
    }
}
