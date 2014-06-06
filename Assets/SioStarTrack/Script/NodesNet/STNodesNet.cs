using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum NET_T{
	SIMPLE,
	RADIAL,
	CIRCLE,
}


public class STNodesNet : MonoBehaviour {
	iNodesNet iNet;
	STNodesNetRadial netRadial;
	STNodesNetSimple netSimple;
	STNodesNetCircle netCircle;
	
	public STNetNode NetNodePref;
	public NET_T NetworkType;
	public float NetworkNodesDistance;
	//public int NetworkNodesCount;
	public int NetworkElementNodesCount;
	
	//public STNode showNetNode;
	
	void Start ()
	{
		if (NetworkType == NET_T.RADIAL)
		{
			netRadial = new STNodesNetRadial();
			iNet = netRadial;
		
		}
		else if (NetworkType == NET_T.SIMPLE)
		{
			netSimple = new STNodesNetSimple ();
			iNet = netSimple;
			
		}
		else if (NetworkType == NET_T.CIRCLE)
		{
			netCircle = new STNodesNetCircle ();
			iNet = netCircle;
			
		}
		else
			return;


		iNet.BuildWithPrefab (NetNodePref, NetworkNodesDistance, NetworkElementNodesCount);

	}


	
	
	public void ChangeNetNodesState (bool isEnabled)
	{
		iNet.ChangeNetNodesState (isEnabled);
	}
	
	public void ShowElementNetNode (Vector3 elementPostion)
	{
		//if (NetworkType == NET_T.CIRCLE)
		//{
		//	if (showNetNode != null)
		//		iNet.ShowElementNetNode (showNetNode.transform.position);
			//else
		//		iNet.ShowElementNetNode (elementPostion);
	//	}
		//else
			iNet.ShowElementNetNode (elementPostion);
	}

	public void RefreshNet ()
	{
		iNet.Refresh();
	}
	
	public void HideElementNetNode ()
	{
		iNet.HideElementNetNode();
	}
	
	public Vector3 GetClothestNetNodePos (Vector3 elementPos)
	{
		return iNet.GetClothestNetNodePos(elementPos);
	}
	
	public float NetBtwNodesWidth()
	{
		return NetworkNodesDistance;
	}
	
	public STSerializedNet GetSerializedObject()
	{
		STSerializedNet sNet = new STSerializedNet();
		
		sNet.NetworkNodesDistance = NetworkNodesDistance;
		
		if (NetworkType == NET_T.SIMPLE)
		{
			sNet.NetworkType = 0;
		}
		else if (NetworkType == NET_T.RADIAL)
		{
			sNet.NetworkType = 1;
		}
		else if (NetworkType == NET_T.CIRCLE)
		{
			sNet.NetworkType = 2;
		}
	
		
		List <Vector3> activePointsList = iNet.GetActiveNodesPositions ();
		
		
		
		int listLen = activePointsList.Count;
		sNet.activeNodePos = new STSerializableVector3 [listLen] ;
			//sObject.children = new STSerializedNode[mRotor.mChilds.Count];
	
		
		int i = 0;
		Debug.Log ("NET POINTS" + activePointsList);
		Debug.Log ("OUT SLEVEL NET POS - " + sNet.activeNodePos);
		foreach (Vector3 point in activePointsList)
		{
			//sNet.activeNodePos.Add (new STSerializableVector3 (point));
			Debug.Log ("ADDNODE");
			sNet.activeNodePos [i] = new STSerializableVector3();
			sNet.activeNodePos [i].X = point.x;
			sNet.activeNodePos [i].Y = point.y;
			sNet.activeNodePos [i].Z = point.z;
			i++;
		}
		
		
		return sNet;
	}
	
	public void LoadNet (STSerializedNet net) 
	{
		NetworkNodesDistance = net.NetworkNodesDistance;
		
		if (net.NetworkType == 0)
		{
			NetworkType = NET_T.SIMPLE;
		}
		else if (net.NetworkType == 1)
		{
			NetworkType = NET_T.RADIAL;
		}
		else if (net.NetworkType == 2)
		{
			NetworkType = NET_T.CIRCLE;
		}

		
		List <Vector3> activePointsList = new List<Vector3> ();
		
		
		
		if (net.activeNodePos != null)
		foreach (STSerializableVector3 point in net.activeNodePos)
		{
			activePointsList.Add(new Vector3(point.X, point.Y, point.Z));
		}
		
		this.Reset ();
		this.Start ();
		iNet.SetActiveNodes (activePointsList);

	}

	public void SetActiveNodes (List <Vector3> activeNodesArray)
	{
	
		iNet.SetActiveNodes (activeNodesArray);
	}
	
	public void Reset ()
	{
		iNet.Clear();
		//netRadial = null;
		//netSimple = null;
	}

	public void UpdateActiveNodePosition (Vector3 oldPosition, Vector3 newPosition)
	{
		iNet.UpdateActiveNodePosition (oldPosition, newPosition);
	}


	
	public  List <Vector3> GetGenPositions (STPattern pattern, Vector3 nodePosition, int prevPatternPos, List <int> paternVects, int branchLength)
	{
		//return iNet.GetGenPoint();

		
		return iNet.GetGenPositions (pattern, nodePosition, prevPatternPos, paternVects, branchLength);
	}

	public void CheckActiveNodesForSolution (List <Vector3> leafPoses)
	{
		iNet.CheckActiveNodesForSolution (leafPoses);
	}
	
}
