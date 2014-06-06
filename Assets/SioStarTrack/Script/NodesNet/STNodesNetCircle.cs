using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STNodesNetCircle : STNodesDefaultBehaviour, iNodesNet {

	public void BuildWithPrefab (STNetNode nodePrefab, float density,  int nElementsNodes)
	{
		netBtwNodesWidth = density;
		
	//	maxNodesDist = density * nNodes;
	//	maxElementNodesDist = density * nElementsNodes;
		prevElementPostion = Vector3.zero;
		 elementNodesCount = nElementsNodes;
		
		netNodePref = nodePrefab;
		activeNodes = new List<STNetNode> ();
		setElementNodesArray();
	}
	

	
	void setElementNodesArray()
	{
		elementNetNodesArray = new List<STNetNode> ();
		buildNet (elementNodesCount, ref elementNetNodesArray);
		
		HideElementNetNode();
		
	}
	
	void buildNet (int elementCount, ref List<STNetNode> container)
	{
		STNetNode node = Instantiate (netNodePref) as STNetNode;
		node.Net = this;
		node.transform.position = Vector3.zero;
		node.transform.parent = STLevel.GetNodesNet().transform;
		container.Add (node);
		
		float angle = 45.0f;
		float currentAngle = 0.0f;

			
			for (int i = 1; i <= elementCount; i++)
			{
			currentAngle = 0.0f;
				do 
				{
				
				
				//	Debug.Log (node.transform.position);
					float posX = i * netBtwNodesWidth * Mathf.Cos (currentAngle / 180 * Mathf.PI);
					float posY = i * netBtwNodesWidth * Mathf.Sin (currentAngle / 180 * Mathf.PI);
					float posZ = 1;
				
					Vector3 point = new Vector3 (posX, posY, posZ);	
				
					node = Instantiate (netNodePref) as STNetNode;
					node.Net = this;
				
					container.Add (node);
					node.transform.position = point;

					node.transform.parent = STLevel.GetNodesNet().transform;
				
					currentAngle += angle;
				} while (currentAngle < 360.0f );
				//angle /= 2.0f;
			}
		
	}
}
