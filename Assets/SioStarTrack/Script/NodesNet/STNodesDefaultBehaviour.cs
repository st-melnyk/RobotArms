using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STNodesDefaultBehaviour : MonoBehaviour {
	
	protected	 STNetNode netNodePref;
	
	protected float netBtwNodesWidth ;
	
	protected List <STNetNode> elementNetNodesArray;
	protected List <STNetNode> activeNodes;
	protected Vector3 prevElementPostion;
	//float maxNodesDist;
	//float maxElementNodesDist;
	
	//protected int nodesCount;
	protected int elementNodesCount;

	public void UpdateActiveNodePosition (Vector3 oldPosition, Vector3 newPosition)
	{
		foreach (STNetNode netNode in activeNodes)
		{
			if (Vector3.Distance (netNode.transform.position, oldPosition) < 1.0f)
			{
				netNode.transform.position = newPosition;
				return;
			}
		}
	}
	
	public void ChangeNetNodesState (bool isEnabled)
	{

	}
	
	public void ShowElementNetNode (Vector3 elementPostion)
	{
			
		foreach (STNetNode netNode in elementNetNodesArray)
		{
			netNode.transform.position += elementPostion - prevElementPostion;	
			netNode.SetActive (true);
		}
		
				
		prevElementPostion.x = elementPostion.x;
		prevElementPostion.y = elementPostion.y;
		prevElementPostion.z = elementPostion.z;
	}
	
	public void HideElementNetNode ()
	{
		foreach (STNetNode netNode in elementNetNodesArray)
		{
			netNode.SetActive (false);
//			netNode.nodeSprite.renderer.collider.enabled = false;
		}	
	}
	
	public Vector3 GetClothestNetNodePos (Vector3 elementPos)
	{
		float minDistance = 1000000.0f;
		STNetNode minDistNode = null;
		
		foreach (STNetNode netNode in elementNetNodesArray)
		{
			float curDist = Vector3.Distance(netNode.transform.position, elementPos) ;
			if ( curDist < minDistance)
			{
				minDistance = curDist;
				minDistNode = netNode;
			}
		}
		
		if (minDistNode != null)
			return minDistNode.transform.position;	
		else
			return elementPos;
	}
	
	public void Clear()
	{
		//Debug.Log ("CLEAR");
//		while(elementNetNodesArray.Count > 0){
//			STNetNode forDel = elementNetNodesArray[0];
//			Destroy(forDel.gameObject);
//			elementNetNodesArray.Remove(forDel);
//			
//		}

		//prevElementPostion = Vector3.zero;

		while(activeNodes.Count > 0)
		{
			STNetNode forDel = activeNodes[0];
			Destroy(forDel.gameObject);
			activeNodes.Remove (forDel);
		}

	}
	
	public List <Vector3> GetActiveNodesPositions ()
	{
		List <Vector3> nodesPosList = new List<Vector3>();

		foreach (STNetNode netNode in activeNodes)
		{
			nodesPosList.Add (netNode.transform.position);
		}
		
		return nodesPosList;
	}
	
		
	public void SetActiveNodes (List <Vector3> activeNodesArray)
	{
	
		//Debug.Log ("Active");
		foreach (Vector3 nodePos in activeNodesArray)
		{
			STNetNode node = Instantiate (netNodePref) as STNetNode;

			activeNodes.Add (node);

			Vector3 thePos = nodePos;
			thePos.z = 50;

			node.transform.position = thePos;

			node.ChangeType (NET_NODE_T.STAR);
			node.SetActive (true);
		}
	}

	public void Refresh ()
	{
		foreach (STNetNode netNode in activeNodes)
		{
			netNode.SetActive (netNode.isActive);
		}
	}
	

	public List<Vector3> GetGenPositions (STPattern pattern, Vector3 nodePosition, int prevPatternPos, List <int> paternVects, int branchLength)
	{
		foreach (STNetNode netNode in elementNetNodesArray)
		{
			netNode.transform.position += nodePosition - prevElementPostion;	
		}

		prevElementPostion.x = nodePosition.x;
		prevElementPostion.y = nodePosition.y;
		prevElementPostion.z = nodePosition.z;
		
		Vector3 genPos = new Vector3 ();
		
		
		List <Vector3> genPoints = new List<Vector3> ();
		//Debug.Log ("GEN");
		
		
		if (STPatternManager.GetMode() == GenMode.patternMode)
		{
			List <int> verts = pattern.GetVertices ();
		if (verts != null)
		{
			foreach (int vert in verts)
			{
				int circleN = (vert - 1) / 8;
				int prevPatternCircle = (prevPatternPos - 1) % 8;
				int posInCircle = ((vert - 1) % 8 + prevPatternCircle) % 8;

				genPos = elementNetNodesArray [circleN * 8 + posInCircle + 1 ].transform.position;
				genPoints.Add (genPos);
				paternVects.Add (circleN * 8 + posInCircle + 1);
			}
		}
		}
		else
		{
		
			int randN;
			int nBranches = (int) ( Random.value * (pattern.maxBranches - pattern.minBranches)) + pattern.minBranches;
		//	int nBranches = 1;
			int genCount = 0;
			int totalGenCount = 0;

			do
			{
				bool goodVect = false;
				//int index;


			do
			{
					int minLength = pattern.minLength;
					int maxLength = pattern.maxLength;

			//		Debug.Log ("MIN" + minLength);
			//		Debug.Log ("MAX" + maxLength);

				
			//	randN = (int) (Random.value * 23) + 1;
					randN = (int) ((minLength - 1) * 8 + Mathf.Round( Random.value * (maxLength - minLength + 1) * 8.0f))  ;

					if (randN == 0) 
						continue;

					//Debug.Log (randN);
					
			//	int circleN = (randN - 1) / 8;	
			//	int prevPatternCircle = (prevPatternPos - 1) % 8;
			//	int posInCircle = ((randN - 1) % 8 + prevPatternCircle) % 8;
					
			//	index = circleN * 8 + posInCircle + 1;
				genPos = elementNetNodesArray [randN].transform.position;


					bool checkGenPos = true;



					SymetryMode mode = STPatternManager.GetSymetryMode();

					if (mode == SymetryMode.symetry_4)
					{
						if ((genPos.x <= STNode.nodeSize) || (genPos.y <= STNode.nodeSize))
							checkGenPos = false;

					}
					else if ((mode == SymetryMode.symetry_2) || (mode == SymetryMode.symetry_2fliped))
					{
						if (genPos.y <= STNode.nodeSize)
						{
							checkGenPos = false;

						//	Debug.Log ("GEN POS ERR");
//							Debug.Log (genPos);
						}
					}


					if ((Mathf.Abs ( genPos.y + STNode.nodeSize) > Camera.main.orthographicSize) || (Mathf.Abs (genPos.x + STNode.nodeSize) > Camera.main.orthographicSize))
					{
						checkGenPos = false;
//						Debug.Log ("OUT OF SCREEN");
					}

//					if (branchLength + ( randN - 1) / 8 + 1 > 6)
//					{
//						checkGenPos = false;
//						goodVect = false;
//					}


					if (((prevPatternPos == 0) || (Mathf.Abs(randN - prevPatternPos) % 8 != 4)) && (checkGenPos))
				{
						goodVect = true;
					foreach (int pos in paternVects)
						{
							if ((randN % 8) == (pos % 8))
							{
								goodVect = false;
							//	Debug.Log ("PREV GEN POS ERR");
							}
						}
				}
					else
					{
						//Debug.Log ("BACK GEN ERR");
					}


					genCount ++;
					if (genCount > 30)
					{

	//					Debug.Log ("FAIL 1" + genCount);
						break;
					}
					
				
			} while (goodVect == false);




	

			//	if (goodVect == true)
				if (STLevel.GetRootNode().CheckForIntersection(genPos.x, genPos.y, nodePosition.x, nodePosition.y, 0, 0) == true)
				{
					goodVect = false;
				}

				totalGenCount += genCount;
				genCount = 0;
				if (totalGenCount > 300)
				{
//					Debug.Log ("FAIL 2" + totalGenCount);
					goodVect = false;
				}
				else
					if (goodVect == false)
						continue;
				
				//Debug.Log ("PREV - " + prevPatternPos);
			//	Debug.Log (randN);
			//	Debug.Log (genPos);

				if (goodVect == true)
				{
					genPoints.Add (genPos );
					paternVects.Add (randN);
				}
				//else
					//Debug.Log ("FAIL");

				
				totalGenCount = 0;
				nBranches --;
			}
			while (nBranches > 0);
			
			//paternVects = verts;
		}
		
		return genPoints;
	}

	public void CheckActiveNodesForSolution (List <Vector3> leafPoses)
	{
		foreach (STNetNode netNode in activeNodes)
		{
		//	Debug.Log (netNode.transform.position);
			foreach (Vector3 leafPos in leafPoses)
			{

				if ((Mathf.Abs(netNode.transform.position.x - leafPos.x) < 2.0) && (Mathf.Abs(netNode.transform.position.y - leafPos.y) < 2.0))
				{
					netNode.ChangeType (NET_NODE_T.PULSAR);
					netNode.SetActive (true);
					//Debug.Log ("STAR");

					break;
				}
				else
				{
					netNode.ChangeType (NET_NODE_T.STAR);
					netNode.SetActive (true);

				//	Debug.Log ("PULSAR");
				}
			}
		}
	}

}
