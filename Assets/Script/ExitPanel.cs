using UnityEngine;

public class ExitPanel : MonoBehaviour
{
    public GameObject exitPanel;

    // Dipanggil dari tombol Exit di Main Menu
    public void ShowExitPanel()
    {
        exitPanel.SetActive(true);
    }

    // Dipanggil dari tombol No
    public void HideExitPanel()
    {
        exitPanel.SetActive(false);
    }

    // Dipanggil dari tombol Yes
    public void ExitGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();

        // Ini hanya akan terlihat di build, tidak di editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
