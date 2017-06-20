﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

	public bool m_musicEnabled = true;
	public bool m_fxEnabled = true;

	[Range(0,1)]
	public float m_musicVolume = 0.15f;

	[Range(0,1)]
	public float m_fxVolume = 1.0f;

	// Row clear sounds
	public AudioClip[] m_clearRowSounds;

	public AudioClip m_moveSound;

	public AudioClip m_dropSound;

	public AudioClip m_gameOverSound;

	public AudioClip m_backgroundMusic;

	public AudioSource m_musicSource;

	public AudioSource m_SFXSource;

	public AudioClip m_errorSound;

	// background music clips
	public AudioClip[] m_musicClips;

	AudioClip m_randomMusicClip;

	public IconToggle m_musicIconToggle;

	public IconToggle m_fxIconToggle;

	public AudioClip m_restartClip;

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

	public void PlayFX(AudioClip sfxClip, float volume)
	{
		/**
		Play one sound at a time
		m_SFXSource.Stop();
		m_SFXSource.clip = sfxClip;
		m_SFXSource.volume = volume;
		m_SFXSource.loop = false;
		m_SFXSource.Play(); 
		**/

		m_SFXSource.PlayOneShot(sfxClip, volume);
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
		if (m_musicIconToggle) 
		{
			m_musicIconToggle.ToggleIcon (m_musicEnabled);
		}
	}

	public void ToggleFX() {
		m_fxEnabled = !m_fxEnabled;

		if (m_fxIconToggle) 
		{
			m_fxIconToggle.ToggleIcon (m_fxEnabled);
		}
	}
}
