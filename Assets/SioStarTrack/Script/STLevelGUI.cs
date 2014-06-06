using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public enum windowShowContentType
{
	saving,
	loading,
	levelGeneration,
}

public class STLevelGUI : MonoBehaviour {

	public static bool editMode = true;
	public static bool editNodesMode = false;


	bool isOnGUI = true;
	int selectedSkin = 0;
	int genCount = 0;

	windowShowContentType windowType = windowShowContentType.loading;

	int depthLevelValue;

	List <string> genBranchesCountStringsMin;
	List <bool>   genBranchesStringMatchesMin;

	List <string> genBranchesCountStringsMax;
	List <bool>   genBranchesStringMatchesMax;

	List <string> genBranchesLengthStringsMin;
	List <bool>   genBranchesLengthMatchesMin;
	
	List <string> genBranchesLengthStringsMax;
	List <bool>   genBranchesLengthMatchesMax;

	string fileNameString;
	string depthLevelString;

	bool regMatchDepth = false;
	
	//bool isSavingMode = false;
	bool oldToogleState;
	
	bool isDialogHiden = false;
	bool isCantSaveDialog = false;
	
//	bool isEditGridMode = false;
	int selectionGrid = 0;
	//int levelGenMode;
	Rect windowRect;
	Rect toogleRect;
	Rect toolBarRect;

	
	Vector2 scrollPosition;
	Vector2 guiScale = new Vector2(1.0f, 1.0f);
	
	string [] levelsArray;
	private string[] selectionStrings = {"2 Symetry", "2 Diag Symetry", "4 Symetry", "No Symetry"};
	
	int buttonHeight = 50;
	int minMaxLabelWidth = 75;

	
	string minBCount = "6";
	string maxBCount = "6";
	string minBLength = "1";
	string maxBLength = "2";
	Regex reg;
	//Regex regLength;

	public GUIStyle style;

	void updateLevelsList ()
	{
		levelsArray = STLevel.GetLevelsList();
	}
	
	void Start () {	

		genBranchesCountStringsMin = new List <string> ();
		genBranchesStringMatchesMin = new List <bool> ();

		genBranchesCountStringsMax = new List <string> ();
		genBranchesStringMatchesMax = new List <bool> ();

		genBranchesLengthStringsMin = new List <string> ();
		genBranchesLengthMatchesMin = new List <bool> ();
		
		genBranchesLengthStringsMax = new List <string> ();
		genBranchesLengthMatchesMax = new List <bool> ();

	


		for (int i = 0; i < 6; i++)
		{
			genBranchesStringMatchesMin.Add (true);
			genBranchesStringMatchesMax.Add (true);

			genBranchesLengthMatchesMin.Add (true);
			genBranchesLengthMatchesMax.Add (true);
		}

		reg = new Regex ("^[1-6]");
		//regLength = new Regex ("^[1-4]");
		scrollPosition = Vector2.zero;
		fileNameString = "puzzle";
		depthLevelString = "3";
		
		updateLevelsList();
		
		oldToogleState = editMode;
		
		windowRect = new Rect (Screen.width * 0.75f / guiScale.x, 0, Screen.width * 0.25f / guiScale.x, Screen.height / guiScale.y);
		toogleRect = new Rect (Screen.width * 0.0f, Screen.width * 0.0f, Screen.width * 0.1f / guiScale.x, Screen.width * 0.075f / guiScale.y);
		toolBarRect = new Rect (Screen.width * 0.025f / guiScale.x, Screen.width / 10 / guiScale.y, Screen.width * 0.1f, Screen.width * 0.5f);
		
		scrollPosition = new Vector2 (Screen.width , windowRect.y);
		
	
		//STLevel.GetNodesNet().ChangeNetNodesState(false);

		if (STScene.getMode() == SCENE_MODE.TEST_ANIM)
		{

			//changeGameMode (true);
			STLevel.isEditMode = false;
			STLevel.GetRootNode().SetEditMode (false);
			StartCoroutine(genLevelWithDelay());
		}
		
	}

	IEnumerator genLevelWithDelay() {
		yield return new WaitForSeconds(1);
		generateLevel();

	}
	
	// Update is called once per frame
	void Update () {
		 
	}

	void changeGameMode (bool isEditModeVal)
	{
		if (isEditModeVal == true)
		{
			STLevel.GetControl().ChangeEnabledState (false);
			STLevel.Solve();
			STLevel.isEditMode = true;
			STLevel.GetRootNode().SetEditMode (true);
			STLevel.GetRootNode().SetEnableTouches (true);
			STLevel.GetNodesNet().ChangeNetNodesState(false);
			
		}
		else
		{
			STLevel.CalcTreeRects();
			
			STLevel.GetRootNode().SaveSolution();
			editNodesMode = false;
			STLevel.GetRootNode().SetEditMode (false);
			STLevel.GetNodesNet().ChangeNetNodesState(false);
			STLevel.GetControl().ChangeEnabledState (true);
			STLevel.isEditMode = false;
			STLevel.GetRootNode().SetEnableTouches (true);
			STLevel.StartLevel();
		}
	}
	
	void guiModeButton()
	{
		editMode = GUI.Toggle (toogleRect , editMode, editMode ? "Edit" :"Play");

		if (editMode != oldToogleState)
		{
			changeGameMode (editMode);
			oldToogleState = editMode;
		}
	}

	void guiPlayToolBarButtons ()
	{
		GUI.BeginGroup (toolBarRect);
		{
			GUILayout.BeginVertical();
			{
				if (GUILayout.Button ("Mix Level", GUILayout.Height (buttonHeight)))
				{
					STLevel.Mix();
				}

				if (GUILayout.Button ("Solve Level", GUILayout.Height (buttonHeight)))
				{
					STLevel.Solve();
				}

				if (GUILayout.Button ("Next Skin", GUILayout.Height (buttonHeight)))
				{
					selectedSkin ++;
					if (selectedSkin > 5)
						selectedSkin = 0;

					STLevel.ChangeSkin (selectedSkin);
				}
			}
			GUILayout.EndVertical();
		}
		GUI.EndGroup();
	}
	
	void guiEditToolBarButtons ()
	{


		GUI.BeginGroup (toolBarRect);
		GUILayout.BeginVertical();
		
		if (GUILayout.Button ("Load Level", GUILayout.Height (buttonHeight)))
		{
			isDialogHiden = false;
			//isSavingMode = false;
			windowType = windowShowContentType.loading;
			isCantSaveDialog = false;
		}
		
		if (GUILayout.Button ("Save Level", GUILayout.Height (buttonHeight)))
		{
			isDialogHiden = false;
			windowType = windowShowContentType.saving;
			isCantSaveDialog = false;
		}
		
		if (GUILayout.Button ("Clear Level", GUILayout.Height (buttonHeight)))
		{
			STLevel.GetRootNode().Reset();
			STLevel.GetNodesNet().Reset();
		}
			
		if (editNodesMode == true)
		{
			if (GUILayout.Button ("Edit Grid", GUILayout.Height (buttonHeight)))
			{
				guiNodesButton();
			}
		}

		if (GUILayout.Button ("Random Level", GUILayout.Height (buttonHeight)))
		{
			//STLevel.GenerateRandomLevel ();
			windowType = windowShowContentType.levelGeneration;
		}
		
		if (isDialogHiden == false)
		{
			if (GUILayout.Button ("Hide Dialog", GUILayout.Height (buttonHeight)))
			{
				isDialogHiden = true;
			}
		}
		else
		{
			if (GUILayout.Button ("Show Dialog", GUILayout.Height (buttonHeight)))
			{
				isDialogHiden = false;
			}
		}
		
//		if (GUILayout.Button ("Align Net To Node", GUILayout.Height (buttonHeight)))
//		{
//			STLevel.GetNodesNet().showNetNode = STRootNode.activeNode;
//		}
		

		
		GUILayout.EndVertical();
		
		GUI.EndGroup();
	}
	
	
	void guiNodesButton ()
	{
		editNodesMode = !editNodesMode;
			
			if (editNodesMode == true)
			{
				STLevel.GetNodesNet().ChangeNetNodesState(true);
				STLevel.GetRootNode().SetEditMode (true);
				
				STLevel.GetRootNode().SetEnableTouches (false);
			}
			else
			{
				STLevel.GetNodesNet().ChangeNetNodesState(false);
				STLevel.GetRootNode().SetEditMode (true);
			
				STLevel.GetRootNode().SetEnableTouches (true);
			}
	}
		
	void OnGUI ()
	{
		GUIUtility.ScaleAroundPivot (guiScale, Vector2.zero);

	if (isOnGUI == false)
			return;

		if (STScene.getMode() == SCENE_MODE.TEST_ANIM)
		{
			windowType = windowShowContentType.levelGeneration;
			windowRect = GUI.Window (0, windowRect, windowFunction, "Levels Manager");
		
			return;
		}

		guiModeButton ();
		
		if (editMode == true)
		{
			guiEditToolBarButtons ();
			
			if (isDialogHiden == false)
				windowRect = GUI.Window (0, windowRect, windowFunction, "Levels Manager");
		}
		else
		{
			guiPlayToolBarButtons ();
		}
	}
	

	void windowFunction (int windowID) 
	{
		if (isCantSaveDialog == false)
		{
			
			if (windowType == windowShowContentType.saving)
			{
				windowShowSaveDialog();
			}
			else if (windowType == windowShowContentType.loading)
			{
				windowShowLoadDialog();
			}
			else if (windowType == windowShowContentType.levelGeneration)
			{
				windowShowGenerationDialog ();
			}
		}
		else
			showCantSaveFileDialog();
		
		//GUI.DragWindow();
	}
	
	void showCantSaveFileDialog ()
	{
		GUILayout.BeginVertical ();
		{
			GUILayout.Label ("Can't save the file.");
			GUILayout.Label ("The file with the same name may exist");
			if (GUILayout.Button ("OK"))
			{
				isCantSaveDialog = false;
			}
		}
		GUILayout.EndVertical ();
	}

	void windowShowGenerationDialog ()
	{
	
		scrollPosition = GUILayout.BeginScrollView (scrollPosition);
		{
			selectionGrid = GUILayout.SelectionGrid (selectionGrid, selectionStrings, 2, GUILayout.Height(buttonHeight * 2));

			GUILayout.Label ("Level depth");
			GUILayout.BeginHorizontal();
			{
				checkDepthString ();
				if (regMatchDepth == true)
					depthLevelString = GUILayout.TextField (depthLevelString);
				else
					GUILayout.TextField (depthLevelString);

				if (GUILayout.Button ("+"))
				{
					if (depthLevelString[0] < '6')
					{

						depthLevelString = new string ( (char)  (depthLevelString[0] + 1), 1);
					}
				}

				if (GUILayout.Button ("-"))
				{
					if (depthLevelString[0] > '1')
					{
						
						depthLevelString = new string ( (char)  (depthLevelString[0] - 1), 1);
					}
				}

			
			}
			GUILayout.EndHorizontal ();

			GUILayout.Label ("Branches generation parameters");

		//	if (genBranchesCount.Count > 0)
			for (int i = 0; i < genBranchesCountStringsMin.Count ; i++)
			{
				GUILayout.BeginVertical ();
				{
					Color oldColor = GUI.contentColor;
					GUI.contentColor = Color.red;

					GUILayout.Label (string.Format ("Branch #{0}", i + 1));

					GUI.contentColor = oldColor;
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min Count", GUILayout.MaxWidth (minMaxLabelWidth));
						checkBranchesMinString(i);

						if (genBranchesStringMatchesMin[i])
							genBranchesCountStringsMin[i] = GUILayout.TextField (genBranchesCountStringsMin[i]);
						else
							GUILayout.TextField (genBranchesCountStringsMin[i]);

						if (GUILayout.Button ("+"))
						{
							if (genBranchesCountStringsMin[i][0] < '6')
							{
								
								genBranchesCountStringsMin[i] = new string ( (char)  (genBranchesCountStringsMin[i][0] + 1), 1);
							}
						}
						
						if (GUILayout.Button ("-"))
						{
							if (genBranchesCountStringsMin[i][0] > '1')
							{
								
								genBranchesCountStringsMin[i] = new string ( (char)  (genBranchesCountStringsMin[i][0] - 1), 1);
							}
						}

					}
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Max Count", GUILayout.MaxWidth (minMaxLabelWidth));
						checkBranchesMaxString(i);
						
						if (genBranchesStringMatchesMax[i])
							genBranchesCountStringsMax[i] = GUILayout.TextField (genBranchesCountStringsMax[i]);
						else
							GUILayout.TextField (genBranchesCountStringsMax[i]);
						
						if (GUILayout.Button ("+"))
						{
							if (genBranchesCountStringsMax[i][0] < '6')
							{
								
								genBranchesCountStringsMax[i] = new string ( (char)  (genBranchesCountStringsMax[i][0] + 1), 1);
							}
						}
						
						if (GUILayout.Button ("-"))
						{
							if (genBranchesCountStringsMax[i][0] > genBranchesCountStringsMin[i][0])
							{
								
								genBranchesCountStringsMax[i] = new string ( (char)  (genBranchesCountStringsMax[i][0] - 1), 1);
							}
						}
						
					}
					GUILayout.EndHorizontal();


					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Min Length", GUILayout.MaxWidth (minMaxLabelWidth));
						checkBranchesLengthMinString(i);

						if (genBranchesLengthMatchesMin[i])
							genBranchesLengthStringsMin[i] = GUILayout.TextField (genBranchesLengthStringsMin[i]);
						else
							GUILayout.TextField (genBranchesLengthStringsMin[i]);
						
						if (GUILayout.Button ("+"))
						{
							if (genBranchesLengthStringsMin[i][0] < '4')
							{
								genBranchesLengthStringsMin[i] = new string ( (char)  (genBranchesLengthStringsMin[i][0] + 1), 1);
							}
						}
						
						if (GUILayout.Button ("-"))
						{
							if (genBranchesLengthStringsMin[i][0] > '1')
							{
								
								genBranchesLengthStringsMin[i] = new string ( (char)  (genBranchesLengthStringsMin[i][0] - 1), 1);
							}
						}
					}
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					{
						GUILayout.Label ("Max Length", GUILayout.MaxWidth (minMaxLabelWidth));
						checkBranchesLengthMaxString(i);
						
						if (genBranchesLengthMatchesMax[i])
							genBranchesLengthStringsMax[i] = GUILayout.TextField (genBranchesLengthStringsMax[i]);
						else
							GUILayout.TextField (genBranchesLengthStringsMax[i]);

						if (GUILayout.Button ("+"))
						{
							if (genBranchesLengthStringsMax[i][0] < '4')
							{
								genBranchesLengthStringsMax[i] = new string ( (char)  (genBranchesLengthStringsMax[i][0] + 1), 1);
							}
						}
						
						if (GUILayout.Button ("-"))
						{
							if (genBranchesLengthStringsMax[i][0] > genBranchesLengthStringsMin[i][0])
							{
								
								genBranchesLengthStringsMax[i] = new string ( (char)  (genBranchesLengthStringsMax[i][0] - 1), 1);
							}
						}
						
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical ();
			}

			if (GUILayout.Button ("Generate",  GUILayout.Height (buttonHeight)))
			{
				generateLevel();
			}

		}
		GUILayout.EndScrollView();
	}

	void generateLevel ()
	{
		List <int> minValues = new List <int> ();
		List <int> maxValues = new List <int> ();
		
		List <int> minLValues = new List <int> ();
		List <int> maxLValues = new List <int> ();
		
		//Debug.Log ("GENERATE");
		
		for (int i = 0; i < 6; i++)
		{
			minValues.Add (1);
			maxValues.Add (1);
			
			minLValues.Add (1);
			maxLValues.Add (1);
		}
		
		
		for (int i = 0; i < depthLevelValue; i++)
		{
			minValues[i] = genBranchesCountStringsMin[i][0] - '0';
			maxValues[i] = genBranchesCountStringsMax[i][0] - '0';
			
			minLValues[i] = genBranchesLengthStringsMin[i][0] - '0';
			maxLValues[i] = genBranchesLengthStringsMax[i][0] - '0';
			
			
		}
		
		SymetryMode mode = SymetryMode.symetry_0 ;
		
		switch (selectionGrid)
		{
		case 0:
			mode = SymetryMode.symetry_2;
			break;
			
		case 1:
			mode = SymetryMode.symetry_2fliped;
			break;
			
		case 2:
			mode = SymetryMode.symetry_4;
			break;
			
		case 3:
			mode = SymetryMode.symetry_0;
			break;
		}
		
		//Debug.Log ("SYM MODE" + selectionGrid);
		
		STPatternManager.SetGenParameters (depthLevelValue, minValues, maxValues, minLValues, maxLValues, mode);
		STLevel.GenerateRandomLevel();

		STLevel.GetRootNode().SetAnimation ();

		if (STScene.getMode() == SCENE_MODE.TEST_ANIM)
		{
			genCount ++;

			if (genCount == 2)
			isOnGUI = false;
		}

        STLevel.GetRootNode().UpdateLinkPosition ( STLevel.GetRootNode().transform.position );

	}

	void checkDepthString()// (string stringToCheck)
	{
		Match regMatch = reg.Match(depthLevelString) ;
		if (depthLevelString.Length == 0)
		{
			depthLevelString = "1";
		}

		if (((regMatch.Success) && (depthLevelString.Length == 1)) )
		{
			//Debug.Log ("match");

			regMatchDepth = true;

			if (depthLevelString.Length > 0)
			if (depthLevelString[0] - '0' != depthLevelValue)
			{
				int depthVal = depthLevelString[0] - '0';
				if (depthVal > depthLevelValue)
				{

					do
					{
						genBranchesCountStringsMin.Add (minBCount);
						genBranchesCountStringsMax.Add (maxBCount);

						genBranchesLengthStringsMin.Add (minBLength);
						genBranchesLengthStringsMax.Add (maxBLength);

						depthLevelValue ++;
					}while (depthVal  > depthLevelValue);
				}
				else
				{
					do
					{
						genBranchesCountStringsMin.RemoveAt (genBranchesCountStringsMin.Count - 1);
						genBranchesCountStringsMax.RemoveAt (genBranchesCountStringsMax.Count - 1);
						depthLevelValue --;
					}while (depthVal  < depthLevelValue);
				}

			}


		}
		else
		{
			regMatchDepth = false;

			if (depthLevelString[depthLevelString.Length - 1] > '0' && depthLevelString[depthLevelString.Length - 1] <= '6')
				depthLevelString =string.Format("{0}", depthLevelString[depthLevelString.Length - 1]);
			else if (depthLevelString.Length > 1)
				if (depthLevelString[depthLevelString.Length - 2] > '0' && depthLevelString[depthLevelString.Length - 2] <= '6')
					depthLevelString =string.Format("{0}", depthLevelString[depthLevelString.Length - 2]);
				else
					depthLevelString = "1";
		}

	
	}

	void checkBranchesMinString (int nBranch)
	{
		//string genBranchesCountStringsMin[nBranch] = genBranchesCountStringsMin[nBranch];
		Match regMatch = reg.Match(genBranchesCountStringsMin[nBranch]);
		if (genBranchesCountStringsMin[nBranch].Length == 0)
		{
			genBranchesCountStringsMin[nBranch] = "1";
		}
		
		if (((regMatch.Success) && (genBranchesCountStringsMin[nBranch].Length == 1)) )
		{
			//Debug.Log ("match");
			
			genBranchesStringMatchesMin[nBranch] = true;

		}
		else
		{
			genBranchesStringMatchesMin[nBranch] = false;
			
			if (genBranchesCountStringsMin[nBranch][genBranchesCountStringsMin[nBranch].Length - 1] > '0' && genBranchesCountStringsMin[nBranch][genBranchesCountStringsMin[nBranch].Length - 1] <= '6')
				genBranchesCountStringsMin[nBranch] = string.Format("{0}", genBranchesCountStringsMin[nBranch][genBranchesCountStringsMin[nBranch].Length - 1]);
			else if (genBranchesCountStringsMin[nBranch].Length > 1)
				if (genBranchesCountStringsMin[nBranch] [genBranchesCountStringsMin[nBranch].Length - 2] > '0' && genBranchesCountStringsMin[nBranch][genBranchesCountStringsMin[nBranch].Length - 2] <= '6')
					genBranchesCountStringsMin[nBranch] = string.Format("{0}", genBranchesCountStringsMin[nBranch][genBranchesCountStringsMin[nBranch].Length - 2]);
			else
				genBranchesCountStringsMin[nBranch] = "1";
		}
	}

	void checkBranchesLengthMaxString (int nBranch)
	{
		if ((genBranchesLengthStringsMax[nBranch].Length > 0) && (genBranchesLengthStringsMax[nBranch][0] >= genBranchesLengthStringsMin[nBranch][0]) && (genBranchesLengthStringsMax[nBranch][0] <= '6'))
		{
			genBranchesStringMatchesMax[nBranch] = true;
		}
		else
		{
			genBranchesStringMatchesMax[nBranch] = false;
			genBranchesLengthStringsMax[nBranch] = genBranchesLengthStringsMin[nBranch];
		}

	}


	void checkBranchesLengthMinString (int nBranch)
	{
		//string genBranchesLengthStringsMin[nBranch] = genBranchesLengthStringsMin[nBranch];
		Match regMatch = reg.Match(genBranchesLengthStringsMin[nBranch]);
		if (genBranchesLengthStringsMin[nBranch].Length == 0)
		{
			genBranchesLengthStringsMin[nBranch] = "1";
		}
		
		if (((regMatch.Success) && (genBranchesLengthStringsMin[nBranch].Length == 1)) )
		{
			//Debug.Log ("match");
			
			genBranchesStringMatchesMin[nBranch] = true;
			
		}
		else
		{
			genBranchesStringMatchesMin[nBranch] = false;
			
			if (genBranchesLengthStringsMin[nBranch][genBranchesLengthStringsMin[nBranch].Length - 1] > '0' && genBranchesLengthStringsMin[nBranch][genBranchesLengthStringsMin[nBranch].Length - 1] <= '6')
				genBranchesLengthStringsMin[nBranch] = string.Format("{0}", genBranchesLengthStringsMin[nBranch][genBranchesLengthStringsMin[nBranch].Length - 1]);
			else if (genBranchesLengthStringsMin[nBranch].Length > 1)
				if (genBranchesLengthStringsMin[nBranch] [genBranchesLengthStringsMin[nBranch].Length - 2] > '0' && genBranchesLengthStringsMin[nBranch][genBranchesLengthStringsMin[nBranch].Length - 2] <= '6')
					genBranchesLengthStringsMin[nBranch] = string.Format("{0}", genBranchesLengthStringsMin[nBranch][genBranchesLengthStringsMin[nBranch].Length - 2]);
			else
				genBranchesLengthStringsMin[nBranch] = "1";
		}
	}
	
	void checkBranchesMaxString (int nBranch)
	{
		if ((genBranchesCountStringsMax[nBranch].Length > 0) && (genBranchesCountStringsMax[nBranch][0] >= genBranchesCountStringsMin[nBranch][0]) && (genBranchesCountStringsMax[nBranch][0] <= '6'))
		{
			genBranchesStringMatchesMax[nBranch] = true;
		}
		else
		{
			genBranchesStringMatchesMax[nBranch] = false;
			genBranchesCountStringsMax[nBranch] = genBranchesCountStringsMin[nBranch];
		}
		
	}


	void windowShowSaveDialog ()
	{
		GUILayout.Label("");
		GUILayout.Label("");


		fileNameString = GUILayout.TextArea (fileNameString);

		GUILayout.Label("");


		if (GUILayout.Button ("SAVE", GUILayout.Height(buttonHeight)))
		{
			windowType = windowShowContentType.loading;
			if (STLevel.SaveLevel(fileNameString, false) == false)
			{
				isCantSaveDialog = true;
				windowType = windowShowContentType.saving;
			}
			else
			{
				updateLevelsList();
			}
		}
	}
	
	void windowShowLoadDialog ()
	{
		scrollPosition =  GUILayout.BeginScrollView (scrollPosition);
		{
			foreach (string levelName in levelsArray)
			{
				GUILayout.BeginHorizontal ();
				{
						GUILayout.Label (levelName);
				
						if (GUILayout.Button ("Load"))
						{
							editNodesMode = false;
							STLevel.LoadLevel (levelName);
							STLevel.GetNodesNet().ChangeNetNodesState(false);
							STLevel.GetRootNode().SetEnableTouches (true);
						}
				
						if (GUILayout.Button ("Delete"))
						{
							STLevel.DeleteLevel (levelName);
							updateLevelsList();
						}
				}
			
				GUILayout.EndHorizontal ();
			}
		}
		GUILayout.EndScrollView();
	}
}
