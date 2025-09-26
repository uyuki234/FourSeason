using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeasonSwitcher : MonoBehaviour
{
    [Header("表示テキスト（中央の季節表示）")]
    [SerializeField] private TextMeshProUGUI seasonText;

    [Header("季節ボタン（春・夏・秋・冬の順に割り当て）")]
    [SerializeField] private Button btnHaru;
    [SerializeField] private Button btnNatsu;
    [SerializeField] private Button btnAki;
    [SerializeField] private Button btnFuyu;

    [Header("オプション")]
    [SerializeField] private bool enableKeyboardShortcut = true; // 1=春, 2=夏, 3=秋, 4=冬

    // 0:春, 1:夏, 2:秋, 3:冬
    private readonly string[] SEASON_KANJI = { "春", "夏", "秋", "冬" };
    private int current = -1;

    private void Awake()
    {
        // Inspector の割り当て忘れ対策（最初に見つかった TMP を使う）
        if (seasonText == null)
        {
            seasonText = FindObjectOfType<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        // ボタンのイベント登録
        if (btnHaru != null) btnHaru.onClick.AddListener(() => OnPressed(0));
        if (btnNatsu != null) btnNatsu.onClick.AddListener(() => OnPressed(1));
        if (btnAki != null)  btnAki.onClick.AddListener(() => OnPressed(2));
        if (btnFuyu != null) btnFuyu.onClick.AddListener(() => OnPressed(3));

        // 最初の季節をランダムに表示
        SetNextSeason(initial: true);
    }

    private void Update()
    {
        if (!enableKeyboardShortcut) return;

        // キーボード上段1〜4 と テンキー1〜4 に対応（任意）
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) OnPressed(0); // 春
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) OnPressed(1); // 夏
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) OnPressed(2); // 秋
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) OnPressed(3); // 冬
    }

    private void OnPressed(int index)
    {
        // 正解（表示中と同じ季節）なら別の季節に切替
        if (index == current)
        {
            SetNextSeason();
        }
        // 不正解は何もしない（演出を入れたければここで赤フラッシュなど可能）
    }

    private void SetNextSeason(bool initial = false)
    {
        int next;
        do
        {
            next = Random.Range(0, SEASON_KANJI.Length); // 0〜3
        } while (!initial && next == current); // 同じ季節の連続を避ける

        current = next;

        if (seasonText != null)
        {
            seasonText.text = SEASON_KANJI[current];
        }
        else
        {
            Debug.LogWarning("seasonText（TextMeshProUGUI）が未割り当てです。");
        }
    }
}