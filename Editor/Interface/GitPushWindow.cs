using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class GitPushWindow : EditorWindow
{
	public static GitPushWindow Instance { get; private set; }

	int remoteSelection = 0;
	string[] remotes;

	public static void Init ()
	{
		// Get existing open window or if none, make a new one:
		Instance = EditorWindow.GetWindow<GitPushWindow>(true, "Git Push");

		Instance.remotes = GitSystem.GetRemotesList();

		for ( int i = 0; i < Instance.remotes.Length; i++ )
		{
			if ( Instance.remotes[i] == GitSystem.currentRemote )
			{
				Instance.remoteSelection = i;
			}
		}
	}


	void OnGUI()
	{
		remoteSelection = EditorGUILayout.Popup(remoteSelection, remotes);
		GitSystem.currentRemote = remotes[remoteSelection];

		if ( GUILayout.Button("Push", GUILayout.MaxWidth(100)) )
		{
			GitSystem.Push(remotes[remoteSelection]);
		}
	}
}