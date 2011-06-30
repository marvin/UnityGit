using UnityEngine;
using UnityEditor;

using System.Collections;

public class GitMenuCommands : MonoBehaviour
{
	[MenuItem("Git/Init New Repo")]
	static void InitNewRepo()
	{
		GitSystem.InitNewRepo();
	}


	[MenuItem ("Git/Commit %#c")]
	static void GitCommit()
	{
		GitCommitWindow.Init();
	}


	[MenuItem("Git/Quick Commit All %#&c")]
	static void CommitAll()
	{
		GitSystem.CommitAll();
	}


	[MenuItem ("Git/Pull %#&p")]
	static void GitPull()
	{
		GitPullWindow.Init();
	}


	[MenuItem ("Git/Push #&p")]
	static void GitPush()
	{
		GitPushWindow.Init();
	}


	[MenuItem ("Git/Fetch")]
	static void GitFetch()
	{
		GitFetchWindow.Init();
	}


	[MenuItem ("Git/Resolve Conflicts")]
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


	[MenuItem ("Git/Branching/Checkout")]
	static void GitCheckoutBranch()
	{
		GitCheckoutBranchWindow.Init();
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


	[MenuItem ("Git/Cleanup/Untracked")]
	static void CleanupUntracked()
	{
		UnityGitHelper.CleanupUntracked();
	}


	[MenuItem ("Git/Cleanup/Untracked And Ignored")]
	static void CleanupUntrackedAndIgnored()
	{
		Debug.LogWarning("This feature partially implemented, it only cleared untracked, but not ignored files...");
		UnityGitHelper.CleanupUntracked();
	}


	[MenuItem ("Git/Cleanup/Ignored")]
	static void CleanupIgnored()
	{
		Debug.Log(GitSystem.RunGitCmd("clean -d -X -f"));

		Debug.LogWarning("This feature not yet implemented...");
//		UnityGitHelper.UnityCleanupUntracked(GitSystem.GetUntrackedFilesList(false));
	}


	[MenuItem("Git/Test")]
	static void TestFunc ()
	{
		Debug.Log (GitSystem.RunGitCmd ("ls-files --modified --exclude-standard"));
	}


	/* **** Git Help **** */

	[MenuItem("Git/Help")]
	static void ShowGitHelp ()
	{
		GitSystem.RunGitCmd ("help git");
	}
}