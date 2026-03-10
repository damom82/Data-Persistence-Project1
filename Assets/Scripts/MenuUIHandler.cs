using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField NameInput;
    public TextMeshProUGUI BestScoreText;

    private void Start()
    {
        if (MainManager.Instance != null)
        {
            BestScoreText.text =
                "Best Score : " +
                MainManager.Instance.HighScorePlayerName +
                " : " +
                MainManager.Instance.HighScore;
        }
    }

    public void StartNew()
    {
        MainManager.PlayerName = NameInput.text;

        SceneManager.LoadScene(1);
    }
}