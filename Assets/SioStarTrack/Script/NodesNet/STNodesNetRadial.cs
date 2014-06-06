using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class STNodesNetRadial : STNodesDefaultBehaviour, iNodesNet  {


	int buildNodeN = 0;

	//float maxNodesDist;
	float maxElementNodesDist;
	float fintPointError = 1.0f;


	#region Nodes Edit Net

	
	
	void buildNet (Vector3 nodePos, float maxRadius, ref List<STNetNode> container)
	{
		float angle = 0.0f;
		for (int i = 0; i < 6; i++, angle += 60.0f)
		{
			float posX = nodePos.x + netBtwNodesWidth * Mathf.Cos (angle / 180 * Mathf.PI);
			float posY = nodePos.y + netBtwNodesWidth * Mathf.Sin (angle / 180 * Mathf.PI);
			float posZ = 1;
			
			Vector3 point = new Vector3 (posX, posY, posZ);

			
			//Debug.Log ("POS - " + point);
			Vector3 beginPoint = new Vector3 (0,0,0);
			
			if ((Vector3.Distance (point, beginPoint) > maxRadius) ||  checkForPoint(point, ref container))
			{
				continue;
			}
			else
			{
				
				STNetNode node = Instantiate (netNodePref) as STNetNode;
				node.Net = this;
				
				container.Add (node);
				node.transform.position = point;

				
				buildNodeN ++;
				buildNet(point, maxRadius, ref container);
			}
		}
	}
	
				
	void setElementNodesArray()
	{
		elementNetNodesArray = new List<STNetNode> ();
		
		Vector3 rootNode = new Vector3(0,0,0);
		
		prevElementPostion = new Vector3 (0,0,0);
		
		STNetNode node = Instantiate (netNodePref) as STNetNode;
		node.Net = this;
				
		elementNetNodesArray.Add (node);
		node.transform.position = rootNode;
	
		buildNet (rootNode, maxElementNodesDist, ref elementNetNodesArray);
		
		HideElementNetNode();
		
	}
	
	#endregion
	
	bool checkForPoint (Vector3 pos, ref List <STNetNode> container)
	{
		bool result = false;
		
		foreach (STNetNode netNode in container)
		{
		//	if (Vector3.Distance (point, pos) < fintPointError)
		//		return true;
			Vector3 point = netNode.transform.position;
			if ( (Mathf.Abs(point.x - pos.x) < fintPointError) && (Mathf.Abs(point.y - pos.y) < fintPointError) )
				return true;
		}
		
		return result;
	}
	
	public void BuildWithPrefab (STNetNode nodePrefab, float density,  int nElementsNodes) {
		
		netBtwNodesWidth = density;
		

		maxElementNodesDist = density * nElementsNodes;
		
		netNodePref = nodePrefab;
		setElementNodesArray();
	}
	
	void Update () {
	
	}
	

	


	
	
}
