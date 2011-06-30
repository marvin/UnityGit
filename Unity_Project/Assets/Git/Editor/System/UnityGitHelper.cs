using UnityEngine;
using UnityEditor;

using System.Collections;

public class UnityGitHelper : MonoBehaviour {
	public static void CleanupUntracked()
	{
		Debug.Log(GitSystem.RunGitCmd("clean -d -f"));

		UnityGitHelper.CleanupUntracked(GitSystem.GetUntrackedFilesList(false));
	}


	static void CleanupUntracked(string[] untrackedFiles)
	{
		foreach ( string path in untrackedFiles)
		{
			Debug.Log(path);
			AssetDatabase.DeleteAsset(path);
		}

		AssetDatabase.Refresh();
	}


	// Simple comment for testing...
}