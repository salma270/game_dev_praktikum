using UnityEngine;
using UnityEngine.SceneManagement;

public class levelComplete : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnLevelComplete();
        }
    }


    void OnLevelComplete()
    {
        int collected = Spirit.collectedSpirits;
        int total = Spirit.totalSpirits;

        int finalSpiritValue = 0;

        if (total > 0)
        {
            float percent = (float)collected / total;

            if (percent >= 1f) finalSpiritValue = 3;
            else if (percent >= 2f / 3f) finalSpiritValue = 2;
            else if (percent >= 1f / 3f) finalSpiritValue = 1;
            else finalSpiritValue = 0;
        }

        int levelNumber = GetCurrentLevel();

        SaveProgress(levelNumber, finalSpiritValue);
        SceneManager.LoadScene("Level"); // atau LevelSelect, tergantung desainmu
    }


    void SaveProgress(int levelNumber, int spiritCollected)
    {
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);
        if (levelNumber >= unlockedLevel)
        {
            PlayerPrefs.SetInt("unlockedLevel", levelNumber + 1);
        }

        int savedSpirit = PlayerPrefs.GetInt("spirit_Level" + levelNumber, 0);
        if (spiritCollected > savedSpirit)
        {
            PlayerPrefs.SetInt("spirit_Level" + levelNumber, spiritCollected);
        }

        PlayerPrefs.Save();
    }

    int GetCurrentLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.StartsWith("level"))
        {
            string number = sceneName.Replace("level", "");
            if (int.TryParse(number, out int level))
            {
                return level;
            }
        }
        return 1;
    }
}
