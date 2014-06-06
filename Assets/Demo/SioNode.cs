using UnityEngine;
using System.Collections;

public class SioNode : MonoBehaviour {

	public enum NODE_STATE_T{
		UNSELECT,
		SELECT,
		PRIMARY_SELECT
	}

//	public float ValueToTestAnimator = 0;
	public Animator mAnimator;

	public SioNodeRotor mRotor;
	public SioNodeVizualizator mVizualizator;

	public NODE_STATE_T mState = NODE_STATE_T.UNSELECT;

	public void SetState(NODE_STATE_T new_state){
		Debug.LogError("STATE to : " + new_state.ToString() );
		mState = new_state;

		mAnimator.SetInteger("AnimState", (int)mState);
	}

	protected virtual void onMouseUpDown(){
		if ( mState == NODE_STATE_T.SELECT ||
		    mState == NODE_STATE_T.PRIMARY_SELECT ){
			SetState(NODE_STATE_T.UNSELECT);
		}
		else{
			SetState(NODE_STATE_T.PRIMARY_SELECT);
		}
	}


	public void FinishAnim(int val){
//		Debug.LogError("Fin : " + val.ToString() );
//		if ( val < 0 ){
//			mAnimator.StopRecording();
//		}
//		mAnimator.enabled = false;
			
			//animation.Stop();
			//StopPlayback();
		//mAnimator.ResetTrigger("Select");
		//mAnimator.ResetTrigger("RootSelect");
		//mAnimator.ResetTrigger("Unselect");
	}

	// Use this for initialization
	void Start () {
		//for(int i = 0; i < animation.GetClipCount(); ++i ){
		//	mAnimator.SetTrigger( animation.GetClipCount
		//}

		//mAnimator.SetTrigger(animation.GetClip("NodeAnim1").GetHashCode());
		//mAnimator.SetTrigger(1);
		//mAnimator.SetTrigger(2);
		//mAnimator.SetTrigger(3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	protected void OnMouseUpAsButton(){
		onMouseUpDown();
	}

//	void OnMouseEnter(){
//		Debug.Log("On");
//	}
//
//	void OnMouseExit(){
//		Debug.Log("Off");
//	}
}
