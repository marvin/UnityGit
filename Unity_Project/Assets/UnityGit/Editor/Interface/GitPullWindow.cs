using UnityEngine;
using UnityEditor;

using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class GitPullWindow : EditorWindow
{
	public static GitPullWindow Instance { get; private set; }

	int remoteSelection = 0;
	string[] remotes;

	bool progressMode = false;
	bool doPostPull = false;
	string progressString = "";

	public static void Init ()
	{
		// Get existing open window or if none, make a new one:
		Instance = EditorWindow.GetWindow<GitPullWindow>(true, "Git Push");

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
		if ( progressMode )
		{
			GUILayout.Label(progressString);

			if ( doPostPull )
			{
				doPostPull = false;
				GitSystem.PostPull();
			}
		}
		else
		{
			remoteSelection = EditorGUILayout.Popup(remoteSelection, remotes);
			GitSystem.currentRemote = remotes[remoteSelection];

			if ( GUILayout.Button("Pull", GUILayout.MaxWidth(100)) )
			{
				GitSystem.Pull(remotes[remoteSelection], ProgressReceiver);
				progressMode = true;
			}
		}
	}


	void ProgressReceiver(string progressUpdate, bool isDone)
	{
		progressString += progressUpdate;

		if ( isDone )
			doPostPull = true;
	}
}