using UnityEngine;
using UnityEngine.UI;

public class SpiritUI : MonoBehaviour
{
    public Image[] spiritIcons;          // isi 3 Image
    public Sprite filledSpirit;

    public void UpdateSpiritIcons()
    {
        int total = Spirit.totalSpirits;
        int collected = Spirit.collectedSpirits;

        float percent = (float)collected / total;

        int activeCount = 0;
        if (percent >= 1f) activeCount = 3;
        else if (percent >= 2f / 3f) activeCount = 2;
        else if (percent >= 1f / 3f) activeCount = 1;

        for (int i = 0; i < spiritIcons.Length; i++)
        {
            if (i < activeCount)
            {
                spiritIcons[i].sprite = filledSpirit;
                spiritIcons[i].enabled = true;
            }
            else
            {
                spiritIcons[i].enabled = false;
            }
        }
    }

    public void ResetSpiritUI()
    {
        foreach (var img in spiritIcons)
        {
            img.enabled = false;
        }
    }
}
