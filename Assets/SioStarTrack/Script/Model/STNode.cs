
//#define ANIM_PNAME_LINKLENGTH "LinkLength"
//#define ANIM_PNAME_ANIMSTATE "AnimState"



using UnityEngine;
using System.Collections;

using System.Collections.Generic;



public enum NODE_ST{
	SIMPLE,
	SELECTED,
}





public class STNode : MonoBehaviour, iListener {

	const string ANIM_PNAME_LINKLENGTH = "LinkLength";
	const string ANIM_PNAME_ANIMSTATE  = "AnimState";

//	bool isStartedHide = false;
//	bool isStartedSelect = false;
//	bool isStartedScale = false;


    public AnimationClip animClipPlaying;

	bool isSelected = false;
	bool isSelectedActive = false;
	//bool isPrevSelectedActive = false;
	//public Animator mAnimator;
	static public int nodeSize = 50;
	public Vector2 minPoint = new Vector2();
	public Vector2 maxPoint = new Vector2();
	public NODE_ST NodeState = NODE_ST.SIMPLE;
	public STNode mParent;
	int leavesAnimationFinishedCount = 0;
	public Vector3 blinkPosition;

    int animClip = 0;
	public STRotor mRotor;
	public STVisual mVisual;
	public STLink mLink;
	public STNodeUI mUI;
	
	public Vector3 savePos = new Vector3 ();
	//float NetWidth;
	public NODE_T NodeType;

	public Vector3 solvedRotation = new Vector3 ();

	Vector3 mouseStartPos = new Vector3();
	private
		float angleAdd;
		int distAdd = 1;

	protected
		float zCoordLink = 0;
	//	float zCoordNode = 0;

	protected
		int nodeDepth = 0;
	public	
		int brenchLength = 0;
	
	public bool isOverlayed = false;

  

	float zCoord;
	
	#region iListener
	void iListener.OnEvent(EVENT_T t, float val, iListener ch)
	{
		switch(t){
			case(EVENT_T.ON_ADD):{
			distAdd = 1;
			angleAdd = 0;

				STRootNode root = STLevel.GetRootNode();
			
				if ( root != null )
			{
				STNode node = Instantiate(root.NodePref) as STNode;
					if ( node != null ){
		
						node.transform.parent = mRotor.transform;

					do
					{
						if (distAdd > 5)
							break;

					if (angleAdd >= 360)
					{
						angleAdd = 0;
						distAdd ++;
					}

						Vector3 transformPos = new Vector3(STLevel.GetNodesNet().NetBtwNodesWidth() * Mathf.Cos(angleAdd / 180 * Mathf.PI) * distAdd, STLevel.GetNodesNet().NetBtwNodesWidth() * Mathf.Sin(angleAdd / 180 * Mathf.PI) * distAdd, 0 );
						node.transform.localPosition = mRotor.transform.localPosition + transformPos;
						
						angleAdd += 45;
					
						node.transform.position = STLevel.GetNodesNet().GetClothestNetNodePos(node.transform.position);

						Debug.Log (node.transform.position);

					}while(STLevel.GetRootNode().CheckForIntersection (node.transform.position.x, node.transform.position.y, transform.position.x, transform.position.y, 0, 0) == true);
					  
					node.transform.localRotation = mRotor.transform.localRotation;
					node.mParent = this;
					mRotor.mChilds.Add(node);

					STLevel.GetNodesNet().ShowElementNetNode (transform.position);
					STLevel.GetNodesNet().HideElementNetNode();
					//node.mLink.UpdatePos (transform.position);


					Vector3 thepos = STLevel.GetNodesNet().GetClothestNetNodePos(node.transform.position);
					node.transform.position = new Vector3 (thepos.x, thepos.y, node.transform.position.z);
					node.UpdateLinkPosition(node.transform.position);

					showBranchSelection();

					}
				}
				break;
			}
			
			case(EVENT_T.ON_REMOVE):
			{
				if ( mParent != null ){
					Destroy (this.gameObject);
					mParent.mRotor.mChilds.Remove (this);
				}

				break;
			}


		case (EVENT_T.ON_CHANGE):
		{
			this.changeNodeType();
			STLevel.UpdateActiveNodes ();
			break;
		}
		}

	}
	#endregion


	public void onMove ()
	{
		Vector3 old_pos = transform.position;
		Vector3 mouse_pos = Input.mousePosition;
		mouse_pos = Camera.main.ScreenToWorldPoint(mouse_pos);
		mouse_pos.z = old_pos.z;
		this.transform.position = mouse_pos - mouseStartPos;

     

	//	SaveChildPositions ();
		this.UpdateLinkPosition (this.transform.position);
	//	LoadChildPositions ();
		//transform.eulerAngles = prevAngle;

	}

	public void onMouseDown ()
	{

        if (STLevel.isEditMode == false)
        {
           
            return;
        }
	//	showBranchSelection();

		STLevel.GetRootNode().HideAllGUIS();
		mUI.ShowGUI(true);
//
		if (mParent != null)
			STLevel.GetNodesNet().ShowElementNetNode(mParent.transform.position);
		else
			STLevel.GetNodesNet().ShowElementNetNode(this.transform.position);



	}

	public void onMousePressed ()
	{

        
        if (STLevel.isEditMode == false)
        {
            showBranchSelection();
            return;
        }

		Vector3 mouse_pos = Input.mousePosition;
		Vector3 pos =  Camera.main.ScreenToWorldPoint(mouse_pos) - this.transform.position;
		mouseStartPos.x = pos.x;
		mouseStartPos.y = pos.y;
		mouseStartPos.z = 0;



	//	showBranchSelection();
	
//
		STLevel.GetNodesNet().HideElementNetNode();
//		
	    Vector3 thepos = STLevel.GetNodesNet().GetClothestNetNodePos(this.transform.position);
		this.transform.position = new Vector3 (thepos.x, thepos.y, transform.position.z);
//
		this.UpdateLinkPosition (this.transform.position);
		STLevel.UpdateActiveNodes();

	}

	bool setActiveNode ()
	{
		if (mRotor.mChilds.Count > 0)
		{
            if ((STLevel.GetRootNode().activeNode != null) && (this != STLevel.GetRootNode().activeNode))
                STLevel.GetRootNode().activeNode.mVisual.HideRootNodeMark();

			STLevel.GetRootNode().SetActiveNode(this);
		}
		else
		{
			if (mParent != null)
			{
                if ((STLevel.GetRootNode().activeNode != null) && (mParent != STLevel.GetRootNode().activeNode))
                    STLevel.GetRootNode().activeNode.mVisual.HideRootNodeMark();

				STLevel.GetRootNode().SetActiveNode(mParent);
			}
			else
			{
                if ((STLevel.GetRootNode().activeNode != null) && (this != STLevel.GetRootNode().activeNode))
                    STLevel.GetRootNode().activeNode.mVisual.HideRootNodeMark();

				STLevel.GetRootNode().SetActiveNode(this);
			}
		}


		if (STLevel.GetRootNode().activeNode != null)
		{
			//Debug.Log (STLevel.GetRootNode().activeNode.transform.position);
			if (STLevel.GetRootNode().prevActiveNode != null)
			{
				//	Debug.Log (STLevel.GetRootNode().prevActiveNode.transform.position);
				if (STLevel.GetRootNode().activeNode == STLevel.GetRootNode().prevActiveNode)
				{
					return false;
				}
			}
		}

		if (STLevel.GetRootNode().prevActiveNode != null)
		{
			STNode prevActiveNode = STLevel.GetRootNode().prevActiveNode;
			prevActiveNode.transform.localPosition = new Vector3 (prevActiveNode.transform.localPosition.x, prevActiveNode.transform.localPosition.y, prevActiveNode.zCoord);
	
		}



		STLevel.GetRootNode().prevAnimActiveNode = STLevel.GetRootNode().prevActiveNode;
		STLevel.GetRootNode().prevActiveNode = STLevel.GetRootNode().activeNode;

		return true;

	
	}

	void showBranchSelection ()
	{
        
       // Debug.Log( "SHOW BRANCH" );
	    if ( setActiveNode() == false ) {
	      //  Debug.Log( "ACTIVE - FALSE" );
            return;
	    }
			


		hideBranchSelection();
		//StartAnimator();

		STNode activeNode = STLevel.GetRootNode().activeNode;
		activeNode.zCoord = activeNode.transform.localPosition.z;

		STLevel.GetRootNode().showAnimationsCount = 0;

       
	    

		if (mRotor.mChilds.Count > 0)
		{
			showBranchSelection (this);
		}
		else
		{
			if (mParent != null)
			{
				showBranchSelection (mParent);
			}
			else
			{
				showBranchSelection (this);
			}
		}

       // STLevel.GetRootNode().prevActiveNode.mVisual.HideRootNodeMark();
	}

	void hideBranchSelection ()
	{
		STLevel.GetRootNode().hideAnimationsCount = 0;
       
	    ;
//		STLevel.GetRootNode().finishSelectAnimationsCount = 0;
		setActiveSelectionLeafesFlags ();

		STLevel.GetRootNode().startHideAnimationFromLeafes();

        if (STLevel.GetRootNode().activeNode != null)
        if (STLevel.GetRootNode().activeNode.isSelected == true)
            STLevel.GetRootNode().activeNode.setAnimatorForUnselectState();
	}

	public void SetAnimation ()
	{
		if (mParent != null)
		{

			float linkLength = mLink.GetLength (mParent.transform.position, transform.position);
		//	Debug.Log (linkLength);
			
	//		float koef = 8.0f;
			if (linkLength < 130)
			{
				//mAnimator.speed = 1.0f * koef;
				animClip = 1;
				//nodeAnimationType = NODE_TYPE_ANIM.NODE1;
			}
			else if (linkLength < 250)
			{
				//mAnimator.speed = 0.75f * koef;
				animClip = 2;
				//nodeAnimationType = NODE_TYPE_ANIM.NODE2;
			}
			else if (linkLength < 360)
			{
				//mAnimator.speed = 0.5f * koef;
				animClip = 3;
				//nodeAnimationType = NODE_TYPE_ANIM.NODE3;
			}
			else if (linkLength < 480)
			{
				//mAnimator.speed = 0.25f * koef;
				animClip = 4;
				//nodeAnimationType = NODE_TYPE_ANIM.NODE4;
			}

			//mAnimator.SetInteger (ANIM_PNAME_ANIMSTATE, -1);
			//mAnimator.SetInteger (ANIM_PNAME_LINKLENGTH, animClip);
			mLink.transform.localPosition = new Vector3 (0, linkLength - nodeSize / 2.2f, 1);

		    float animSpeed = 1.0f;

           
		    float unselectAnimSpeed = animSpeed * (STLevel.GetRootNode().treeDepth - nodeDepth + 1);
            float selectAnimSpeed = animSpeed * nodeDepth;

            animation["Unselect1"].speed = unselectAnimSpeed;
            animation["Unselect2"].speed = unselectAnimSpeed;
            animation["Unselect3"].speed = unselectAnimSpeed;
            animation["Unselect4"].speed = unselectAnimSpeed;

            animation["Select1"].speed = selectAnimSpeed;
            animation["Select2"].speed = selectAnimSpeed;
            animation["Select3"].speed = selectAnimSpeed;
            animation["Select4"].speed = selectAnimSpeed;
		    
	
		}

		foreach (STNode node in mRotor.mChilds)
		{
			node.SetAnimation();
		}
	}



	void showBranchSelection (STNode startNode)
	{
     //   Debug.Log( "show branch selection" );
		STLevel.GetRootNode().isShowingBranchAnimation = false;
		//if (mRotor.mChilds.Count != 0)
		//{

	//	if (startNode.isSelected == true)
	//	{
	//		//startNode.showAllBranchesAnimation();

		//	StartCoroutine(showBranchSelectionWithDelay(true));
		    showBranchSelectionWithDelay( true );
			//Debug.Log ("SELECTED");     
		//	return;
		//}

			startNode.showTreeSelection(startNode, true);
	}


	void showTreeSelection (STNode startNode, bool isRootNode)
	{
      //  Debug.Log( "SHOW TREE SELECTION" );
		if (isRootNode == false)
			showNodeSelection();
		else
			mVisual.ChangeState (STATE_T.rootSelected);

		foreach (STNode node in startNode.mRotor.mChilds)
			if (node.isSelected == false)
			{
				node.setAnimationForSelectState();
			}

	}

    string getSelectAnimationName( int linkLength ) {
        switch ( linkLength ) {
            case 1:
                return "Select1";

            case 2:
                return "Select2";

            case 3:
                return "Select3";

            case 4:
                return "Select4";

            default:
                return null;
                
        }
    }

    string getUnSelectAnimationName(int linkLength)
    {
        switch (linkLength)
        {
            case 1:
                return "Unselect1";

            case 2:
                return "Unselect2";

            case 3:
                return "Unselect3";

            case 4:
                return "Unselect4";

            default:
                return null;

        }
    }
	
	public void RemoveLinkToNode(STNode val){
		if ( val != null ){
			mRotor.mChilds.Remove(val);
			val.RemoveChild();
			Destroy(val);
		}
	}


	void setAnimationForSelectState ()
	{
        animation.wrapMode = WrapMode.Once;
        animation.Play(this.getSelectAnimationName( animClip ));
	}

	void setAnimatorForUnselectState()
	{
       // animation.wrapMode = WrapMode.Once;
       
        if (animation != null)
          animation.Play (this.getUnSelectAnimationName( animClip ));

	}





	void startSelectAnimation ()
	{
	//	mLink.lineSelSprite.transform.localScale = animationNodeScale;
		//if (mRotor.mChilds.Count > 0)
       // Debug.Log( "Start select" );
		STLevel.GetRootNode().showAnimationsCount ++;
		isSelected = true;
        if (mParent != null)
		mParent.leavesAnimationFinishedCount = 0;
	}

	void finishSelectAnimation ()
	{
		showTreeSelection(this, false);
		
		STLevel.GetRootNode().showAnimationsCount --;

	
		STLevel.GetRootNode().activeNodesCount --;

//		Debug.Log (STLevel.GetRootNode().activeNodesCount);

		if ((STLevel.GetRootNode().activeNodesCount == 1) && (mRotor.mChilds.Count == 0))
		{
			//StartCoroutine(showBranchSelectionWithDelay(false));
		    showBranchSelectionWithDelay( false );
		}

		StartCoroutine (turnOffAnimatorWithDelay());

	}


	IEnumerator turnOffAnimatorWithDelay() {
		yield return new WaitForSeconds(0.1f);
		//StopAndIdleAnimator ();
		
	}

void showBranchSelectionWithDelay(bool isSelected) {
	//yield return new WaitForSeconds(0.2f);
		startShowAllBranchesAnimation (isSelected);
	
}


void startShowAllBranchesAnimation (bool isSelected)
	{
//		Debug.Log (STLevel.GetRootNode().showAnimationsCount);

		if (STLevel.GetRootNode().isShowingBranchAnimation == true)
		{
			//Debug.Log ("IS SHOWING");
			return;
		}

		if ((STLevel.GetRootNode().showAnimationsCount == 0) || (isSelected == true))
			//if (true)
		{
			STLevel.GetRootNode().isShowingBranchAnimation = true;
 			//Debug.Log ("SHOW");
			if (STLevel.GetRootNode().activeNode != null)
			{
				STLevel.GetRootNode().scaleAnimationsCount = 0;
				STLevel.GetRootNode().activeNode.showAllBranchesAnimation();
			}
		}
	}

	void showAllBranchesAnimation ()
	{
	//	setAnimatorForShowAllBranchesState();
	//	setAnimationForSelectState ();
		foreach (STNode node in mRotor.mChilds)
		{
			node.showAllBranchesAnimation();
			//node.setAnimatorForShowAllBranchesState();
		}
	}

	void startHideSelectAnimation ()
	{
	//	mLink.lineSelSprite.transform.localScale = animationNodeScale;
		isSelected = false;
		STLevel.GetRootNode().hideAnimationsCount ++;
	}

	void finishHideSelectAnimation ()
	{
		STLevel.GetRootNode().hideAnimationsCount --;

       // if (isUnselectFromNode = true)
		    mParent.hideSelectAnimationFinishedFromLeaf();
       // else


	
	}

	void finishScaleAnimation ()
	{
		setAnimationForSelectState();


		STLevel.GetRootNode().scaleAnimationsCount --;
	//	checkForFinishAnimation();
		StartCoroutine (turnOffAnimatorWithDelay());
	//	setAnimatorForIdleState();
	//	animationNodeScale = mLink.lineSelSprite.transform.localScale;
	}

	

	void startScaleAnimation ()
	{
		STLevel.GetRootNode().scaleAnimationsCount ++;
		//	animationNodeScale = mLink.lineSelSprite.transform.localScale;
	}

	void hideSelectAnimationFinishedFromLeaf ()
	{
		leavesAnimationFinishedCount ++;

		//transform.position = new Vector3 (transform.position.x, transform.position.y, - nodeDepth );

		//	Debug.Log (transform.position);
		//	Debug.Log (STLevel.GetRootNode().prevActiveNode.transform.position);
		if (this == STLevel.GetRootNode().prevAnimActiveNode)
		{
			if (STLevel.GetRootNode().hideAnimationsCount == 0)
			{
                
				hideNodeSelection();
			}
		   
//			Debug.Log ("PAA");
		}

      //  Debug.Log("hide");
		if (leavesAnimationFinishedCount == mRotor.mChilds.Count)
		{
            if (isSelected == true)
			{
				//Debug.Log ("hide");
				setAnimatorForUnselectState();
              //  Debug.Log("hideSelectAnimationFinishedFromLeaf - 1");
				hideNodeSelection ();
			}
		}

	}

	void Awake()
	{
		mUI.mListener = (this as iListener);
	}
	
	void Start () {

		if ( mRotor == null ){
			Debug.LogError("Rotor error");
			return;
		}
		
		if ( mVisual == null ){
			Debug.LogError("mVisual error");
			return;
		}

		if ( mLink == null ){
			Debug.LogError("mLink error");
		}

	    if ( mUI == null ) {
	        Debug.LogError( "mUI error" );
	        return;
	    }


	 //   mAnimator = GetComponent <Animator> ();
	  //  if ( mAnimator == null ) {
	     //   Debug.LogError( "animation error" );
	   // }

	    mUI.ShowGUI(false);



	}
	public void StopAndIdleAnimator()
	{
	//	mAnimator.StartPlayback();
		Debug.Log ("DISABLED");
	//	mAnimator.enabled = false;

	}

	public void StartAnimator ()
	{
		Debug.Log ("ENABLED");
	//	mAnimator.enabled = true;
	//	mAnimator.StopPlayback();
	}

	void Update () {

	}


	float AngleDir(Vector2 A, Vector2 B)
	{
		Vector3 crossVector = Vector3.Cross (A, B); 
		
		if (crossVector.z > 0)
			return -1;
		else
			return 1;
	}

	public void UpdateLinkPosition (Vector3 position) {
	  //
		if (mParent != null)
		{
		
            Vector2 angleVector = new Vector2 (mParent.transform.position.x - transform.position.x, mParent.transform.position.y - transform.position.y);
			
			float angle = 90 +  Mathf.Atan (angleVector.y / angleVector.x) * 180.0f / Mathf.PI ;
			
			if (angleVector.x < 0)
				angle += 180;
			
		
			float angleShift = transform.eulerAngles.z - (angle - 180);
			transform.eulerAngles = new Vector3 (0, 0, angle - 180);
			mRotor.transform.eulerAngles = new Vector3 (0, 0, mRotor.transform.eulerAngles.z + angleShift);
        
            this.mLink.UpdatePos(mParent.transform.position, transform.position);
            //Debug.Log( "UPDATE POS" );
			// = new Vector3 (0, 0,  mRotor.transform.eulerAngles.z - transform.eulerAngles.z);
		}
		
		foreach (STNode childNode in mRotor.mChilds)
		{
			childNode.UpdateLinkPosition(this.transform.position);
		}
	}
	
	public void HideAllGUIS()
	{
		mUI.ShowGUI(false);
	
		
		foreach (STNode childNode in mRotor.mChilds)
		{
			childNode.HideAllGUIS();	
		}
	}
	

	
	#region Interface
	public void RemoveChild(){
		while(mRotor.mChilds.Count > 0){
			STNode forDel = mRotor.mChilds[0];
			Destroy(forDel.gameObject);
			mRotor.mChilds.Remove(forDel);
			
		}
	}
	#endregion
	
	#region NetRegion

	
	void alignToNet(List<GameObject> netNodeList)
	{
		float minDist = 1000000;
		GameObject clothestGameObject = null;
		
		foreach (GameObject sphere in netNodeList)
		{
		//	Debug.Log(transform.position);
			float currentDist = Vector3.Distance (transform.position, sphere.transform.position);
			
			if (currentDist < minDist)
			{
				minDist = currentDist;
				clothestGameObject = sphere;
			}
		}
		
		if (clothestGameObject != null)
		{
			transform.position = new Vector3 ( clothestGameObject.transform.position.x , clothestGameObject.transform.position.y, transform.position.z);
		}
	}
	
	#endregion


	void showNodeSelection ()
	{
	    STATE_T currentTState;

			NodeState = NODE_ST.SELECTED;
	    if ( NodeType == NODE_T.SIMPLE )
	        currentTState = STATE_T.simpleSelected;
	            //mVisual.ChangeState (STATE_T.simpleSelected);
	    else
            currentTState = STATE_T.collectableSelected;
	    //mVisual.ChangeState (STATE_T.collectableSelected);

	    mVisual.ChangeState( currentTState );
        if (mLink != null)
            mLink.ChangeState (currentTState);

	}

	void hideNodeSelection ()
	{
        STATE_T currentTState;

		NodeState = NODE_ST.SIMPLE;
        if (NodeType == NODE_T.SIMPLE)
            currentTState = STATE_T.simple;
       
        else
            currentTState = STATE_T.collectable;

        mVisual.ChangeState(currentTState);
        if (mLink != null)
            mLink.ChangeState(currentTState);
	}

	void setActiveSelectionLeafesFlags ()
	{


		if (STLevel.GetRootNode().activeNode != null)
		{
			//if (STLevel.GetRootNode().selectionContainsActiveNodes == false)
				//STLevel.GetRootNode().activeNodesCount = 0;
			// != null)
			STLevel.GetRootNode().activeNodesCount = 0;
			//STLevel.GetRootNode().selectionContainsActiveNodes = false;
			STLevel.GetRootNode().activeNode.checkForPrevActiveFlags();
			STLevel.GetRootNode().setActiveSelectionFlagsNonactive();


			STLevel.GetRootNode().activeNode.setActiveSelectionFlagsActive();
		}

	}

	void checkForPrevActiveFlags()
	{
		if (isSelectedActive == true)
			STLevel.GetRootNode().activeNodesCount --;

		foreach (STNode node in mRotor.mChilds)
		{
			node.checkForPrevActiveFlags ();
		}
	}

	void setActiveSelectionFlagsActive ()
	{
//		Debug.Log (transform.position);
		STLevel.GetRootNode().activeNodesCount ++;
		isSelectedActive = true;
		foreach (STNode node in mRotor.mChilds)
		{
			node.setActiveSelectionFlagsActive ();
		}
	}

	void setActiveSelectionFlagsNonactive ()
	{
//		if (isSelectedActive == true)
//		{
//			STLevel.GetRootNode().activeNodesCount --;
//			STLevel.GetRootNode().selectionContainsActiveNodes = true;
//		}
	//	isPrevSelectedActive = isSelectedActive;
		isSelectedActive = false;
		foreach (STNode node in mRotor.mChilds)
		{
			node.setActiveSelectionFlagsNonactive ();
		}
	}

   
  


	void startHideAnimationFromLeafes ()
	{
		if (STLevel.GetRootNode().activeNode != null)
		    if ( STLevel.GetRootNode().activeNode.mParent != null ) {
               // Debug.Log("startHideAnimationFromLeafes - 1");
		       // STLevel.GetRootNode().activeNode.setAnimatorForUnselectState();
		    }

	    if (mRotor.mChilds.Count == 0)
		{
			if (isSelected == true)
			{
				if (isSelectedActive == false)
				{
					hideNodeSelection ();
                  //  Debug.Log("startHideAnimationFromLeafes - 2");
					setAnimatorForUnselectState();

//					Debug.Log ("YES" + transform.position);
				}
				else
				{
//					Debug.Log ("NO ACTIVE" + transform.position);
				}
			}
		}
		else
		{
			foreach (STNode node in mRotor.mChilds)
			{
				node.startHideAnimationFromLeafes();
			}
		}
	}

	protected void changeNodeType ()
	{
		NodeType ++;
		
		if (NodeType > NODE_T.COLLECTABLE)
			NodeType = 0;
		
		mVisual.Change (NodeType);
	}
	
	
	public STSerializedNode GetSerializedObject (STSerializedNode sObject)
	{
		sObject.rotation = new STSerializableQuaternion();
		sObject.rotation.X = mRotor.transform.rotation.x;
		sObject.rotation.Y = mRotor.transform.rotation.y;
		sObject.rotation.Z = mRotor.transform.rotation.z;
		sObject.rotation.W = mRotor.transform.rotation.w;

		sObject.position = new STSerializableVector3();
		sObject.position.X = transform.localPosition.x;
		sObject.position.Y = transform.localPosition.y;
		sObject.position.Z = transform.localPosition.z;
		
		if (NodeType == NODE_T.SIMPLE)
			sObject.node_type = 0;
		
		else if (NodeType == NODE_T.COLLECTABLE)
			sObject.node_type = 1;
		
		sObject.children = new STSerializedNode[mRotor.mChilds.Count];
		int i = 0;
		foreach (STNode childNode in mRotor.mChilds)
		{
			sObject.children [i] = new STSerializedNode();
			sObject.children [i] = childNode.GetSerializedObject(sObject.children [i]);
			
			Debug.Log ("SER CHILD" + sObject.children [i]);
			
			i++;
		}
		
		return sObject;
	}
	
	
	public void LoadNodes (STSerializedNode sNode)
	{
		this.loadNodes (sNode);
	//	SaveChildPositions ();
		this.UpdateLinkPosition (this.transform.position);
	//	LoadChildPositions ();
	}
	
	 void loadNodes (STSerializedNode sNode)
	{
		mRotor.transform.rotation = new Quaternion (sNode.rotation.X, sNode.rotation.Y, sNode.rotation.Z, sNode.rotation.W);
		transform.localPosition = new Vector3 (sNode.position.X, sNode.position.Y, sNode.position.Z);
		
		if (sNode.node_type == 1)
			this.changeNodeType();
		if (sNode.children == null)
			return;
		
		foreach (STSerializedNode nodeChild in sNode.children )
		{
			STRootNode root = STLevel.GetRootNode();
			STNode node = Instantiate(root.NodePref) as STNode;
			if ( node != null )
			{
						node.transform.parent = mRotor.transform;
						node.mParent = this;
						mRotor.mChilds.Add(node);
						node.LoadNodes (nodeChild);
			}
		}
		
	}
	
	public void SetEnableTouches (bool val)
	{
//		mUI.collider.enabled = val;
		
		foreach (STNode childNode in mRotor.mChilds)
		{
			childNode.SetEnableTouches (val);
		}
	}
	
	public void GenNextNode (int N, int depth, int prevPattern, int curBranchLength)
	{
		if (N == 0)
			return;

		STPattern pattern = STPatternManager.GetPattern(depth);
		List <int> paternVects = new List<int>();
		List <Vector3> vectPositions = STLevel.GetNodesNet().GetGenPositions (pattern, transform.position, prevPattern, paternVects, curBranchLength);
		int i = 0;

		if (vectPositions != null)


		foreach (Vector3 nodePos in vectPositions)
		{
			STRootNode root = STLevel.GetRootNode();
			STNode nodePref = Instantiate(root.NodePref) as STNode;
			if ( nodePref != null )
			{
				nodePref.brenchLength = this.brenchLength + (((paternVects[i] - 1) / 8) + 1);

				nodePref.transform.parent = mRotor.transform;
				nodePref.mParent = this;
					
				nodePref.nodeDepth = depth;
				mRotor.mChilds.Add(nodePref);
				nodePref.transform.position = new Vector3 (nodePos.x, nodePos.y, - depth );

                STLevel.GetRootNode().SetTreeDepth( depth );
			}

			i++;
		}

		i = 0;

		foreach (STNode node in mRotor.mChilds)
		{
			node.GenNextNode (N - 1, depth + 1, paternVects[i], node.brenchLength);
			i++;
		}
	}

	public bool CheckForIntersection ( float X1, float Y1, float X2, float Y2, float X3, float Y3)
	{	
		bool result = false;
		float X4 = this.transform.position.x;
		float Y4 = this.transform.position.y;



		foreach (STNode child in mRotor.mChilds)
		{
			result = child.CheckForIntersection (X1, Y1, X2, Y2, X4, Y4);
			if (result == true)
				return result;
		}
		
		return //checkForLinesIntersection (X1, Y1, X2, Y2, X3, Y3, X4, Y4) ||
				//checkForLinkIntersection (90, X3, Y3, X4, Y4, X1, Y1) ||
				checkForPointsIntersection (STNode.nodeSize, X1, Y1, X3, Y3) ||
				checkForPointsIntersection (STNode.nodeSize, X1, Y1, X4, Y4);

	}

	bool checkForLinesIntersection (float X1, float Y1, float X2, float Y2, float X3, float Y3, float X4, float Y4)
	{
		return  ( ((((X3 - X1) * (Y2 - Y1) - (Y3 - Y1) * (X2 - X1)) * 
		            ((X4 - X1) * (Y2- Y1) - (Y4 - Y1) * (X2 - X1)) < 0) &&
		           (((Y4 - Y3) - (Y1 - Y3) * (X4 - X3)) * 
				 ((X2 - X3) * (Y4 - Y3) - (Y2 - Y3) * (X4 - X3)) < 0)) );
	}
	
	
	bool checkForPointsIntersection (float radius, float X1, float Y1, float X2, float Y2)
	{
		return Mathf.Pow (X1 - X2, 2) + Mathf.Pow (Y1 - Y2, 2) <= 4.0f * Mathf.Pow (radius, 2);
	}

	bool checkForLinkIntersection (float radius, float X1, float Y1, float X2, float Y2, float M1, float M2)
	{
		float A = Y2 - Y1;
		float B = X1 - X2;
		float C = -X1 * (Y2 - Y1) + Y2 * (X2 - X1);
		return Mathf.Abs(A * M1 + B * M2 + C) / (Mathf.Sqrt(Mathf.Pow (A , 2) + Mathf.Pow (B , 2))) <= 2.0f * radius;
	}

	
	public void MirrorLevel (STNode prevNode, bool isFlipped, bool isYMirror)
	{
		List <Vector3> positions = new List <Vector3> ();
		foreach (STNode node in mRotor.mChilds)
		{
			if (isFlipped == false)
				if (isYMirror == true)
					positions.Add (new Vector3 (node.transform.position.x, -node.transform.position.y, node.transform.position.z));
				else
					positions.Add (new Vector3 (-node.transform.position.x, node.transform.position.y, node.transform.position.z));
			else
				positions.Add (new Vector3 (-node.transform.position.x, -node.transform.position.y, node.transform.position.z));
		}

		int i = 0;
		foreach (Vector3 nodePos in positions)
		{
			STRootNode root = STLevel.GetRootNode();
			STNode nodePref = Instantiate(root.NodePref) as STNode;
			nodePref.transform.position = nodePos;
			prevNode.mRotor.mChilds.Add (nodePref);
			nodePref.transform.parent = prevNode.mRotor.transform;
			nodePref.mParent = prevNode;
			if (mRotor.mChilds.Count > 0)
				nodePref.nodeDepth = mRotor.mChilds[0].nodeDepth;
			mRotor.mChilds[i].MirrorLevel(nodePref, isFlipped, isYMirror);
			i++;
		}

	}

	public void SetCollectableLastNodes (List <Vector3> nodesList)
	{
		if (mRotor.mChilds.Count == 0)
		{
			nodesList.Add(transform.position);
			changeNodeType();
		}

		foreach (STNode node in mRotor.mChilds)
		{
			node.SetCollectableLastNodes(nodesList);
		}
	}

	public void GetCollectableNodes (List <Vector3> nodesList)
	{
		if (NodeType == NODE_T.COLLECTABLE)
			nodesList.Add (transform.position);

		foreach (STNode node in mRotor.mChilds)
		{
			node.GetCollectableNodes(nodesList);
		}
	}

	public float GetMaxDist ( Vector2 rotVect)
	{
		float dist = Vector2.Distance (rotVect, transform.position);

		foreach (STNode node in mRotor.mChilds)
		{
			float nodeDist = node.GetMaxDist(rotVect);
			if (nodeDist > dist)
				dist = nodeDist;
		}

		return dist;
	}


	public void MixLevel ()
	{
			if (mRotor.mChilds.Count == 0)
				return;

			List <int> validRotations = new List<int> ();

			for (int i = 0; i < 8; i++)
			{
				mRotor.transform.Rotate (0 , 0,  45);

				//transform.Rotate (0, 0, 45);

			List <Vector3> nodesPos = new List<Vector3> ();
			STLevel.GetRootNode().GetTreePositions(nodesPos);

				if (checkTreeForIntersection(nodesPos, STNode.nodeSize) == false)
				{
					validRotations.Add (i);
				}
			}

			if (validRotations.Count > 0)
			{
				int index = (int) Mathf.Round ( Random.value * (validRotations.Count - 1));
				mRotor.transform.Rotate (0, 0, 45 * (validRotations[index] + 1));
				//transform.Rotate (0, 0, 45 * (validRotations[index] + 1));
			}

			foreach (STNode node in mRotor.mChilds)
			{
				node.MixLevel ();
			}

	}

	bool checkTreeForIntersection(List <Vector3> nodesPos, int R)
	{
		for (int i = 0; i < nodesPos.Count - 1; i++)
		{
			for (int j = i + 1; j < nodesPos.Count; j++)
			{
				if (Vector3.Distance (nodesPos[i], nodesPos[j]) < R)
					return true;
			}
		}

		return false;
	}



	public void ClearOverlayedNodes ()
	{
		isOverlayed = false;

		foreach (STNode node in mRotor.mChilds)
		{
			node.ClearOverlayedNodes ();
		}
	}


	public void GetTreeNodes (ref List <STNode> nodesArray)
	{
		nodesArray.Add (this);
//		Debug.Log (transform.position);
		foreach (STNode node in mRotor.mChilds)
		{
			node.GetTreeNodes (ref nodesArray);
		}
	}

	public void GetTreePositions(List <Vector3> positions)
	{
		positions.Add(transform.position);

		Debug.Log (transform.position);
		foreach (STNode node in mRotor.mChilds)
		{
			node.GetTreePositions(positions);
		}
	}

	public void SaveSolution ()
	{
		solvedRotation = mRotor.transform.eulerAngles;
		foreach (STNode node in mRotor.mChilds)
		{
			node.SaveSolution();
		}
	}

	bool checkIfPointInRect (Vector2 minPos, Vector2 maxPos, Vector3 point)
	{
		return ((point.x > minPos.x) && (point.y > minPos.y) && (point.x < maxPos.x) && (point.y < maxPos.y));
	}


	public void GetTouchedNodes (ref List <STNode> touchedNodes, Vector3 touchpos)
	{
		Vector2 minNodePos;
		Vector2 maxNodePos;

		if (mParent != null)
		{
			minNodePos = new Vector2 (transform.position.x - STNode.nodeSize, transform.position.y - STNode.nodeSize * 0.75f );
			maxNodePos = new Vector2 (transform.position.x + STNode.nodeSize, transform.position.y + STNode.nodeSize * 0.75f );
		}
		else
		{
			minNodePos = new Vector2 (transform.position.x - STNode.nodeSize, transform.position.y - STNode.nodeSize * 1.5f);
			maxNodePos = new Vector2 (transform.position.x + STNode.nodeSize, transform.position.y + STNode.nodeSize * 1.5f);
		}

		if (checkIfPointInRect (minNodePos, maxNodePos, touchpos))
		{
			touchedNodes.Add (this);
		}

		foreach (STNode node in mRotor.mChilds)
		{
			node.GetTouchedNodes (ref touchedNodes, touchpos);
		}
	}

	public bool GetTouchedLinkNodes (ref List <STNode> touchedNodes, Vector3 touchpos)
	{
		if (checkIfPointInRect(minPoint, maxPoint, touchpos))
		{
			touchedNodes.Add (this);
			DebugDraw.DrawRect (minPoint, maxPoint, Color.red);
		}
		else
		{
			return false;
		}

		foreach (STNode node in mRotor.mChilds)
		{
			node.GetTouchedLinkNodes (ref touchedNodes, touchpos);
		}
	
		return true;
	}


	public void CalcRects (ref Vector2 minP, ref Vector2 maxP)
	{
		List <Vector2> minPointsList = new List <Vector2> ();
		List <Vector2> maxPointsList = new List <Vector2> ();

		Vector2 minVect = new Vector2 ();
		Vector2 maxVect = new Vector2 ();

		foreach (STNode node in mRotor.mChilds)
		{
			node.CalcRects(ref minVect, ref maxVect);
			minPointsList.Add (minVect);
			maxPointsList.Add (maxVect);
		}
		maxPointsList.Add (new Vector2(transform.position.x, transform.position.y));
		minPointsList.Add (new Vector2(transform.position.x, transform.position.y));
		                
		minVect.x = 100000;
		minVect.y = 100000;

		maxVect.x = -100000;
		maxVect.y = -100000;
		
		foreach (Vector2 vect in maxPointsList)
		{
			if (vect.x > maxVect.x)
				maxVect.x = vect.x;

			if (vect.y > maxVect.y)
				maxVect.y = vect.y;
		}

		foreach (Vector2 vect in minPointsList)
		{
			if (vect.x < minVect.x)
				minVect.x = vect.x;
			
			if (vect.y < minVect.y)
				minVect.y = vect.y;
		}

		minP = minVect;
		maxP = maxVect;

		minPoint.x = minP.x - STNode.nodeSize / 2 ;
		minPoint.y = minP.y - STNode.nodeSize / 2;
		
		maxPoint.x = maxP.x + STNode.nodeSize / 2;
		maxPoint.y = maxP.y + STNode.nodeSize / 2;

		DebugDraw.DrawRect (minPoint, maxPoint, Color.green);
	}
	
	public void Solve ()
	{
		mRotor.transform.eulerAngles = solvedRotation;
		foreach (STNode node in mRotor.mChilds)
		{
			node.Solve();
		}
	}

	public bool CheckIfLinksTouched (Vector3 touchPos)
	{
		float x1 = this.transform.position.x;
		float y1 = this.transform.position.y;

	//	Debug.Log ("TouchPos - " + touchPos);
//		Debug.Log ("Node Position - " + transform.position);

		foreach (STNode node in mRotor.mChilds)
		{
//			Debug.Log ("Node Child Position - " + node.transform.position);

			float x2 = node.transform.position.x;
			float y2 = node.transform.position.y;

			float m = (y2 - y1) / (x2 - x1 + 0.1f);
			float b = - m * x1 + y1;

			float Dist = Mathf.Abs (touchPos.y - m * touchPos.x - b) / Mathf.Sqrt (m * m + 1);

	//		Debug.Log (Dist);

			if (Dist < STNode.nodeSize)
			{
				float linkLength = Vector2.Distance (new Vector2 (x1, y1), new Vector2 (x2, y2));
				if ((Vector2.Distance (new Vector2(x1, y1) , new Vector2 (touchPos.x, touchPos.y)) < linkLength) && (Vector2.Distance (new Vector2(x2, y2) , new Vector2 (touchPos.x, touchPos.y)) < linkLength))
					return true;
			}
		}
		return false;
	}
	
	
}
