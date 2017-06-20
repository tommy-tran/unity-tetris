﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {

	int m_score = 0;
	int m_lines = 0;
	int m_level = 1;

	public int m_linesPerLevel = 5;

	const int m_minLines = 1;
	const int m_maxLines = 4;
	public Text m_linesText;
	public Text m_levelText;
	public Text m_scoreText;

	public void ScoreLines(int n)
	{
		n = Mathf.Clamp (n, m_minLines, m_maxLines);

		switch (n) 
		{
		case 1:
			m_score += 50 * m_level;
			break;
		case 2:
			m_score += 100 * m_level;
			break;
		case 3:
			m_score += 300 * m_level;
			break;
		case 4:
			m_score += 1200 * m_level;
			break;
		}

		UpdateUI ();
	}

	public void Reset()
	{
		m_level = 1;
		m_lines = m_linesPerLevel * m_level;
	}

	void UpdateUI()
	{
		if (m_linesText) {
			m_linesText.text = m_lines.ToString();
		}

		if (m_levelText) {
			m_levelText.text = m_level.ToString();
		}

		if (m_scoreText) {
			m_scoreText.text = m_score.ToString();
		}
	}

	// Use this for initialization
	void Start () {
		Reset ();
	}
}
