using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {

	public int m_score = 0;
	int m_lines = 0;
	public int m_level = 1;

	public int m_linesPerLevel = 4;

	const int m_minLines = 1;
	const int m_maxLines = 4;
	public Text m_linesText;
	public Text m_levelText;
	public Text m_scoreText;

    GameController gameController;

	public void ScoreLines(int n)
	{
		n = Mathf.Clamp (n, m_minLines, m_maxLines);

		switch (n) 
		{
		case 1:
			m_score += 40 * m_level;
			break;
		case 2:
			m_score += 80 * m_level;
			break;
		case 3:
			m_score += 200 * m_level;
			break;
		case 4:
			m_score += 800 * m_level;
			break;
		}

		m_lines -= n;
		if (m_lines <= 0) {
			LevelUp ();
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

	public void LevelUp()
	{
		m_level++;
		m_lines = m_linesPerLevel * m_level;
        if (m_level <= 5)
        {
            gameController.m_dropInterval -= 0.025f;
        }
        
	}

	// Use this for initialization
	void Start () {
		Reset ();
		UpdateUI ();
        gameController = GameObject.FindObjectOfType<GameController>();
	}
}
