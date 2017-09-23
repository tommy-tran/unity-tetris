using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighScores : MonoBehaviour {

	public Text[] highscoreNames;
	public Text[] highscores;
	HighScores highScoreManager;

	// Use this for initialization
	void Start () {
		highScoreManager = GetComponent<HighScores>();
		StartCoroutine ("RefreshHighscores");
    }

	public void OnHighscoresDownloaded(Highscore[] highscoreList) {
		
		for (int i = 0; i < highscoreNames.Length; i++) {
			highscoreNames[i].text = i+1 + ". ";
			if (highscoreList.Length > i) {
				highscoreNames [i].text = highscoreList [i].username;
				highscores [i].text = highscoreList [i].score.ToString ();
			}
		}
	}
	
	IEnumerator RefreshHighscores() {
		while (true) {
			highScoreManager.DownloadHighScores ();
			yield return new WaitForSeconds(10);
		}
	}
}
