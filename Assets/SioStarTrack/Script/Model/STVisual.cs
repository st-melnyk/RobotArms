using UnityEngine;
using System.Collections;

public class STVisual : MonoBehaviour {

	public STATE_T currentState;
	public GameObject Node;
	public GameObject Leaf;
	public GameObject ActiveNode;

	public SpriteRenderer lerpObjectSpriteRenderer;

	private Vector3 meshScale;
	private Quaternion meshRotation;
	private Color meshColor;

	float currentLerpPersentage = 0;

	Color aLerp = new Color ();
	Color bLerp = new Color ();

	float lerpFrameTime ;

	float fadeOutTime = 0.5f;
	float fadeInTime = 2.0f;

	float updateLerpTime = 0.03f;

    private float zPos;


	void Start () {
	    zPos = transform.position.z;

        //Debug.Log( zPos );

		ChangeState (STATE_T.simple);
	
	}

	void Update () 
	{

	}

	void updateColor ()
	{
		//Debug.Log (currentLerpPersentage);
		currentLerpPersentage += lerpFrameTime;
		if (currentLerpPersentage > 1.0f)
		{
		//	isFading = false;
			CancelInvoke ("updateColor");
			return;
		}

		float rL = Mathf.Lerp (aLerp.r, bLerp.r, currentLerpPersentage);
		float gL = Mathf.Lerp (aLerp.g, bLerp.g, currentLerpPersentage);
		float bL = Mathf.Lerp (aLerp.b, bLerp.b, currentLerpPersentage);


		lerpObjectSpriteRenderer.color = new Color (rL, gL, bL, 1.0f);
	}


	
	public void Change (NODE_T type)
	{
		switch (type)
		{
			case NODE_T.COLLECTABLE:
			{
				ChangeState (STATE_T.collectable);
				break;
			}
			
			case NODE_T.SIMPLE:
			{
				ChangeState (STATE_T.simple);
				break;
			}

		}
	}

	public void HideRootNodeMark ()
	{
		ActiveNode.renderer.enabled = false;
	}



	public void ChangeState (STATE_T val)
	{

        transform.position = new Vector3(transform.position.x, transform.position.y, zPos);

		Node.renderer.enabled = false;
		Leaf.renderer.enabled = false;
		ActiveNode.renderer.enabled = false;
	
		currentLerpPersentage = 0;

		CancelInvoke ("updateColor");

		switch (val)
		{
		case (STATE_T.simple):
		{
			Node.renderer.enabled = true;

			currentLerpPersentage = 0;

			lerpObjectSpriteRenderer = Node.GetComponent <SpriteRenderer> ();
			aLerp = lerpObjectSpriteRenderer.color;
			bLerp = STLevel.GetCurrentSkin().NodeColor;

			lerpFrameTime =  updateLerpTime / fadeOutTime;
			InvokeRepeating ("updateColor", 0, updateLerpTime);

			break;
		}
			
		case (STATE_T.simpleSelected):
		{
		
			Node.renderer.enabled = true;

			currentLerpPersentage = 0;

			lerpObjectSpriteRenderer = Node.GetComponent <SpriteRenderer> ();
			aLerp = lerpObjectSpriteRenderer.color;
			bLerp = STLevel.GetCurrentSkin().NodeSelectedColor;


			lerpFrameTime =  updateLerpTime / fadeInTime;
			InvokeRepeating ("updateColor", 0, updateLerpTime);

            transform.position = new Vector3(transform.position.x, transform.position.y, zPos - 100);

			break;
		}
			
		case (STATE_T.collectable):
		{
	
		//	iTween.ValueTo (gameObject, iTween.Hash ("from", Leaf.renderer.material.color, "to", STLevel.GetCurrentSkin().LeafColor, "time", 0.2f, "onUpdate", "leafColorChange" ));
			Leaf.renderer.enabled = true;

			currentLerpPersentage = 0;

			lerpObjectSpriteRenderer = Leaf.GetComponent <SpriteRenderer> ();
			aLerp = lerpObjectSpriteRenderer.color;
			bLerp = STLevel.GetCurrentSkin().LeafColor;


			lerpFrameTime =  updateLerpTime / fadeOutTime;

			InvokeRepeating ("updateColor", 0, updateLerpTime);
			
		//	transform.position = new Vector3 (transform.position.x, transform.position.y, zPos);
			break;
		}
			
		case (STATE_T.collectableSelected):
		{
		
		//	iTween.ValueTo (gameObject, iTween.Hash ("from", Leaf.renderer.material.color, "to", STLevel.GetCurrentSkin().LeafSelectedColor, "time", 0.5f, "onUpdate", "leafColorChange" ));
			Leaf.renderer.enabled = true;

			currentLerpPersentage = 0;

			lerpObjectSpriteRenderer = Leaf.GetComponent <SpriteRenderer> ();
			aLerp = lerpObjectSpriteRenderer.color;
			bLerp = STLevel.GetCurrentSkin().LeafSelectedColor;


			lerpFrameTime =  updateLerpTime / fadeInTime;

			InvokeRepeating ("updateColor", 0, updateLerpTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos - 100);
		//	transform.position = new Vector3 (transform.position.x, transform.position.y, -40);
			break;
		}
			
			
		case (STATE_T.rootSelected):
		{
			//iTween.ValueTo (gameObject, iTween.Hash ("from", Node.renderer.material.color, "to", STLevel.GetCurrentSkin().NodeRootSelectedColor, "time", 0.5f, "onUpdate", "nodeColorChange" ));
			ActiveNode.GetComponent<SpriteRenderer>().color = STLevel.GetCurrentSkin().NodeActiveSelectColor;

			ActiveNode.renderer.enabled = true;
			Node.renderer.enabled = true;

			
			currentLerpPersentage = 0;

			lerpObjectSpriteRenderer = Node.GetComponent <SpriteRenderer> ();
			aLerp = lerpObjectSpriteRenderer.color;
			bLerp = STLevel.GetCurrentSkin().NodeRootSelectedColor;


			lerpFrameTime =  updateLerpTime / fadeInTime;

			InvokeRepeating ("updateColor", 0, updateLerpTime);

           // Debug.Log (zPos - 100);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos - 100);
			break;
		}
			
		}
	}


}


