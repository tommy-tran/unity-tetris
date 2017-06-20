using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	// our library of shapes, make sure you don't leave any blanks in the Inspector
	public Shape[] m_allShapes;
	public Transform[] m_queueTransforms = new Transform[3];

	Shape[] m_queuedShapes = new Shape[3];

	float m_queueScale = 0.5f;


	void Start() 
	{
		initQueue (); 
	}

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
		Shape shape = GetShape ();
		shape.transform.position = transform.position;
		shape.transform.localScale = Vector3.one;
		// shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;

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

	void initQueue() 
	{
		for (int i = 0; i < m_queuedShapes.Length; i++)
		{
			m_queuedShapes [i] = null;
		}
		FillQueue ();

	}

	void FillQueue() 
	{
		for (int i = 0; i < m_queuedShapes.Length; i++) 
		{
			if (!m_queuedShapes [i]) 
			{
				m_queuedShapes [i] = Instantiate (GetRandomShape (), transform.position, Quaternion.identity) as Shape;
				m_queuedShapes [i].transform.position = m_queueTransforms [i].position; // takes position of precreated queueTransforms
				m_queuedShapes[i].transform.localScale = new Vector3(m_queueScale, m_queueScale, m_queueScale);
			}
		}
	}

	Shape GetShape() 
	{
		Shape firstShape = null;
		if (m_queuedShapes [0]) 
		{
			firstShape = m_queuedShapes [0];
		}

		for (int i = 1; i < m_queuedShapes.Length; i++) 
		{
			m_queuedShapes [i - 1] = m_queuedShapes [i];
			m_queuedShapes [i - 1].transform.position = m_queueTransforms [i - 1].position;
		}

		m_queuedShapes [m_queuedShapes.Length - 1] = null;

		FillQueue ();

		return firstShape;
	}

}
