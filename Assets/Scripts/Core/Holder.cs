using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {

	public Transform m_holderTransform;

	public Shape m_heldShape = null;
	Shape m_releasedShape;
	float m_scale = 0.5f;

	public Shape Swap(Shape shape) 
	{
		if (!shape) 
		{
			Debug.LogWarning ("HOLDER WARNING! Invalid Shape!");
			return null;
		}

		if (!m_holderTransform)
		{
			Debug.LogWarning ("HOLDER WARNING! Holder has no transform assigned");
			return null;
		}

		if (m_heldShape) {
			// Keeps shape
			m_releasedShape = m_heldShape;

			// Stores new shape
			Catch(shape);

			// Release
			m_releasedShape.transform.localScale = Vector3.one;
			return m_releasedShape;
		} 
		else 
		{
			Catch (shape);
			return null;
		}






	}

	public void Catch(Shape shape) 
	{
		shape.transform.position = m_holderTransform.position + shape.m_queueOffset;
		shape.transform.localScale = new Vector3 (m_scale, m_scale, m_scale);
		m_heldShape = shape;
	}

}
