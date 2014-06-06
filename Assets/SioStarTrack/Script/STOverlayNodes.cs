using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class STOverlayInfo: MonoBehaviour
{
	//public Vector3 position;
	public bool isLeaf;
	public STNode displayNode;
	public GameObject orbitSprite = null;

	public STOverlayInfo (bool isLeafVal, STNode nodeVal, GameObject pref, float angle )
	{
		isLeaf = isLeafVal;
		displayNode = nodeVal;
 

		float time = 3.0f;
		orbitSprite = Instantiate (pref)  as GameObject;
		orbitSprite.transform.Rotate (0, 0, angle);
		orbitSprite.transform.position = displayNode.transform.position;

		if (isLeafVal == true)
		{
			iTween.RotateBy (orbitSprite, iTween.Hash ("amount", new Vector3(0, 0, -1.0f),  "looptype", "loop", "time", time, "easetype", "linear" ));

		}
		else
		{
			iTween.RotateBy (orbitSprite, iTween.Hash ("amount", new Vector3(0, 0, 1.0f),  "looptype", "loop", "time", time, "easetype", "linear" ));
		}

		orbitSprite.renderer.enabled = false;
	}

	public bool Remove (STNode nodeVal)
	{
		if (nodeVal == displayNode)
		{
			RemoveOrbit();
			return true;
		}
		return false;
	}

	public void ShowInfo ()
	{
		orbitSprite.renderer.enabled = true;
	}

	public void RemoveOrbit ()
	{
		Destroy (orbitSprite);
	}
}

public class STOverlayNodes : MonoBehaviour {

	List <STOverlayInfo> overlayedNodes = new List<STOverlayInfo> ();

	public GameObject ActiveLeafSprite;
	public GameObject ActiveNodeSprite;
	

	float angleLeaf = 0;
	float angleNode = 0;

	void Start ()
	{
		ActiveLeafSprite.renderer.enabled = false;
		ActiveNodeSprite.renderer.enabled = false;
	}
	
	public void AddNode (STNode node, bool isLeaf)
	{
		if (isLeaf == true)
		{
			STOverlayInfo obj = new STOverlayInfo (isLeaf, node, ActiveLeafSprite, angleLeaf);
			overlayedNodes.Add (obj);
			//obj.RemoveOrbit();
			angleLeaf += 45;
		}
		else
		{
			STOverlayInfo obj = new STOverlayInfo (isLeaf, node, ActiveNodeSprite, angleNode);
			overlayedNodes.Add (obj);
			//obj.RemoveOrbit();
			angleNode += 45;
		}
	}

	public bool isContainingNode (STNode node)
	{
		foreach (STOverlayInfo info in overlayedNodes)
		{
			if (info.displayNode == node)
				return true;
		}

		return false;
	}

	public void Remove (STNode node)
	{
		foreach (STOverlayInfo info in overlayedNodes)
		{
			if (info.Remove (node))
				break;
		}
	//	overlayedNodes.Clear();
	}

	public void ShowInfo ()
	{
		foreach (STOverlayInfo info in overlayedNodes)
		{
			info.ShowInfo();
		}
	}

//	public void HideInfo ()
//	{
//
//	}



	public bool DestroyObjects ()
	{
		bool doDelete = false;
		foreach (STOverlayInfo info in overlayedNodes)
		{
			if (info.displayNode.NodeState == NODE_ST.SELECTED)
				doDelete = true;
		}

		if (doDelete == false)
			return false;

		foreach (STOverlayInfo info in overlayedNodes)
		{
			info.RemoveOrbit();
		}

		overlayedNodes.Clear();
		Destroy (ActiveLeafSprite);
		Destroy (ActiveNodeSprite);

		return true;
	}

}
