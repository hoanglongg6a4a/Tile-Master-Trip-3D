using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider comboBar;
    [SerializeField] private TextMeshProUGUI textComboBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject panel;
    [SerializeField] private bool isFreeze = false;
    private float timeRemaining;

    public bool IsFreeze { get => isFreeze; set => isFreeze = value; }
    public Slider ComboBar { get => comboBar; set => comboBar = value; }

    public void Init(float timeRemaining,int level)
    {
        this.timeRemaining = timeRemaining;
        this.levelText.text = "LV " +(level+1).ToString();
        StartCoroutine(Countdown());
    }
    private IEnumerator Countdown()
    {
        while (timeRemaining > 0)
        {
            if (isFreeze)
            {
                yield return new WaitForSeconds(10f);
                isFreeze = false;
            }
            yield return new WaitForSeconds(1.0f);
            timeRemaining -= 1f;
            float minus = Mathf.FloorToInt(timeRemaining / 60);
            float second = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minus, second);
        }
    }
    public void SetValueComboBar(float value, int comboCount)
    {
        ShowComBobar(value > 0);
        comboBar.value = value;
        textComboBar.text = comboCount.ToString();
    }
    public void ShowComBobar(bool status)
    {
        comboBar.gameObject.SetActive(status);
        textComboBar.gameObject.SetActive(status);
    }
    public void ShowScore(float score)
    {
        scoreText.text = score.ToString();
    }
    public void SettingButton()
    {
        SceneManager.LoadSceneAsync("Home");
    }
    public void ShowResult(bool isWin)
    {
        panel.SetActive(true);
        if (isWin)
        {
            titleText.text = "Complete";
            buttonText.text = "Next Level";
        }
        else
        {
            titleText.text = "Failure";
            buttonText.text = "Retry";
        }
    }
    public void PopupButton()
    {
        SceneManager.LoadSceneAsync("Ingame");
    }
}
