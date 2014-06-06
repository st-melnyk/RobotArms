using UnityEngine;
using System.Collections;

public enum NET_NODE_T{
	STAR,
	PULSAR,
	NET_NODE,
}

public class STNetNode : MonoBehaviour {
	
	NET_NODE_T currentType = NET_NODE_T.NET_NODE;
	public iNodesNet Net;
	
	public bool isActive = false;
	public GameObject label;
	public GameObject starSprite;
	public GameObject pulsarSprite;
	public GameObject netNodeSprite;



	void Start () {
		starSprite.GetComponent<SpriteRenderer>().color = STLevel.GetCurrentSkin().StarColor;
		pulsarSprite.GetComponent<SpriteRenderer>().color = STLevel.GetCurrentSkin().PulsarColor;

	}

	public void SetActive (bool isActiveState)
	{
		isActive = isActiveState;

		starSprite.renderer.enabled = false;
		pulsarSprite.renderer.enabled = false;
		netNodeSprite.renderer.enabled = false;

		if (isActive == true)
		{
			if (currentType == NET_NODE_T.STAR)
			{
				starSprite.renderer.enabled = true;
			}
			else if (currentType == NET_NODE_T.PULSAR)
				pulsarSprite.renderer.enabled = true;
			else if (currentType == NET_NODE_T.NET_NODE)
				netNodeSprite.renderer.enabled = true;
		}
	}

	public void ChangeType (NET_NODE_T type)
	{
		currentType = type;
	}
}
