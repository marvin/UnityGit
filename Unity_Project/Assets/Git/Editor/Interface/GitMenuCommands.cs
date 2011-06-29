using UnityEngine;
using UnityEditor;

using System.Collections;

public class GitMenuCommands : MonoBehaviour {
	[MenuItem ("Git/Commit")]
	static void GitCommit()
	{
		GitCommitWindow.Init();
	}


	[MenuItem("Git/Commit All")]
	static void CommitAll()
	{
		GitSystem.CommitAll();
	}


	[MenuItem ("Git/Pull")]
	static void GitPull()
	{
		GitPullWindow.Init();
	}


	[MenuItem ("Git/Push")]
	static void GitPush()
	{
		GitPushWindow.Init();
	}


	[MenuItem ("Git/Resolve Conflicts (Unmerged)")]
	static void GitResolveConflicts()
	{
		GitConflictsWindow.Init();
	}


	/* **** Branching **** */

	[MenuItem ("Git/Branching/Print Current")]
	static void GitCurrentBranch()
	{
		Debug.Log("Current branch: " + GitSystem.GetCurrentBranch() + "\n");
	}

	[MenuItem ("Git/Branching/Create")]
	static void GitCreateBranch()
	{
		GitCreateBranchWindow.Init();
	}


	[MenuItem ("Git/Branching/Merge")]
	static void GitMergeBranch()
	{
		GitMergeBranchWindow.Init();
	}


	[MenuItem ("Git/Branching/Delete")]
	static void GitDeleteBranch()
	{
		GitDeleteBranchWindow.Init();
	}


	[MenuItem ("Git/Branching/Checkout")]
	static void GitCheckoutBranch()
	{
		GitCheckoutBranchWindow.Init();
	}


	[MenuItem ("Git/Cleanup/Untracked")]
	static void CleanupUntracked()
	{
		Debug.Log(GitSystem.RunGitCmd("clean -d -f"));

		UnityGitHelper.UnityCleanupUntracked(GitSystem.GetUntrackedFilesList(false));
	}


	[MenuItem ("Git/Cleanup/Untracked And Ignored")]
	static void CleanupUntrackedAndIgnored()
	{
		Debug.Log(GitSystem.RunGitCmd("clean -d -x -f"));

		UnityGitHelper.UnityCleanupUntracked(GitSystem.GetUntrackedFilesList(false));
	}


	[MenuItem ("Git/Cleanup/Ignored")]
	static void CleanupIgnored()
	{
		Debug.Log(GitSystem.RunGitCmd("clean -d -X -f"));

//		UnityGitHelper.UnityCleanupUntracked(GitSystem.GetUntrackedFilesList(false));
	}


	[MenuItem("Git/Test")]
	static void TestFunc ()
	{
		Debug.Log (GitSystem.RunGitCmd ("ls-files --modified --exclude-standard"));
	}
}