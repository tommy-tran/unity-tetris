using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {

	public Transform m_holderTransform;
	public Shape m_heldShape = null;
	float m_scale = 0.5f;

	public void Catch(Shape shape) 
	{
		if (m_heldShape) 
		{
			Debug.LogWarning ("HOLDER WARNING! Release a shape before you try to hold!");
			return
		}

		if (!shape) 
		{
			Debug.LogWarning ("HOLDER WARNING! Invalid Shape!");
			return
		}

		if (m_holderTransform) 
		{


		}
	}
}
