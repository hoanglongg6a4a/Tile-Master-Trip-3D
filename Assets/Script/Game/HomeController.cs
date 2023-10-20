using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image soundImage;
    [SerializeField] private Image musicImage;
    [SerializeField] private Sprite[] sprites;

    private void Start()
    {
        levelText.text = (PlayerSaveManager.Instance.GetLevel()+1).ToString();
        SoundManager.Instance.PlaySound("Music");

        musicImage.sprite = PlayerSaveManager.Instance.GetMusic() ? sprites[0] : sprites[1];
        soundImage.sprite = PlayerSaveManager.Instance.GetSound() ? sprites[2] : sprites[3];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.DeleteAll();
        }
    }
    public void HomeButton()
    {
        SoundManager.Instance.PlaySound("Button");
        SceneManager.LoadSceneAsync("Ingame");
    }
    public void MusicButton()
    {
        SoundManager.Instance.PlaySound("Button");
        PlayerSaveManager.Instance.SetMusic(!PlayerSaveManager.Instance.GetMusic());
        SoundManager.Instance.UpdateSound();
        musicImage.sprite = PlayerSaveManager.Instance.GetMusic() ? sprites[0] : sprites[1];
    }
    public void SoundButton()
    {
        SoundManager.Instance.PlaySound("Button");
        PlayerSaveManager.Instance.SetSound(!PlayerSaveManager.Instance.GetSound());
        SoundManager.Instance.UpdateSound();
        soundImage.sprite = PlayerSaveManager.Instance.GetSound() ? sprites[2] : sprites[3];
    }
}
