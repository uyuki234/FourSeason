using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public enum Season { Spring = 0, Summer = 1, Autumn = 2, Winter = 3 }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI seasonText; // 春/夏/秋/冬 の表示
    [SerializeField] private TextMeshProUGUI timerText;  // 残り時間
    [SerializeField] private TextMeshProUGUI scoreText;  // スコア

    [Header("Buttons")]
    [SerializeField] private Button springButton;
    [SerializeField] private Button summerButton;
    [SerializeField] private Button autumnButton;
    [SerializeField] private Button winterButton;

    [Header("Game Settings")]
    [SerializeField] private float gameDurationSeconds = 30f;

    // オプション：間違い時の時間ペナルティ（使わないなら false）
    [SerializeField] private bool enablePenaltyOnWrong = false;
    [SerializeField] private float penaltySeconds = 0.5f;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource correctSfx;
    [SerializeField] private AudioSource wrongSfx;

    private float timeRemaining;
    private int score;
    private bool isPlaying;
    private Season currentTarget;

    private void Start()
    {
        // クリックイベント登録（インスペクタでやってもOK）
        springButton.onClick.AddListener(() => OnSeasonButtonClicked((int)Season.Spring));
        summerButton.onClick.AddListener(() => OnSeasonButtonClicked((int)Season.Summer));
        autumnButton.onClick.AddListener(() => OnSeasonButtonClicked((int)Season.Autumn));
        winterButton.onClick.AddListener(() => OnSeasonButtonClicked((int)Season.Winter));

        StartGame();
    }

    private void StartGame()
    {
        timeRemaining = gameDurationSeconds;
        score = 0;
        isPlaying = true;

        UpdateTimerText();
        UpdateScoreText();

        NextQuestion();
    }

    private void Update()
    {
        if (!isPlaying) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            UpdateTimerText();
            EndGame();
            return;
        }

        UpdateTimerText();

        // テスト用：キーボード入力（任意）
        // 数字キー1-4で春夏秋冬
        if (Input.GetKeyDown(KeyCode.Alpha1)) OnSeasonButtonClicked((int)Season.Spring);
        if (Input.GetKeyDown(KeyCode.Alpha2)) OnSeasonButtonClicked((int)Season.Summer);
        if (Input.GetKeyDown(KeyCode.Alpha3)) OnSeasonButtonClicked((int)Season.Autumn);
        if (Input.GetKeyDown(KeyCode.Alpha4)) OnSeasonButtonClicked((int)Season.Winter);
    }

    private void UpdateTimerText()
    {
        // 小数2桁表示
        timerText.text = timeRemaining.ToString("F2");
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }

    private void NextQuestion()
    {
        // ランダムに次の季節を選ぶ
        currentTarget = (Season)Random.Range(0, 4);
        seasonText.text = SeasonToKanji(currentTarget);
        // 見た目でわかりやすく、色分けしてもOK（任意）
        // seasonText.color = SeasonToColor(currentTarget);
    }

    public void OnSeasonButtonClicked(int seasonIndex)
    {
        if (!isPlaying) return;

        Season chosen = (Season)seasonIndex;
        bool correct = (chosen == currentTarget);

        if (correct)
        {
            score++;
            if (correctSfx) correctSfx.Play();
        }
        else
        {
            if (enablePenaltyOnWrong)
            {
                timeRemaining = Mathf.Max(0f, timeRemaining - penaltySeconds);
            }
            if (wrongSfx) wrongSfx.Play();
        }

        UpdateScoreText();
        NextQuestion();
    }

    private void EndGame()
    {
        isPlaying = false;

        // 入力を無効にして多重遷移を防ぐ
        SetButtonsInteractable(false);

        // スコアを次のシーンへ渡す（簡単な方法：PlayerPrefs）
        PlayerPrefs.SetInt("lastScore", score);
        PlayerPrefs.Save();

        // シーン遷移
        SceneManager.LoadScene("Continue"); // シーン名は実際のものに合わせて
    }

    private void SetButtonsInteractable(bool interactable)
    {
        springButton.interactable = interactable;
        summerButton.interactable = interactable;
        autumnButton.interactable = interactable;
        winterButton.interactable = interactable;
    }

    private static string SeasonToKanji(Season s)
    {
        switch (s)
        {
            case Season.Spring: return "春";
            case Season.Summer: return "夏";
            case Season.Autumn: return "秋";
            case Season.Winter: return "冬";
        }
        return "?";
    }

    // 任意：色分けしたいとき
    // private static Color SeasonToColor(Season s)
    // {
    //     switch (s)
    //     {
    //         case Season.Spring: return new Color(1f, 0.6f, 0.8f); // ピンク
    //         case Season.Summer: return new Color(0.3f, 0.7f, 1f); // 水色
    //         case Season.Autumn: return new Color(1f, 0.6f, 0.2f); // 橙
    //         case Season.Winter: return new Color(0.8f, 0.9f, 1f); // 薄い青
    //     }
    //     return Color.white;
    // }
}
