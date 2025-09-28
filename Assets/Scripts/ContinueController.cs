using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ContinueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreResultText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button titleButton;

    private void Start()
    {
        int lastScore = PlayerPrefs.GetInt("lastScore", 0);
        scoreResultText.text = $"Score: {lastScore}";

        retryButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        titleButton.onClick.AddListener(() => SceneManager.LoadScene("Title"));
    }
}
