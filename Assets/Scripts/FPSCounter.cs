using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{


	float deltaTime = 0.0f;

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperCenter;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color(1f, 1f, 1f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = (int)(1f / Time.unscaledDeltaTime);
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);

	}
}