using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class GitValueSettingWindow : EditorWindow
{
	public static GitValueSettingWindow Instance { get; private set; }

	string label = "";
	string command = "";
	string configValue = "";
	bool isGlobal = false;


	public static void Init (string label, string command)
	{
		// Get existing open window or if none, make a new one:
		Instance = EditorWindow.GetWindow<GitValueSettingWindow>(true, label);

		Instance.label = label;
		Instance.command = command;
	}


	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(label + ": ");
		configValue = EditorGUILayout.TextField(configValue);
		EditorGUILayout.EndHorizontal();

		isGlobal = GUILayout.Toggle(isGlobal, "Global");

		if ( GUILayout.Button("Okay", GUILayout.MaxWidth(100)) )
		{
			GitSystem.RunGitCmd("config " + (isGlobal ? "--global " : "") + command + " " + configValue);
			Close();
		}
	}
}