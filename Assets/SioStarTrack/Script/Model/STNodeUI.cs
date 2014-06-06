using UnityEngine;
using System.Collections;

public enum EVENT_T{
	ON_ERROR,
	ON_ADD,
	ON_REMOVE,
	ON_CHANGE,
	ON_MOVE,
	ON_ROTATE_LEFT,
	ON_ROTATE_RIGHT,
	ON_NODE,
	
	ON_MOUSEENTER,
	ON_MOUSEEXIT,
	ON_MOUSEDOWN,
	ON_MOUSEUP,
	ON_MOUSEPRESS,
}

public enum CONTROL_T{
	BTN,
	NODE
}

public interface iListener{
	void OnEvent(EVENT_T t, float val, iListener ch);
}



public class STNodeUI : STLButton, iListener {
	
	public STLButton mBtnAdd;
	public STLButton mBtnRemove;
	public STLButton mBtnChange;

	bool isShowGUI = true;
	
	public bool IsShowGUI(){
		return isShowGUI;
	}
	
	Quaternion rotation;
	
	public void ShowGUI(bool val){
		
		this.transform.rotation = rotation;
		isShowGUI = val;
		if (val == false)
		{
			mBtnAdd.EnableControl(isShowGUI);
			mBtnRemove.EnableControl(isShowGUI);
			mBtnChange.EnableControl(isShowGUI);	
		}
		else
		{
			if (STLevel.GetRootNode().GetEditMode() == false)
			{

			}
			else
			{
				mBtnAdd.EnableControl(isShowGUI);
				mBtnRemove.EnableControl(isShowGUI);
				mBtnChange.EnableControl(isShowGUI);
			}
		}
	}
	

	
	#region iListener
	void iListener.OnEvent(EVENT_T t, float val, iListener ch){

		if ( mListener != null ){
			mListener.OnEvent(t, val, ch);
		}
		
	}
	#endregion
	
	#region PROTECT

	#endregion

	void Awake()
	{
		rotation = this.transform.rotation;
	}
	
	void Start () {
		if (mListener != null ){
			mBtnAdd.mListener = this as iListener;
			mBtnChange.mListener = this as iListener;
			mBtnRemove.mListener = this as iListener;

			mBtnAdd.SetColor (Color.green);
			mBtnChange.SetColor (Color.cyan);
			mBtnRemove.SetColor (Color.red);
		}
	}
	
	void OnMouseEnter ()
	{
		(this as iListener).OnEvent(EVENT_T.ON_MOUSEENTER, -1, this);
	}
	
	void OnMouseExit ()
	{
		(this as iListener).OnEvent(EVENT_T.ON_MOUSEEXIT, -1, this);
	}



}
