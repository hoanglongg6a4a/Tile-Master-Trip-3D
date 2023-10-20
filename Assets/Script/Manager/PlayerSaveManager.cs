using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveManager : Singleton<PlayerSaveManager>
{
    private const string MusicKey = "mck";
    private const string SoundKey = "sdk";
    private const string LevelKey = "lvk";

	public bool GetMusic()
	{
		return PlayerPrefs.GetInt(MusicKey, 1) == 1;
	}
	public void SetMusic(bool isOn)
	{
		PlayerPrefs.SetInt(MusicKey, isOn ? 1 : 0);
	}
	public bool GetSound()
	{
		return PlayerPrefs.GetInt(SoundKey, 1) == 1;
	}
	public void SetSound(bool isOn)
	{
		PlayerPrefs.SetInt(SoundKey, isOn ? 1 : 0);
	}
	public void SetLevel(int level)
	{
		PlayerPrefs.SetInt(LevelKey, level);
	}
	public int GetLevel()
	{
		return PlayerPrefs.GetInt(LevelKey, 0);
	}
}
