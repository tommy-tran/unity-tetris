using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// reference to board
	Board m_gameBoard;

	// reference to spawner 
	Spawner m_spawner;

	// reference to holder
	Holder m_holder;

	// active
	Shape m_activeShape;

	// Managers
	SoundManager m_soundManager;
	ScoreManager m_scoreManager;

	public float m_dropInterval = 0.15f;
	float m_timeToDrop;
	float m_timeToNextKey;


	[Range(0.02f, 1f)]
	public float m_keyRepeatRateLeftRight = 0.1f;
	float m_timeToNextKeyLeftRight;


	[Range(0.01f, 1f)]
	public float m_keyRepeatRateDown = 0.025f;
	float m_timeToNextKeyDown;

	[Range(0.02f, 1f)]
	public float m_keyRepeatRateRotate = 0.083f;
	float m_timeToNextKeyRotate;

	[Range(0.2f, 0.8f)]
	public float m_settleTimeDelay = 0.5f;

	float m_settleTime;

	bool flag;
	bool m_gameOver = false;

	public IconToggle m_rotateIconToggle;
	bool m_clockwise = true;
	int holdCount;

	public bool m_isPaused = false;

	public GameObject m_pausePanel;

	public GameObject m_gameOverPanel;

	// Use this for initialization
	void Start () 
	{
		
		// find spawner and board with GameObject.FindWithTag plus GetComponent; make sure you tag your objects correctly
		//m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
		//m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

		// find spawner and board with generic version of GameObject.FindObjectOfType, slower but less typing
		m_gameBoard = GameObject.FindObjectOfType<Board>();
		m_spawner = GameObject.FindObjectOfType<Spawner>();
		m_soundManager = GameObject.FindObjectOfType<SoundManager> ();
		m_scoreManager = GameObject.FindObjectOfType<ScoreManager> ();
		m_holder = GameObject.FindObjectOfType<Holder> ();

		m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
		m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
		m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
			
		if (!m_gameBoard)
		{
			Debug.LogWarning("WARNING!  There is no game board defined!");
		}

		if (!m_soundManager) 
		{
			Debug.LogWarning("WARNING!  There is no sound manager defined!");
		}

		if (!m_scoreManager) 
		{
			Debug.LogWarning("WARNING!  There is no score manager defined!");
		}

		if (!m_spawner) {
			Debug.LogWarning ("WARNING!  There is no spawner defined!");
		} else {
			m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
			if (!m_activeShape) {
				m_activeShape = m_spawner.SpawnShape ();
				m_settleTime = 1;
			}
		}

		if (!m_gameOver) {
			m_gameOverPanel.SetActive (false);
		}

		if (m_pausePanel) {
			m_pausePanel.SetActive (false);
		}
	}

	void LandShape ()
	{

		m_timeToNextKeyLeftRight = Time.time;
		m_timeToNextKeyDown = Time.time;
		m_timeToNextKeyRotate = Time.time;

		m_activeShape.MoveUp ();
		m_gameBoard.StoreShapeInGrid (m_activeShape);
		m_activeShape = m_spawner.SpawnShape ();
		m_settleTime = 1;

		m_gameBoard.ClearAllRows ();

		PlaySound (m_soundManager.m_dropSound, 0.3f);
		if (m_gameBoard.m_completedRows > 0) {
			m_scoreManager.ScoreLines (m_gameBoard.m_completedRows);
			if (m_gameBoard.m_completedRows == 4){
				PlaySound (m_soundManager.m_clearRowSounds [2]);
			} else if (m_gameBoard.m_completedRows > 1) {
				PlaySound (m_soundManager.m_clearRowSounds [1]);
			} else {
				PlaySound (m_soundManager.m_clearRowSounds [0]);
			}

			//PlaySound (m_soundManager.m_clearRowSound);
		}

		holdCount = 0;
	}

	void PlaySound (AudioClip clip, float volume = 1)
	{
		if (m_soundManager.m_fxEnabled && clip) {
			m_soundManager.PlayFX (clip, volume);
		//	AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volume, 0.05f, 1f));
		}
	}

	void Settle ()
	{
		m_activeShape.MoveDown ();
		if (!m_gameBoard.isValidPosition (m_activeShape)) {
			m_settleTime = 1;
		}
		m_activeShape.MoveUp ();
	}

	void SettleFloor ()
	{
		m_activeShape.MoveUp ();
		if (!m_gameBoard.isValidPosition (m_activeShape)) {
			m_activeShape.MoveUp ();
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				m_activeShape.MoveUp ();
				if (!m_gameBoard.isValidPosition (m_activeShape)) {
					m_activeShape.MoveDown ();
					m_activeShape.MoveDown ();
					m_activeShape.MoveDown ();
				}
				else {
					Settle ();
					flag = true;
				}
			}
			else {
				Settle ();
				flag = true;
			}
		}
		else {
			Settle ();
			flag = true;
		}
		if (!flag) {
			m_activeShape.RotateLeft ();
			PlaySound (m_soundManager.m_errorSound, 0.1f);
		}
		// Flag for when block is settled
		flag = false;
	}

	void PerformRotate (bool clockwiseDir)
	{
		if (clockwiseDir == true) {
			m_activeShape.RotateRight ();
			m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				if (m_activeShape.transform.position.x <= 1) {
					m_activeShape.MoveRight ();
					if (!m_gameBoard.isValidPosition (m_activeShape)) {
						m_activeShape.MoveLeft ();
						m_activeShape.RotateLeft ();
						PlaySound (m_soundManager.m_errorSound, 0.1f);
					}
					else {
						Settle ();
						PlaySound (m_soundManager.m_dropSound, 0.3f);
					}
				}
				else
					if (m_activeShape.transform.position.x >= 8) {
						m_activeShape.MoveLeft ();
						if (!m_gameBoard.isValidPosition (m_activeShape)) {
							m_activeShape.MoveRight ();
							m_activeShape.RotateLeft ();
							PlaySound (m_soundManager.m_errorSound, 0.1f);
						}
						else {
							Settle ();
							PlaySound (m_soundManager.m_dropSound, 0.3f);
						}
					}
					else {
						SettleFloor ();
					}
			}
			else {
				Settle ();
				PlaySound (m_soundManager.m_dropSound, 0.3f);
			}
		} else {
			m_activeShape.RotateLeft ();
			m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				if (m_activeShape.transform.position.x <= 1) {
					m_activeShape.MoveRight ();
					if (!m_gameBoard.isValidPosition (m_activeShape)) {
						m_activeShape.MoveLeft ();
						m_activeShape.RotateRight ();
						PlaySound (m_soundManager.m_errorSound, 0.1f);
					}
					else {
						Settle ();
						PlaySound (m_soundManager.m_dropSound, 0.3f);
					}
				}
				else
					if (m_activeShape.transform.position.x >= 8) {
						m_activeShape.MoveLeft ();
						if (!m_gameBoard.isValidPosition (m_activeShape)) {
							m_activeShape.MoveRight ();
							m_activeShape.RotateRight ();
							PlaySound (m_soundManager.m_errorSound, 0.1f);
						}
						else {
							Settle ();
							PlaySound (m_soundManager.m_dropSound, 0.3f);
						}
					}
					else {
						SettleFloor ();
					}
			}
			else {
				Settle ();
				PlaySound (m_soundManager.m_dropSound, 0.3f);
			}

		}

	}

	void PlayerInput ()
	{
		if (Input.GetButton ("MoveRight") && Time.time > m_timeToNextKeyLeftRight) {
			m_activeShape.MoveRight ();
			m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				PlaySound (m_soundManager.m_errorSound, 0.1f);
				m_activeShape.MoveLeft ();
			} else {
				Settle ();
				PlaySound (m_soundManager.m_moveSound, 1f);
			}
		} else if (Input.GetButton ("MoveLeft") && Time.time > m_timeToNextKeyLeftRight) {
			m_activeShape.MoveLeft ();
			m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				PlaySound (m_soundManager.m_errorSound, 0.1f);
				m_activeShape.MoveRight ();
			} else {
				Settle ();
				PlaySound (m_soundManager.m_moveSound, 1f);
			}
		} else if ((Input.GetButtonDown ("Rotate") || Input.GetButtonDown("RotateAlt")) && Time.time > m_timeToNextKeyRotate) {
			if (m_activeShape.m_canRotate) {
				PerformRotate (m_clockwise);
			}

		}

		if (Input.GetButtonDown ("Drop")) {
			m_settleTime = 0;
			bool reachedBottom = false;
			while (!reachedBottom) {
				m_activeShape.MoveDown ();
				if (!m_gameBoard.isValidPosition (m_activeShape)) {
					m_activeShape.MoveUp ();
					reachedBottom = true;
				}
			}
			reachedBottom = false; // reset
		}

		if (Input.GetButton ("MoveDown") && (Time.time > m_timeToNextKeyDown) || (Time.time > m_timeToDrop)) {
			m_timeToDrop = Time.time + m_dropInterval - Mathf.Clamp(m_scoreManager.m_level * 0.01f, 0.01f, 0.08f);
			m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

			m_activeShape.MoveDown ();
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				if (m_gameBoard.IsOverLimit (m_activeShape)) {
					m_activeShape.MoveUp ();
					m_gameOver = true;
					Debug.LogWarning (m_activeShape.name + " is over limit");
					if (m_gameOverPanel) {
						m_gameOverPanel.SetActive (true);
					}
					PlaySound (m_soundManager.m_gameOverSound, 0.5f);
				} else {
					if (m_settleTime <= 0) {
						LandShape ();
					}
					m_activeShape.MoveUp ();
					m_settleTime -= (1 - m_settleTimeDelay);
				}
			}
		}



		if (Input.GetButtonDown ("RotateDirection")) {
			ToggleRotateDirection ();
		} else if (Input.GetButtonDown ("Pause")) {
			TogglePause ();
		} else if (Input.GetButtonDown ("Hold")) {
			Hold ();
		}
		// if we don't have a spawner or gameBoard just don't run the game
		if (!m_gameBoard || !m_spawner) {
			return;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver || !m_soundManager || !m_scoreManager) {
			return;
		}

		PlayerInput ();

	}

	public void Restart() {
		Debug.Log ("Restarted");
		Application.LoadLevel (Application.loadedLevel);
		TogglePause ();
		PlaySound (m_soundManager.m_restartClip, 0.2f);
	}

	public void ToggleRotateDirection()
	{
		m_clockwise = !m_clockwise;

		if (m_rotateIconToggle) {
			m_rotateIconToggle.ToggleIcon(m_clockwise);
		}
	}

	public void TogglePause() 
	{
		if (m_gameOver) {
			return;
		}

		m_isPaused = !m_isPaused;

		if (m_pausePanel) {
			m_pausePanel.SetActive (m_isPaused);

			if (m_soundManager) {
				m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume * 0.35f : m_soundManager.m_musicVolume;
			}

			Time.timeScale = (m_isPaused) ? 0 : 1;
		}

	}

	public void Hold() 
	{
		if (!m_holder) {
			return;
		}

		if (holdCount < 1) {
			if (!m_holder.m_heldShape) {
				m_holder.Swap (m_activeShape);
				m_activeShape = m_spawner.SpawnShape ();
				holdCount++;
			} 
			else 
			{
				m_activeShape = m_holder.Swap (m_activeShape);
				m_spawner.SpawnShape (m_activeShape);
				holdCount++;
			}
		}


			

	}
}
