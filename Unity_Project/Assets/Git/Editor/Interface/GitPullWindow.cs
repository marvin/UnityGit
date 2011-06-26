using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class GitPullWindow : EditorWindow {
	public static GitPullWindow Instance { get; private set; }

	int remoteSelection = 0;
	string[] remotes;

	[MenuItem ("Git/Pull")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		Instance = EditorWindow.GetWindow<GitPullWindow>(true, "Git Push");

		Instance.remotes = GitSystem.GetRemotesList();

		for ( int i = 0; i < Instance.remotes.Length; i++ ) {
			if ( Instance.remotes[i] == GitSystem.currentRemote ) {
				Instance.remoteSelection = i;
			}
		}
	}


	void OnGUI() {
		remoteSelection = EditorGUILayout.Popup(remoteSelection, remotes);
		GitSystem.currentRemote = remotes[remoteSelection];

		if ( GUILayout.Button("Pull", GUILayout.MaxWidth(100)) ) {
			GitSystem.Pull(remotes[remoteSelection]);
			Close();
		}
	}
}