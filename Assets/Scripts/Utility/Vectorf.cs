using UnityEngine;
using System.Collections;

// static class with helper functions for vectors
public static class Vectorf
{
	// a static method for returning a rounded Vector2
	public static Vector2 Round(Vector2 v)
	{
		return new Vector2(Mathf.Round(v.x),Mathf.Round(v.y));
	}

	// a static method for returning a rounded Vector3
	public static Vector3 Round(Vector3 v) 
	{
		return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
	}

}
