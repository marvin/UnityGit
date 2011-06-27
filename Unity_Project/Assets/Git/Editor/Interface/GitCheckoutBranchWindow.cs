using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class GitCheckoutBranchWindow : EditorWindow {
	public static GitCheckoutBranchWindow Instance { get; private set; }

	int selection = 0;
	string currentBranch = "";
	string[] branches = null;


	public static void Init () {
		// Get existing open window or if none, make a new one:
		Instance = EditorWindow.GetWindow<GitCheckoutBranchWindow>(true, "Git Checkout Branch");

		Instance.currentBranch = GitSystem.GetCurrentBranch();
		Instance.branches = GitSystem.GetBranchList();

		for ( int i = 0; i < Instance.branches.Length; i++ ) {
			if ( Instance.branches[i] == Instance.currentBranch ) {
				Instance.selection = i;
				break;
			}
		}
	}


	void OnGUI() {
		bool currentBranchSelected = false;
		Color defaultColor = GUI.contentColor;

		selection = EditorGUILayout.Popup(selection, branches);

		if ( branches[selection] == currentBranch ) {
			currentBranchSelected = true;

			GUI.contentColor = Color.yellow;
			GUILayout.Label("You have the current branch selected...");
			GUI.contentColor = defaultColor;
		}
		else
			GUILayout.Label("");

		if ( GUILayout.Button("Checkout Branch", GUILayout.MaxWidth(125)) )
		{
			if ( !currentBranchSelected ) {
				GitSystem.CheckoutBranch(branches[selection]);
				Debug.Log("Current branch: " + GitSystem.GetCurrentBranch() + "\n");
			}

			Close();
		}
	}
}