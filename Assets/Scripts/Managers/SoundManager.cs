using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public bool m_musicEnabled = true;
	public bool m_fxEnabled = true;

	[Range(0,1)]
	public float m_musicVolume = 0.15f;

	[Range(0,1)]
	public float m_fxVolume = 1.0f;

	public AudioClip m_clearRowSound;

	public AudioClip m_moveSound;

	public AudioClip m_dropSound;

	public AudioClip m_gameOverSound;

	public AudioClip m_backgroundMusic;

	public AudioSource m_musicSource;

	// background music clips
	public AudioClip[] m_musicClips;

	AudioClip m_randomMusicClip;

	public AudioClip GetRandomClip(AudioClip[] clips)
	{
		AudioClip randomClip = clips [Random.Range (0, clips.Length)];
		return randomClip;
	}

	// Use this for initialization
	void Start () {
		PlayBackgroundMusic (GetRandomClip(m_musicClips));
	}
	


	public void PlayBackgroundMusic(AudioClip musicClip)
	{
		// return if music is disabled or if musicSource or musicClip is null
		if (!m_musicEnabled || !musicClip || !m_musicSource) 
		{
			return;
		}
		// if music is playing, stop it
		m_musicSource.Stop();

		m_musicSource.clip = musicClip;

		// set music volume
		m_musicSource.volume = m_musicVolume;

		// music repeats forever
		m_musicSource.loop = true;

		// start playing
		m_musicSource.Play();
	}

	void UpdateMusic () 
	{
		if (m_musicSource.isPlaying != m_musicEnabled) 
		{
			if (m_musicEnabled) {
				PlayBackgroundMusic (GetRandomClip(m_musicClips));
			} else {
				m_musicSource.Stop ();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateMusic ();
	}

	public void ToggleMusic() {
		m_musicEnabled = !m_musicEnabled;
	}

	public void ToggleFX() {
		m_fxEnabled = !m_fxEnabled;
	}
}
