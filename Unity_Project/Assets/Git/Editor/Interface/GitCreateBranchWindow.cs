using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GitCreateBranchWindow : EditorWindow {
	public static GitCreateBranchWindow Instance { get; private set; }

	string newBranchName = "";
	bool checkoutAfterCreation = true;
	string[] existingBranches = null;


	public static void Init () {
		// Get existing open window or if none, make a new one:
		Instance = EditorWindow.GetWindow<GitCreateBranchWindow>(true, "Git Create Branch");

		Instance.existingBranches = GitSystem.GetBranchList();
	}


	void OnGUI() {
		bool branchTaken = false;
		Color defaultColor = GUI.contentColor;

		GUILayout.Label("Enter a name for the new branch:");

		newBranchName = EditorGUILayout.TextField(newBranchName);

		checkoutAfterCreation = GUILayout.Toggle(checkoutAfterCreation, "Checkout");

		// Make sure we don't already have that branch
		foreach ( string branch in existingBranches ) {
			if ( branch == newBranchName ) {
				branchTaken = true;
				break;
			}
		}

		GUI.contentColor = Color.red;

		if ( branchTaken )
			GUILayout.Label("You already have a branch with that name!");
		else
			GUILayout.Label("");

		GUI.contentColor = defaultColor;

		if ( GUILayout.Button("Create Branch", GUILayout.MaxWidth(100)) ) {
			if ( !branchTaken ) {
				GitSystem.CreateBranch(Regex.Replace(newBranchName, @"\s", "_"), checkoutAfterCreation);
				Close();
			}
		}
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