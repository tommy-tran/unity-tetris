using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	// our library of shapes, make sure you don't leave any blanks in the Inspector
	public Shape[] m_allShapes;

	// returns a random shape from our library of shapes
	Shape GetRandomShape()
	{
		int i = Random.Range(0,m_allShapes.Length);
		if (m_allShapes[i])
		{
			return m_allShapes[i];
		}
		else
		{
			Debug.LogWarning("WARNING! Invalid shape in spawner!");
			return null;
		}

	}

	// instantiates a shape at the spawner's position
	public Shape SpawnShape()
	{
		Shape shape = null;
		shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;

		if (shape)
		{
			return shape;
		}
		else
		{
			Debug.LogWarning("WARNING! Invalid shape in spawner!");
			return null;
		}
	}

}
