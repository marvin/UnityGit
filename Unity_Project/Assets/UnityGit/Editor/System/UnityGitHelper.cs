using UnityEngine;
using UnityEditor;

using System.IO;
using System.Collections;

public class UnityGitHelper : MonoBehaviour
{
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


	public static void CreateUnityGitIgnores()
	{
		string libraryPath = Application.dataPath + "../Library/.gitignore";
		string projectPath = Application.dataPath + "../.gitignore";
		string libraryContents = "/*\n!/.gitignore\n!/EditorBuildSettings.asset\n!/InputManager.asset\n!/ProjectSettings.asset\n!/QualitySettings.asset\n!/TagManager.asset\n!/TimeManager.asset\n!/AudioManager.asset\n!/DynamicsManager.asset\n!/NetworkManager.asset";
		string projectContents = "Temp\n*.csproj\n*.pidb\n*.sln\n*.userprefs";

		Debug.Log(libraryPath);
		Debug.Log(projectPath);
		Debug.Log(libraryContents);
		Debug.Log(projectContents);
	}
}