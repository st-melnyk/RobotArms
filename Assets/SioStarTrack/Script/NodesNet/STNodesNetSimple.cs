using UnityEngine;
using System.Collections.Generic;

public class STNodesNetSimple : MonoBehaviour, iNodesNet  {


	
	int elementNetWidth;
	int elementNetHeight;
	
	 float netBtwNodesWidth;
	 STNetNode netNodePref;

	STNetNode [ , ] elementNetNodesArray;
	

	// Use this for initialization

	#region Nodes Edit Net
		
	#endregion

	#region Elements Nodes Net

	public void Refresh()
	{
	}

	public void UpdateActiveNodePosition (Vector3 oldPosition, Vector3 newPosition)
	{
	}
	
		void setElementNodesArray()
	{
		for (int i = 0; i < elementNetHeight; i++)
			for (int j = 0; j < elementNetWidth; j++)
		{
			STNetNode node = Instantiate (netNodePref) as STNetNode;
			node.Net = this;
			elementNetNodesArray[i, j] = node;
		}
	}
	
	public void ShowElementNetNode (Vector3 elementPostion)
	{
		for (int i = 0; i < elementNetHeight; i++)
		for (int j = 0; j < elementNetWidth; j++)
		{
			STNetNode netPoint = elementNetNodesArray[i, j];
			netPoint.renderer.enabled = true;
			netPoint.renderer.collider.enabled = true;
			netPoint.transform.position = new Vector3 ( elementPostion.x + (i - elementNetWidth / 2) * netBtwNodesWidth, elementPostion.y + (j - elementNetHeight / 2) * netBtwNodesWidth, 2 );
		}
	}
	
	public void HideElementNetNode ()
	{
		for (int i = 0; i < elementNetHeight; i++)
		for (int j = 0; j < elementNetWidth; j++)
		{
			STNetNode netPoint = elementNetNodesArray[i, j];
			netPoint.renderer.enabled = false;
			netPoint.renderer.collider.enabled = false;
		}
	}
	
	public Vector3 GetClothestNetNodePos (Vector3 elementPos)
	{
		float minDistance = 1000000.0f;
		Vector3 clothesNetNode = new Vector3(1000, 1000, 1000);
		
		for (int i = 0; i < elementNetHeight; i++)
		for (int j = 0; j < elementNetWidth; j++)
		{
			STNetNode netPoint = elementNetNodesArray[i, j];
			float distBtwPoints = Vector3.Distance (elementPos, netPoint.transform.position) ;
			
			if (distBtwPoints < minDistance)
			{
				minDistance = distBtwPoints; 
				clothesNetNode = netPoint.transform.position;
			}
		}
		
		//Debug.Log ("CLOTHEST NODE - " + clothesNetNode + "THE DISTANCE - " + minDistance);
		
		return clothesNetNode;
	}
	#endregion

	public void BuildWithPrefab (STNetNode nodePrefab, float density, int nElementsNodes) {

		elementNetWidth = nElementsNodes * 2 + 1;
		elementNetHeight = nElementsNodes * 2 + 1;

		netBtwNodesWidth = density;
		netNodePref = nodePrefab;

		elementNetNodesArray = new STNetNode [elementNetWidth, elementNetHeight];

		setElementNodesArray();
	}
	
	void Start()
	{
		
	}
	
	void Update () {
	
	}
	
	public void Clear()
	{
		for (int i = 0; i < elementNetHeight; i++)
		for (int j = 0; j < elementNetWidth; j++)
		{
			STNetNode forDel = elementNetNodesArray[i, j];
			Destroy(forDel.gameObject);
			forDel = null;
		}
	}
	
	public List <Vector3> GetActiveNodesPositions ()
	{
		List <Vector3> nodesPosList = new List<Vector3>();
		return nodesPosList;
	}
	
	public void SetActiveNodes (List <Vector3> activeNodesArray)
	{

	}

	public void ChangeNetNodesState (bool isEnabled)
	{

	}
	
	public List<Vector3> GetGenPositions (STPattern pattern, Vector3 nodePosition, int prevPatternPos, List <int> paternVects, int branchLength)
	{
		return null;
	}

	public void CheckActiveNodesForSolution (List <Vector3> leafPoses)
	{

	}

}
