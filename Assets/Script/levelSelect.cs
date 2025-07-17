using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelSelect : MonoBehaviour
{
    public LevelButtonUI[] levelButtons; // drag 5 button

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            bool unlocked = levelIndex <= unlockedLevel;
            int spiritCount = PlayerPrefs.GetInt("spirit_Level" + levelIndex, 0);
            

            levelButtons[i].Setup(unlocked, spiritCount, "level" + levelIndex);
        }
    }
}
