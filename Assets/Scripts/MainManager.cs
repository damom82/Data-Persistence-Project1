using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestScoreText;
    public GameObject GameOverText;

    public static string PlayerName;

    public int HighScore;
    public string HighScorePlayerName;

    private int m_Points;
    private bool m_Started = false;
    private bool m_GameOver = false;

    private void Awake()
    {
        Instance = this;
        LoadHighScore();
    }

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position =
                    new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);

                Brick brick =
                    Instantiate(BrickPrefab, position, Quaternion.identity);

                brick.PointValue = pointCountArray[i];

                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        UpdateBestScoreUI();
    }

    void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;

                float randomDirection =
                    Random.Range(-1.0f, 1.0f);

                Vector3 forceDir =
                    new Vector3(randomDirection, 1, 0);

                forceDir.Normalize();

                Ball.transform.SetParent(null);

                Ball.AddForce(
                    forceDir * 2.0f,
                    ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;

        ScoreText.text =
            $"Score : {m_Points} Player : {PlayerName}";
    }

    public void GameOver()
    {
        m_GameOver = true;

        GameOverText.SetActive(true);

        if (m_Points > HighScore)
        {
            HighScore = m_Points;
            HighScorePlayerName = PlayerName;

            SaveHighScore();

            UpdateBestScoreUI();
        }
    }

    void UpdateBestScoreUI()
    {
        BestScoreText.text =
            $"Best Score : {HighScorePlayerName} : {HighScore}";
    }

    [System.Serializable]
    class SaveData
    {
        public int HighScore;
        public string PlayerName;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();

        data.HighScore = HighScore;
        data.PlayerName = HighScorePlayerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(
            Application.persistentDataPath + "/savefile.json",
            json);
    }

    public void LoadHighScore()
    {
        string path =
            Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data =
                JsonUtility.FromJson<SaveData>(json);

            HighScore = data.HighScore;
            HighScorePlayerName = data.PlayerName;
        }
    }
}