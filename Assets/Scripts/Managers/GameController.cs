﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// reference to our board
	Board m_gameBoard;

	// reference to our spawner 
	Spawner m_spawner;

	// active
	Shape m_activeShape;

	public float m_dropInterval = 0.7f;
	float m_timeToDrop;
	float m_timeToNextKey;


	[Range(0.02f, 1f)]
	public float m_keyRepeatRateLeftRight = 0.15f;
	float m_timeToNextKeyLeftRight;


	[Range(0.01f, 1f)]
	public float m_keyRepeatRateDown = 0.01f;
	float m_timeToNextKeyDown;

	[Range(0.02f, 1f)]
	public float m_keyRepeatRateRotate = 0.02f;
	float m_timeToNextKeyRotate;


	// Use this for initialization
	void Start () 
	{
		
		// find spawner and board with GameObject.FindWithTag plus GetComponent; make sure you tag your objects correctly
		//m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
		//m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

		// find spawner and board with generic version of GameObject.FindObjectOfType, slower but less typing
		m_gameBoard = GameObject.FindObjectOfType<Board>();
		m_spawner = GameObject.FindObjectOfType<Spawner>();

		m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
		m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
		m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
			
		if (!m_gameBoard)
		{
			Debug.LogWarning("WARNING!  There is no game board defined!");
		}

		if (!m_spawner) {
			Debug.LogWarning ("WARNING!  There is no spawner defined!");
		} else {
			m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
			if (!m_activeShape) {
				m_activeShape = m_spawner.SpawnShape ();
			}
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

		m_gameBoard.ClearAllRows ();
	}

	void PlayerInput ()
	{
		if (Input.GetButton ("MoveRight") && Time.time > m_timeToNextKeyLeftRight) {
			m_activeShape.MoveRight ();
			m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				m_activeShape.MoveLeft ();
			}
		}
		else if (Input.GetButton ("MoveLeft") && Time.time > m_timeToNextKeyLeftRight) {
			m_activeShape.MoveLeft ();
			m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				m_activeShape.MoveRight ();
			}
		}
		else if (Input.GetButtonDown ("Rotate") && Time.time > m_timeToNextKeyRotate) {
			m_activeShape.RotateRight ();
			m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				m_activeShape.RotateLeft ();
			}
		}

		if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || (Time.time > m_timeToDrop)) {
			
			m_timeToDrop = Time.time + m_dropInterval;
			m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

			m_activeShape.MoveDown ();
			if (!m_gameBoard.isValidPosition (m_activeShape)) {
				LandShape ();
			}

		}
		// if we don't have a spawner or gameBoard just don't run the game
		if (!m_gameBoard || !m_spawner) {
			return;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!m_gameBoard || !m_spawner || !m_activeShape) {
			return;
		}

		PlayerInput ();

	}
}
