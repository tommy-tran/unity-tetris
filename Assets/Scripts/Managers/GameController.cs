using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// reference to our board
	Board m_gameBoard;

	// reference to our spawner 
	Spawner m_spawner;

	// Use this for initialization
	void Start () 
	{
		
		// find spawner and board with GameObject.FindWithTag plus GetComponent; make sure you tag your objects correctly
		//m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
		//m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

		// find spawner and board with generic version of GameObject.FindObjectOfType, slower but less typing
		m_gameBoard = GameObject.FindObjectOfType<Board>();
		m_spawner = GameObject.FindObjectOfType<Spawner>();


		if (m_spawner)
		{
			m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
		}
			

		if (!m_gameBoard)
		{
			Debug.LogWarning("WARNING!  There is no game board defined!");
		}

		if (!m_spawner)
		{
			Debug.LogWarning("WARNING!  There is no spawner defined!");
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		// if we don't have a spawner or gameBoard just don't run the game
		if (!m_gameBoard || !m_spawner)
		{
			return;
		}

	}
}
