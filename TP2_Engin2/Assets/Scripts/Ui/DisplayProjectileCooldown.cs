using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayProjectileCooldown : MonoBehaviour
{
    [SerializeField]
    private Image m_bombCooldownImage;
    [SerializeField]
    private Image m_bigBombCooldownImage;

    // Update is called once per frame
    void Update()
    {
        UpdateBombCooldownImage(NetworkMatchManager._Instance.GetLocalPlayerBulletRemainingPercentage());
        UpdateBigBombCooldownImage(NetworkMatchManager._Instance.GetBombRemainingPercentage());
    }

    private void UpdateBombCooldownImage(float cooldown)
    {
        m_bombCooldownImage.fillAmount = cooldown;
    }

    private void UpdateBigBombCooldownImage(float cooldown)
    {
        m_bigBombCooldownImage.fillAmount = cooldown;
    }
}
