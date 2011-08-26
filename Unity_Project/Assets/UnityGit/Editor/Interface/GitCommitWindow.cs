using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

public class GitCommitWindow : EditorWindow
{
	public static GitCommitWindow Instance { get; private set; }

	string[] modifiedFiles = GitSystem.GetModifiedFilesList();
	bool[] commitModifiedFiles;
	string[] untrackedFiles = GitSystem.GetUntrackedFilesList();
	bool[] commitUntrackedFiles;
	string[] deletedFiles = GitSystem.GetDeletedFilesList();
	bool[] commitDeletedFiles;

	bool somethingToCommit = false;
	string commitMessage = "";


	public static void Init ()
	{
		int fileCount = 0;

		// Get existing open window or if none, make a new one:
		Instance = EditorWindow.GetWindow<GitCommitWindow>(true, "Git Commit");

		Instance.modifiedFiles = GitSystem.GetModifiedFilesList();
		Instance.untrackedFiles = GitSystem.GetUntrackedFilesList();
		Instance.deletedFiles = GitSystem.GetDeletedFilesList();

		Instance.commitModifiedFiles = new bool[Instance.modifiedFiles.Length];
		Instance.commitUntrackedFiles = new bool[Instance.untrackedFiles.Length];
		Instance.commitDeletedFiles = new bool[Instance.deletedFiles.Length];

		for ( int i = 0; i < Instance.commitModifiedFiles.Length; i++ )
			Instance.commitModifiedFiles[i] = true;

		for ( int i = 0; i < Instance.commitDeletedFiles.Length; i++ )
			Instance.commitDeletedFiles[i] = true;

		fileCount = Instance.modifiedFiles.Length;
		fileCount += Instance.untrackedFiles.Length;
		fileCount += Instance.deletedFiles.Length;

		Instance.somethingToCommit = fileCount > 0;
		Instance.modifiedFiles = GitSystem.ConformModifyListToDeletionList(Instance.modifiedFiles, Instance.deletedFiles);
	}


	Vector2 scrollPosition = Vector2.zero;
	int lastSelectedIndex = -1;

	void OnGUI()
	{
		if ( somethingToCommit )
		{
			Color baseContentColor = GUI.contentColor;

			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			GUI.contentColor = Color.cyan;
			for ( int i = 0; i < modifiedFiles.Length; i++ )
			{
				bool prevValue = commitModifiedFiles[i];

				commitModifiedFiles[i] = GUILayout.Toggle(commitModifiedFiles[i], modifiedFiles[i]);

				if ( prevValue != commitModifiedFiles[i] )
				{
					if ( commitModifiedFiles[i] )
						lastSelectedIndex = i;
					else
						lastSelectedIndex = -1;
				}
			}

			GUI.contentColor = baseContentColor;
			for ( int i = 0; i < untrackedFiles.Length; i++ )
			{
				bool prevValue = commitUntrackedFiles[i];

				commitUntrackedFiles[i] = GUILayout.Toggle(commitUntrackedFiles[i], untrackedFiles[i]);

				if ( prevValue != commitUntrackedFiles[i] )
				{
					if ( commitUntrackedFiles[i] )
						lastSelectedIndex = i + modifiedFiles.Length;
					else
						lastSelectedIndex = -1;
				}
			}

			GUI.contentColor = Color.red;
			for ( int i = 0; i < deletedFiles.Length; i++ )
			{
				bool prevValue = commitDeletedFiles[i];

				commitDeletedFiles[i] = GUILayout.Toggle(commitDeletedFiles[i], deletedFiles[i]);

				if ( prevValue != commitDeletedFiles[i] )
				{
					if ( commitDeletedFiles[i] )
						lastSelectedIndex = i + modifiedFiles.Length + commitUntrackedFiles.Length;
					else
						lastSelectedIndex = -1;
				}
			}
			GUILayout.EndScrollView();

			// TODO: Add shift key to select a range of files to commit
			if ( Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) )
				GUILayout.Label(lastSelectedIndex.ToString());

			// Select All and None
			GUI.contentColor = baseContentColor;
			GUILayout.BeginHorizontal();
			if ( GUILayout.Button("Select All", GUILayout.MaxWidth(100)) )
			{
				for ( int i = 0; i < commitModifiedFiles.Length; i++ )
					commitModifiedFiles[i] = true;

				for ( int i = 0; i < commitUntrackedFiles.Length; i++ )
					commitUntrackedFiles[i] = true;

				for ( int i = 0; i < commitDeletedFiles.Length; i++ )
					commitDeletedFiles[i] = true;
			}
			else if ( GUILayout.Button("Select None", GUILayout.MaxWidth(100)) )
			{
				for ( int i = 0; i < commitModifiedFiles.Length; i++ )
					commitModifiedFiles[i] = false;

				for ( int i = 0; i < commitUntrackedFiles.Length; i++ )
					commitUntrackedFiles[i] = false;

				for ( int i = 0; i < commitDeletedFiles.Length; i++ )
					commitDeletedFiles[i] = false;
			}
			GUILayout.EndHorizontal();

			GUILayout.Label("");
			GUILayout.Label("Commit message:");
			commitMessage = GUILayout.TextArea(commitMessage, GUILayout.MinHeight(45));
			commitMessage = commitMessage.Replace("\"", "");
			commitMessage = commitMessage.Replace("\'", "");

			// Commit and Cancel
			if ( commitMessage != "" )
			{
				GUILayout.Label("");
				GUILayout.BeginHorizontal();
				if ( GUILayout.Button("Cancel", GUILayout.MaxWidth(100)) )
				{
					Close();
				}
				else if ( GUILayout.Button("Commit", GUILayout.MaxWidth(100)) )
				{
					DoCommit();
					Close();
				}
				GUILayout.EndHorizontal();
			}
		}
		else
		{
			GUILayout.Label("Nothing to commit...");
		}
	}


	void DoCommit()
	{
		List<string> addFiles = new List<string>();
		List<string> removeFiles = new List<string>();

		for ( int i = 0; i < modifiedFiles.Length; i++ )
		{
			if ( commitModifiedFiles[i] )
			{
				addFiles.Add(modifiedFiles[i]);
			}
		}

		for ( int i = 0; i < untrackedFiles.Length; i++ )
		{
			if ( commitUntrackedFiles[i] )
			{
				addFiles.Add(untrackedFiles[i]);
			}
		}

		for ( int i = 0; i < deletedFiles.Length; i++ )
		{
			if ( commitDeletedFiles[i] )
			{
				removeFiles.Add(deletedFiles[i]);
			}
		}

		GitSystem.Commit(commitMessage, addFiles.ToArray(), removeFiles.ToArray());
	}
}