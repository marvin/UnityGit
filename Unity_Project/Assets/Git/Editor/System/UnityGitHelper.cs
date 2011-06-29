using UnityEngine;
using System.Collections;

public class UnityGitHelper : MonoBehaviour {
	public static void UnityCleanupUntracked(string[] untrackedFiles)
	{
		foreach ( string path in untrackedFiles)
		{
			Debug.Log(path);
		}
	}
}