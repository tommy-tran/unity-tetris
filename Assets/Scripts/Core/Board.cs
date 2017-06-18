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

	bool isWithinBoard(int x, int y) {
		return (x >= 0 && x < m_width && y >= 0);
	}

	bool isOccupied(int x, int y, Shape shape)
	{
		return (m_grid [x, y] != null && m_grid [x, y].parent != shape.transform);
	}


	public bool isValidPosition(Shape shape)
	{
		foreach (Transform child in shape.transform) {
			Vector2 pos = Vectorf.Round (child.position);

			if (!isWithinBoard ((int)pos.x, (int)pos.y)) {
				return false;
			}

			if (isOccupied ((int)pos.x, (int)pos.y, shape)) {
				return false;
			}
		}

		return true;
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

	public void StoreShapeInGrid(Shape shape) {
		if (shape == null) {
			return;
		}

		foreach (Transform child in shape.transform) 
		{
			Vector2 pos = Vectorf.Round (child.position);
			m_grid [(int)pos.x, (int)pos.y] = child;
		}
	}

	bool IsComplete(int y) 
	{
		for (int x = 0; x < m_width; ++x) 
		{
			if (m_grid [x, y] == null) {
				return false;
			}
		}
		return true;
	}

	void ClearRow(int y) 
	{
		for (int x = 0; x < m_width; ++x) {
			if (m_grid [x, y] != null) {
				Destroy (m_grid [x, y].gameObject);
			}
			m_grid [x, y] = null;
		}
	}

	void ShiftOneRowDown(int y) {
		for (int x = 0; x < m_width; ++x) {
			if (m_grid [x, y] != null) {
				m_grid [x, y - 1] = m_grid [x, y];
				m_grid [x, y] = null;
				m_grid [x, y - 1].position += new Vector3 (0, -1, 0);
			}
		}
	}

	void ShiftRowsDown(int startY) {
		for (int i = startY; i < m_height; ++i) {
			ShiftOneRowDown (i);
		}
	}

	public void ClearAllRows()
	{
		for (int y = 0; y < m_height; ++y) {
			if (IsComplete (y)) {
				ClearRow (y);
				ShiftRowsDown (y + 1);
				y--;
			}
		}

	}

}
