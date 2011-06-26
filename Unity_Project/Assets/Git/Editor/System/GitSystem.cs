using UnityEngine;
using UnityEditor;

using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Debug = UnityEngine.Debug;

public class GitSystem : Editor
{
	public static string currentRemote = "";

	[MenuItem("Git/Init New Repo")]
	public static void InitNewRepo ()
	{
		string repoPath = GetRepoPath ();
		
		if (repoPath == "") {
			repoPath = EditorUtility.OpenFolderPanel ("Choose a repo directory", "", "");
			
			if (repoPath == "" || repoPath == null)
				return;
			
			repoPath = repoPath.Replace (@"\", "/");
			Debug.Log (repoPath);
		} else {
			Debug.Log ("Repo already exists at: " + repoPath);
			return;
		}
		
		Debug.Log (RunGitCmd ("init " + repoPath));
	}


	[MenuItem("Git/Test")]
	static void TestFunc ()
	{
		Debug.Log (RunGitCmd ("ls-files --modified --exclude-standard"));
	}


	[MenuItem("Git/Branch Test")]
	static void BranchTestFunc ()
	{
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
	}


	static string GetRepoPath ()
	{
		string[] locationParts = Application.dataPath.Split ('/');
		
		for (int o = 0; o < locationParts.Length; o++) {
			string tryPath = "";
			
			for (int i = 0; i < locationParts.Length - o; i++) {
				tryPath += locationParts[i] + "/";
			}
			
			if (Directory.Exists (tryPath + ".git")) {
				tryPath = tryPath.Remove(tryPath.Length-1);
				return tryPath.Replace (@"\", @"/");
			}
		}
		
		return "";
	}


	[MenuItem("Git/Commit All")]
	static void CommitAll ()
	{
		string[] modifiedFiles = GetModifiedFilesList ();
		string[] untrackedFiles = GetUntrackedFilesList ();
		string[] deletedFiles = GetDeletedFilesList ();
		
		foreach (string path in modifiedFiles) {
			Debug.Log (RunGitCmd ("add \"" + path + "\""));
		}
		
		foreach (string path in untrackedFiles) {
			Debug.Log (RunGitCmd ("add \"" + path + "\""));
		}
		
		foreach (string path in deletedFiles) {
			Debug.Log (RunGitCmd ("rm \"" + path + "\""));
		}
		
		Debug.LogWarning (RunGitCmd ("commit -m \"Commit from Unity!\""));
	}


	/* **** Commit **** */

	public static void Commit(string commitMessage, string[] addFiles, string[] removeFiles) {
		foreach (string path in addFiles)
			RunGitCmd ("add \"" + path + "\"");

		foreach (string path in removeFiles)
			RunGitCmd ("rm \"" + path + "\"");

		Debug.LogWarning (RunGitCmd ("commit -m \"" + commitMessage + "\""));
	}


	/* **** Push **** */

	public static void Push(string remoteName) {
		Debug.Log(RunGitCmd("push " + remoteName));
	}


	/* **** Pull **** */

	public static void Pull(string remoteName) {
		string feedback = RunGitCmd("pull -v --progress \"" + remoteName + "\" " + GetCurrentBranch());
		string[] unmergedFiles;

		if ( feedback.Contains("Aborting") ) {
			Debug.LogError(feedback);
			Debug.LogError("Error pulling!");
		}

		unmergedFiles = GetUnmergedFilesList();

		if ( unmergedFiles.Length > 0 )
			GitConflictsWindow.Init(unmergedFiles);
	}


	public static string GetCurrentBranch() {
		string[] branches = RemoveEmptyListEntries (RunGitCmd ("branch"));
		
		foreach (string branch in branches) {
			if (branch.Contains ("*")) {
				return branch.Replace("*", "");
			}
		}

		return "";
	}


	/* **** GetModifiedFilesList **** */

	public static string[] GetModifiedFilesList ()
	{
		string filesString = RunGitCmd ("ls-files --modified --exclude-standard");
		string[] filesList = RemoveEmptyListEntries(filesString);
		
		return FilterUsingSelection(filesList);
	}


	/* **** GetUntrackedFilesList **** */

	public static string[] GetUntrackedFilesList ()
	{
		string filesString = RunGitCmd ("ls-files --other --exclude-standard");
		string[] filesList = RemoveEmptyListEntries(filesString);
		
		return FilterUsingSelection(filesList);
	}


	/* **** GetDeletedFilesList **** */

	public static string[] GetDeletedFilesList ()
	{
		string filesString = RunGitCmd ("ls-files --deleted --exclude-standard");
		string[] filesList = RemoveEmptyListEntries(filesString);
		
		return FilterUsingSelection(filesList);
	}

	public static string[] GetUnmergedFilesList () {
		string filesString = RunGitCmd ("ls-files --unmerged --exclude-standard");
		string[] filesList = RemoveEmptyListEntries(filesString);
		
		return FilterUsingSelection(filesList);
	}

	/* **** GetDeletedFilesList **** */

	public static string[] GetRemotesList () {
		return RemoveEmptyListEntries(RunGitCmd("remote"));
	}

	/* **** Removes any empty strings (typically found at the end of the array) **** */

	static string[] RemoveEmptyListEntries (string listString)
	{
		string[] items = listString.Split ('\n');
		List<string> itemsList = new List<string> ();
		
		for (int i = 0; i < items.Length; i++)
			if (Regex.Replace (items[i], "\\s+", "") != "")
				itemsList.Add (items[i]);
		
		return itemsList.ToArray ();
	}


	/* **** Filters files based on a selected directory **** */

	static string[] FilterUsingSelection(string[] files) {
		if ( Selection.activeObject != null ) {
			List<string> filteredFiles = new List<string>();
			string baseDirectory = AssetDatabase.GetAssetPath(Selection.activeObject);

			foreach ( string file in files ) {
				if ( file.StartsWith(baseDirectory) ) {
					filteredFiles.Add(file);
				}
			}

			return filteredFiles.ToArray();
		}

		return files;
	}


	/* **** RunGitCmd **** */

	public static string RunGitCmd (string command)
	{
		string cmd = GetGitExePath();
		string repoPath = GetRepoPath();

		if ( cmd != "" ) {
			Process proc = new Process ();
			ProcessStartInfo startInfo = new ProcessStartInfo (cmd);
			StreamReader streamReader;
			string result;

			startInfo.Arguments = "--git-dir=\"" + repoPath + "/.git\" --work-tree=\"" + repoPath + "\" " + command;
		
			startInfo.UseShellExecute = false;
			startInfo.RedirectStandardInput = true;
			startInfo.RedirectStandardOutput = true;
			startInfo.CreateNoWindow = true;
		
			proc.StartInfo = startInfo;
		
			proc.Start ();
		
			streamReader = proc.StandardOutput;
		
			while (!proc.HasExited)
				Thread.Sleep (0);
		
			result = streamReader.ReadToEnd ();
		
			proc.Close ();
		
			return result;
		}

		return "No Git.exe path defined!";
	}


	const string git32 = "\"C:\\Program Files (x86)\\Git\\bin\\git.exe\"";
	const string git64 = "\"C:\\Program Files\\Git\\bin\\git.exe\"";

	static string GetGitExePath() {
		string locationKey = "GitLocation";
		string location;

		if ( EditorPrefs.HasKey(locationKey) ) {
			string loc = EditorPrefs.GetString(locationKey);

			if ( File.Exists(loc) ) {
				return loc;
			}
		}

		if ( File.Exists(git64) ) {
			EditorPrefs.SetString(locationKey, git64);
			return git64;
		}

		if ( File.Exists(git32) ) {
			EditorPrefs.SetString(locationKey, git32);
			return git32;
		}

		location = EditorUtility.OpenFilePanel("Where is Git.exe?", "C:\\Program Files\\", "exe");

		if ( File.Exists(location) ) {
			EditorPrefs.SetString(locationKey, location);
			return location;
		}

		return "";
	}


	/* **** Git Help **** */

	[MenuItem("Git/Help")]
	static void ShowGitHelp ()
	{
		RunGitCmd ("help git");
	}
}
