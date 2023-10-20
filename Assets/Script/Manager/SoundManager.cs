using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound
{
	public string Name;
	public AudioClip AudioClip;
	[Range(0f, 1f)]
	public float Volume;
}
public class SoundManager : Singleton<SoundManager>
{
	[SerializeField] private List<Sound> sounds;

	private readonly Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
	private Dictionary<string, float> soundVolumes = new Dictionary<string, float>();

	protected override void Awake()
	{
		base.Awake();
		foreach (var sound in sounds)
		{
			var audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.clip = sound.AudioClip;
			audioSource.playOnAwake = false;

			audioSources.Add(sound.Name, audioSource);
			soundVolumes.Add(sound.Name, sound.Volume);
		}
	}

    private void Start()
    {
		UpdateSound();

	}

    public void UpdateSound()
	{
		foreach (var sound in audioSources)
		{
			sound.Value.volume = PlayerSaveManager.Instance.GetSound() ? soundVolumes[sound.Key] : 0;
		}
		audioSources["Music"].volume = PlayerSaveManager.Instance.GetMusic() ? soundVolumes["Music"] : 0;
	}
	public void PlaySound(string name)
	{
		if (audioSources.ContainsKey(name))
		{
			audioSources[name].Play();
		}
	}
	public void StopSound(string name)
	{
		if (audioSources.ContainsKey(name))
		{
			audioSources[name].Stop();
		}
	}

	public bool IsSoundPlaying(string name)
	{
		if (audioSources.ContainsKey(name))
		{
			if (audioSources[name].isPlaying == true)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}
}
