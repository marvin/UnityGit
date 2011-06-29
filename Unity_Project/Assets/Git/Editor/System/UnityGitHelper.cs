using UnityEngine;
using UnityEditor;

using System.Collections;

public class UnityGitHelper : MonoBehaviour {
	public static void UnityCleanupUntracked(string[] untrackedFiles)
	{
		foreach ( string path in untrackedFiles)
		{
			Debug.Log(path);
			AssetDatabase.DeleteAsset(path);
		}
	}
}