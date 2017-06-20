using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour {

	// our starting alpha value
	public float m_startAlpha = 1f;

	// our ending alpha value
	public float m_targetAlpha = 0f;

	// how long in seconds before we start the fade effect
	public float m_delay = 0f;

	// how long it takes to change from start to target alpha values
	public float m_timeToFade = 1f;

	// increment applied to our alpha per frame
	float m_inc;

	// the current value of the alpha
	float m_currentAlpha;

	// reference to our MaskableGraphic component
	MaskableGraphic m_graphic;

	// the graphic's original color
	Color m_originalColor;

	// Use this for initialization
	void Start () {

		// cache the Maskable Graphic
		m_graphic = GetComponent<MaskableGraphic>();

		// cache our graphic's original color
		m_originalColor = m_graphic.color;

		// set our current alpha to the starting value
		m_currentAlpha = m_startAlpha;

		// set the color of the graphic, based on the original rgb and current alpha value
		Color tempColor = new Color (m_originalColor.r, m_originalColor.g, m_originalColor.b, m_currentAlpha);
		m_graphic.color = tempColor;

		// calculate how much we increment the alpha based on our transition time; this rate is per FRAME, note Time.deltaTime
		m_inc = ((m_targetAlpha - m_startAlpha)/ m_timeToFade) * Time.deltaTime;

		// start our coroutine
		StartCoroutine (FadeRoutine());
	
		// alternate way of starting coroutine
		//StartCoroutine("FadeRoutine");
	}
	
	IEnumerator FadeRoutine()
	{
		// wait to begin the fade effect
		yield return new WaitForSeconds(m_delay);

		// 
		while (Mathf.Abs(m_targetAlpha - m_currentAlpha) > 0.01f)
		{
			yield return new WaitForEndOfFrame();

			// increment our alpha value
			m_currentAlpha = m_currentAlpha + m_inc;

			// set the color of the graphic, based on the original rgb and current alpha value
			Color tempColor = new Color (m_originalColor.r, m_originalColor.g, m_originalColor.b, m_currentAlpha);
			m_graphic.color = tempColor;


		}

		// 
		//Debug.Log("SCREEN FADER finished!");





	}

}













/*
		while (Mathf.Abs(m_targetAlpha - m_startAlpha) > 0.01f)
		{
			yield return new WaitForEndOfFrame();

			m_currentAlpha = m_currentAlpha + m_inc;

			Color tempColor = new Color (m_originalColor.r, m_originalColor.g, m_originalColor.b, m_currentAlpha);

			m_graphic.color = tempColor;

		}
		*/
