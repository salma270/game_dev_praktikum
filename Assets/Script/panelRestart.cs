using UnityEngine;
using UnityEngine.SceneManagement;

public class panelRestart : MonoBehaviour
{
    public GameObject restartPanel; // drag Panel_Restart ke sini di Inspector

    void Start()
    {
        if (restartPanel != null)
            restartPanel.SetActive(false);
    }

    public void ShowRestartPanel()
    {
        restartPanel.SetActive(true);
    }

    public void HideRestartPanel()
    {
        restartPanel.SetActive(false);
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll(); // Hapus semua progress
        PlayerPrefs.Save();
        SceneManager.LoadScene("mainmenu"); // Atau ganti ke level 1 kalau kamu mau langsung main
    }
}
