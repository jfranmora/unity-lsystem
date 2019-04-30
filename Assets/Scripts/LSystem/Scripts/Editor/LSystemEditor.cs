using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LSystem))]
public class LSystemEditor : Editor
{
	private LSystem dat;

	private void OnEnable()
	{
		dat = target as LSystem;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying)
		{
			EditorGUILayout.Space();

			DrawButtons();

			EditorGUILayout.Space();

			DrawLayoutButton();
		}
	}

	private void DrawButtons()
	{
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Generate L-System"))
		{
			dat.DoGenerateLSystem();
		}

		if (GUILayout.Button("Random generate L-System"))
		{
			dat.DoRandomGenerateLSystem();
		}

		EditorGUILayout.EndHorizontal();
	}

	private void DrawLayoutButton()
	{
		// BEGIN EXAMPLES

		EditorGUILayout.LabelField("Examples", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("1"))
		{
			dat.SetupExample1();
		}

		EditorGUILayout.EndHorizontal();

		// END EXAMPLES

		// BEGIN TREES

		EditorGUILayout.LabelField("Trees", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("1"))
		{
			dat.SetupTreeDemo1();
		}

		if (GUILayout.Button("2"))
		{
			dat.SetupTreeDemo2();
		}

		if (GUILayout.Button("3"))
		{
			dat.SetupTreeDemo3();
		}

		if (GUILayout.Button("4"))
		{
			dat.SetupTreeDemo4();
		}

		if (GUILayout.Button("5"))
		{
			dat.SetupTreeDemo5();
		}

		if (GUILayout.Button("6"))
		{
			dat.SetupTreeDemo6();
		}

		EditorGUILayout.EndHorizontal();

		// END TREES
	}
}
