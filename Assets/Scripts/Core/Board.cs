using UnityEngine;
using System.Collections;


public class Board : MonoBehaviour {
	
	// a SpriteRenderer that will be instantiated in a grid to create our board
	public Transform m_emptySprite;

	// the height of the board
	public int m_height = 30;

	// width of the board
	public int m_width = 10;

	// number of rows where we won't have grid lines at the top
	public int m_header = 10;

	// store inactive shapes that have landed here
	Transform[,] m_grid;


	void Awake()
	{
		m_grid = new Transform[m_width,m_height];
	}


	// Use this for initialization

	void Start () {
		DrawEmptyCells();
	}
	
	// Update is called once per frame
	void Update () {

	}

	// draw our empty board with our empty sprite object
	void DrawEmptyCells() {
		if (m_emptySprite)
		{
			for (int y = 0; y < m_height - m_header; y++)
			{
				for (int x = 0; x < m_width; x++) 
				{
					Transform clone;
					clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;

					// names the empty squares for organizational purposes
					clone.name = "Board Space ( x = " + x.ToString() +  " , y =" + y.ToString() + " )"; 

					// parents all of the empty squares to the Board object
					clone.transform.parent = transform;
				}
			}
		}
	}


}
