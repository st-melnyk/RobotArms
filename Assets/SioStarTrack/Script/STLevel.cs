using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class STLevel: MonoBehaviour  {

	// Use this for initialization

	public GameObject BackGround;
	public STRootNode RootNode;
	//public GameObject RootNode;
	public STNodesNet NodesNet;
	public STControl  Control;
	public STSkinsManager SkinsMngr;

	public STOverlayNodes OverlayInfoPref;

	static List <STOverlayNodes> overlayedNodesList = new List <STOverlayNodes> ();

	static List <STNode> activeTreeNodes = new List <STNode> ();

	static STLevel _instance = null;

	static STSerializedLevel sLevel;
	static ProtoSerialization sLevelSerializer;
	static public bool isEditMode = true;

	static List <Vector3> collectableNetNodesPos = new List <Vector3> ();
	static public STLevel Instance(){
	
		if (_instance == null)
		{
			GameObject level = GameObject.Find("Level");

			if ( level == null ){
				Debug.LogError("Error get Level");
				return null;
			}
			
			_instance = level.GetComponent<STLevel>();
			

	_instance.RootNode = GameObject.Instantiate (_instance.RootNode) as STRootNode;
		//	GameObject obj = GameObject.Find ("RootNode");
		//	_instance.RootNode = obj.GetComponent <STRootNode> ();
			
			if (_instance.RootNode == null)
			{
				Debug.LogError("Error getting root node");
				return null;
			}

//			public GameObject Node;
//			public GameObject Leaf;
//			public GameObject ActiveNode;

//			_instance.RootNode.mVisual.ActiveNode.renderer.enabled = false;
//			_instance.RootNode.mVisual.Leaf.renderer.enabled = false;
//			_instance.RootNode.mVisual.Node.renderer.enabled = false;

			
			_instance.NodesNet = GameObject.Instantiate (_instance.NodesNet) as STNodesNet;
			
			if (_instance.NodesNet == null)
			{
				Debug.LogError("Error getting nodes net");
				return null;
			}
			
			_instance.Control = GameObject.Find("Background").GetComponent<STControl>() ;
			_instance.SkinsMngr = GameObject.Instantiate (_instance.SkinsMngr) as STSkinsManager;

			if (_instance.SkinsMngr == null)
			{
				Debug.LogError("Error getting skins manager");
				return null;
			}
			
			sLevelSerializer = new ProtoSerialization ();

		

		}
		return _instance;
	}
	
	static public STNodesNet GetNodesNet ()
	{
		return _instance.NodesNet;
	}
	
	static public STRootNode GetRootNode ()
	{
		return _instance.RootNode;
	}
	
	static public STControl GetControl ()
	{
		return _instance.Control;
	}

	static public STSkin GetCurrentSkin ()
	{
		return _instance.SkinsMngr.GetCurrentSkin();
	}

	static public void ChangeSkin (int skinNumber)
	{
		_instance.SkinsMngr.ChangeCurrentSkin(skinNumber);
		_instance.BackGround.GetComponent<SpriteRenderer>().color = _instance.SkinsMngr.GetCurrentSkin().BackgroundColor;
		//_instance.RootNode.star;
		_instance.NodesNet.RefreshNet();
	}

	void Start()
	{
	//	Camera.main.orthographicSize = ScreenSize.y;
		_instance.BackGround.GetComponent<SpriteRenderer>().color = _instance.SkinsMngr.GetCurrentSkin().BackgroundColor;

//		if (STScene.getMode() == SCENE_MODE.TEST_ANIM)
//		{
//
//		}
	}

	void Update()
	{

	}

	
	public STSerializedLevel GetSerializedLevel()
	{
		STSerializedLevel sLevel = new STSerializedLevel ();

		sLevel.rootNode = new STSerializedNode ();
		sLevel.rootNode = _instance.RootNode.GetSerializedObject(sLevel.rootNode);
		sLevel.net = _instance.NodesNet.GetSerializedObject();

		return sLevel;
	}
	
	private static void clearLevel ()
	{
		_instance.RootNode.Reset();
		_instance.NodesNet.Reset();
	}
	

	public static bool SaveLevel (string fileName, bool shouldRewrite)
	{
		 sLevel = STLevel.Instance().GetSerializedLevel();
	
		return STFileMng.SaveData (sLevel, sLevelSerializer, fileName, shouldRewrite);
		
	}
	
	public static void LoadLevel (string fileName)
	{
		
		sLevel = new STSerializedLevel ();
		//STSerializedLevel data;
		
		if ( STFileMng.LoadData (fileName, sLevelSerializer, out sLevel))
		{
			clearLevel();

			_instance.NodesNet.LoadNet (sLevel.net);
			_instance.RootNode.LoadNodes (sLevel.rootNode);
		}
		else
		{
			
		}
	
	}
	
	public static void DeleteLevel (string fileName)
	{
		STFileMng.DeleteData (fileName);
	}
	
	public static string [] GetLevelsList ()
	{
		string [] levelsFullPathes = STFileMng.GetFilesList();
		string [] levelsArray = new string  [levelsFullPathes.Length];
		
		for (int i = 0; i < levelsFullPathes.Length; i++)
		{
			levelsArray[i] = Path.GetFileNameWithoutExtension (levelsFullPathes[i]);
		}
		
		return levelsArray;
	}
	
	public static void GenerateRandomLevel ()
	{
		STPatternManager.SetMode (GenMode.randomMode);

		int levelDepth = STPatternManager.GetDepth();

		STLevel.clearLevel();
	
		
		_instance.RootNode.GenerateRandomLevel (levelDepth);

		if (STPatternManager.GetSymetryMode() == SymetryMode.symetry_0)
		{

		}
		else if (STPatternManager.GetSymetryMode() == SymetryMode.symetry_2)
		{
			_instance.RootNode.MirrorLevel(_instance.RootNode, false, true);
		}
		else if (STPatternManager.GetSymetryMode() == SymetryMode.symetry_2fliped)
		{
			_instance.RootNode.MirrorLevel(_instance.RootNode, true, false);
		}
		else if (STPatternManager.GetSymetryMode() == SymetryMode.symetry_4)
		{
			_instance.RootNode.MirrorLevel(_instance.RootNode, false, false);
			_instance.RootNode.MirrorLevel(_instance.RootNode, false, true);
		}

		collectableNetNodesPos = new List <Vector3> ();

		_instance.RootNode.SetCollectableLastNodes(collectableNetNodesPos);

		_instance.RootNode.UpdateLinkPosition (_instance.RootNode.transform.position);
		//_instance.RootNode.HideSelection();

		_instance.NodesNet.SetActiveNodes (collectableNetNodesPos);

        

	}

	public static void UpdateActiveNodes()
	{
		_instance.NodesNet.Reset();
		collectableNetNodesPos.Clear();

		_instance.RootNode.GetCollectableNodes (collectableNetNodesPos);
		_instance.NodesNet.SetActiveNodes (collectableNetNodesPos);

	}

	public static void Solve ()
	{
		_instance.RootNode.Solve();
	//	_instance.RootNode.SaveChildPositions ();
	//	_instance.RootNode.UpdateLinkPosition (_instance.RootNode.transform.position);
	//	_instance.RootNode.LoadChildPositions ();

		CheckForSolution();

		//STLeьшvel.CalcTreeRects();
	}

	public static void Mix ()
	{
			_instance.RootNode.MixLevel();
	//	_instance.RootNode.SaveChildPositions ();
	//	_instance.RootNode.UpdateLinkPosition (_instance.RootNode.transform.position);
	//	_instance.RootNode.LoadChildPositions ();

			CheckForSolution();
	}

	public static void StartLevel()
	{
		activeTreeNodes.Clear();

		STLevel.GetRootNode().GetTreeNodes(ref activeTreeNodes);
		STLevel.GetRootNode().SetAnimation ();
	}

	public static void CheckForSolution ()
	{
		collectableNetNodesPos.Clear();
		_instance.RootNode.GetCollectableNodes (collectableNetNodesPos);
		_instance.NodesNet.CheckActiveNodesForSolution (collectableNetNodesPos);
	}
	

	public static float GetBranchSize()
	{
		Vector2 brancPos = new Vector2();
		brancPos.x = _instance.RootNode.activeNode.transform.position.x;
		brancPos.y = _instance.RootNode.activeNode.transform.position.y;

		float dist = _instance.RootNode.activeNode.GetMaxDist(brancPos);
		return dist + STNode.nodeSize;
	}

	public static void CalcTreeRects()
	{
		Vector2 minRect = new Vector2 (10000.0f, 10000.0f);
		Vector2 maxRect = new Vector2 (-10000.0f, -10000.0f);

		_instance.RootNode.CalcRects(ref minRect,ref maxRect);

	}

	public static List <STNode> GetTouchedNodesList (Vector3 touchPosition)
	{
		List <STNode> nodesList = new List<STNode> ();
		List <STNode> touchedNodesList = new List<STNode> ();

		STLevel.GetRootNode().GetTouchedNodes (ref nodesList, touchPosition);

		if (nodesList.Count == 0)
		{
			STLevel.GetRootNode().GetTouchedLinkNodes (ref nodesList, touchPosition);

			foreach (STNode node in nodesList)
			{
				if (node.CheckIfLinksTouched (touchPosition) == true)
				{
					touchedNodesList.Add (node);
				}
				//DebugDraw.DrawRect (node.minPoint, node.maxPoint, Color.red);
				//DebugDraw.DrawCrossMark (node.transform.position, 10, Color.cyan);
			}

		}
		else
		{
			foreach (STNode node in nodesList)
			{
				touchedNodesList.Add (node);
				DebugDraw.DrawCrossMark (node.transform.position, 10, Color.green);
			}
		}

		return touchedNodesList;

	}

	public static void HandleOverlayedNodes()
	{

		if (STLevel.GetRootNode().GetActiveNode() == null)
		{
			Debug.Log ("root null");
			return;
		}

	
		STLevel.GetRootNode().ClearOverlayedNodes();


		SetOverlayedNodes (activeTreeNodes);
	}

	static public void HideOverlayedNodes ()
	{
		int SavedNodesCount = 0;
		while ((overlayedNodesList.Count - SavedNodesCount) > 0){
			STOverlayNodes forDel = overlayedNodesList [SavedNodesCount];

		

			if (forDel.DestroyObjects() == false)
			{
				SavedNodesCount ++;
				continue;
			}

			Destroy(forDel.gameObject);
			overlayedNodesList.Remove(forDel);
			
		}
		//overlayedNodesList.Clear();
	}

	static void SetOverlayedNodes (List <STNode> nodesArray)
	{
		for (int i = 0; i < nodesArray.Count - 1; i++)
		{
			bool isOverlayed = false;
			if (nodesArray[i].isOverlayed == true)
			{
				continue;
			}



			STOverlayNodes overlayedNodes = null;
			for (int j = i + 1; j < nodesArray.Count; j++)
			{
				if ((nodesArray[i].NodeState == NODE_ST.SIMPLE) && (nodesArray[j].NodeState == NODE_ST.SIMPLE))
					continue;

				if (Vector2.Distance (nodesArray[i].transform.position, nodesArray[j].transform.position) < 1.0f)
				{
					//Debug.Log (nodesArray[i].transform.position);
					//Debug.Log (nodesArray[j].transform.position);
					if (isOverlayed == false)
					{
						overlayedNodes = Instantiate ( Instance().OverlayInfoPref) as STOverlayNodes;
					//	Debug.Log (nodesArray[i].transform.position);
						nodesArray[i].isOverlayed = true;
						isOverlayed = true;

						//overlayedNodes = new STOverlayNodes(nodesArray[i].transform.position);
						overlayedNodes.AddNode (nodesArray[i],  (nodesArray[i].NodeType == NODE_T.COLLECTABLE));
						overlayedNodesList.Add (overlayedNodes);

					}

					overlayedNodes.AddNode (nodesArray[j],  (nodesArray[j].NodeType == NODE_T.COLLECTABLE));
					nodesArray[j].isOverlayed = true;
				}
			}


		}

		foreach (STOverlayNodes nodeInfo in overlayedNodesList)
		{
			nodeInfo.ShowInfo();
		}
	}

//	static public void UpdateInfoPos ()
//	{
//		foreach (STOverlayNodes nodeInfo in overlayedNodesList)
//		{
//			nodeInfo.UpdatePos();
//		}
//	}

}
