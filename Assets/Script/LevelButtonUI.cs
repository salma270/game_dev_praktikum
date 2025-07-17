using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonUI : MonoBehaviour
{
    public Button levelButton;
    public GameObject lockIcon;
    public Image[] spiritIcons; // isi 3 image
    public Sprite filledSpirit;

    private string sceneName;

    public void Setup(bool isUnlocked, int spiritCount, string sceneToLoad)
    {
        sceneName = sceneToLoad;

        levelButton.interactable = isUnlocked;
        lockIcon.SetActive(!isUnlocked);

        for (int i = 0; i < spiritIcons.Length; i++)
        {
            spiritIcons[i].enabled = (i < spiritCount);
            if (i < spiritCount)
                spiritIcons[i].sprite = filledSpirit;
        }

        if (isUnlocked)
        {
            levelButton.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
        }
    }
}
