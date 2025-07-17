using UnityEngine;

public class panelSetting : MonoBehaviour
{
    public GameObject settingsPanel; // drag Panel_Settings ke sini di Inspector

    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false); // Supaya tidak muncul saat start
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
