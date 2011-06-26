using UnityEngine;
using UnityEditor;

using System.Collections;

public class GitMenuCommands : MonoBehaviour {
	[MenuItem ("Git/Commit")]
	static void GitCommit() {
		GitCommitWindow.Init();
	}


	[MenuItem("Git/Commit All")]
	static void CommitAll() {
		GitSystem.CommitAll();
	}


	[MenuItem ("Git/Pull")]
	static void GitPull() {
		GitPullWindow.Init();
	}


	[MenuItem ("Git/Push")]
	static void GitPush() {
		GitPushWindow.Init();
	}


	[MenuItem ("Git/Resolve Conflicts (Unmerged)")]
	static void GitResolveConflicts() {
		GitConflictsWindow.Init();
	}


	[MenuItem ("Git/Branching/Create Branch")]
	static void GitCreateBranch() {
		GitCreateBranchWindow.Init();
	}


	[MenuItem ("Git/Branching/Delete Branch")]
	static void GitDeleteBranch() {
		GitDeleteBranchWindow.Init();
	}


	[MenuItem ("Git/Branching/Checkout Branch")]
	static void GitCheckoutBranch() {
		GitCheckoutBranchWindow.Init();
	}
}