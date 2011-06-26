using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GitDeleteBranchWindow : EditorWindow {
	public static GitDeleteBranchWindow Instance { get; private set; }

	int selection = 0;
	bool deleteOnlyIfMerged = true;
	string[] branches = null;


	public static void Init () {
		// Get existing open window or if none, make a new one:
		Instance = EditorWindow.GetWindow<GitDeleteBranchWindow>(true, "Git Delete Branch");

		Instance.branches = GitSystem.GetBranchList(false);
	}


	void OnGUI() {
		if ( branches.Length > 0 ) {
			selection = EditorGUILayout.Popup(selection, branches);

			if ( GUILayout.Button("Delete Branch", GUILayout.MaxWidth(100)) ) {
				GitSystem.DeleteBranch(branches[selection], deleteOnlyIfMerged);
				Close();
			}
		}
		else
			GUILayout.Label("No existing branches to delete...");
	}


	static void BranchTestFunc ()
	{
/*		
		string[] branches = RemoveEmptyListEntries (RunGitCmd ("branch"));
		
		foreach (string branch in branches) {
			if (!branch.Contains ("*")) {
				string result = RunGitCmd ("checkout" + branch);
				
				if (result.ToLower ().Contains ("aborting")) {
					Debug.LogError ("Branch switching has been aborted.  Make sure you commit or stash your changes before checking out another branch.");
					return;
				}
				
				break;
			}
		}
		
		Debug.Log (RunGitCmd ("branch"));
*/
	}
}