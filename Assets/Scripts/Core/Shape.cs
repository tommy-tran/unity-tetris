using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {

	// turn this property off if you don't want the shape to rotate (Shape O)
	public bool m_canRotate = true;

	public Vector3 m_queueOffset;

	// general move method
	void Move(Vector3 moveDirection)
	{
		transform.position += moveDirection;
	}


	//public methods for moving left, right, up and down, respectively
	public void MoveLeft()
	{
		Move(new Vector3(-1, 0, 0));
	}

	public void MoveRight()
	{
		Move(new Vector3(1, 0, 0));
	}

	public void MoveUp()
	{
		Move(new Vector3(0, 1, 0));
	}

	public void MoveDown()
	{
		Move(new Vector3(0, -1, 0));
	}


	//public methods for rotating right and left
	public void RotateRight()
	{
		if (m_canRotate)
			transform.Rotate(0, 0, -90);
	}
	public void RotateLeft()
	{
		if (m_canRotate)
			transform.Rotate(0, 0, 90);
	}

	public void RotateClockwise(bool clockwise) 
	{
		if (clockwise) {
			RotateRight ();
		} else {
			RotateLeft ();
		}
	}
		
}
