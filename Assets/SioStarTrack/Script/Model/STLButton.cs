using UnityEngine;
using System.Collections;

public class STLButton : MonoBehaviour, iListener {
	
	bool state_on = false;
	public iListener mListener = null;
	public float mVal;
	public CONTROL_T control_t;
	public EVENT_T event_t;
	
	Color theColor = Color.white;
	
	
	
	#region iListener
	void iListener.OnEvent(EVENT_T t, float val, iListener ch){
		string name_info = "none";
		if ( ch is STLButton){
			name_info = (ch as STLButton).name;
		}
		Debug.LogError(string.Format("Event {0}, {1}", t, name_info));

		if ( mListener != null ){
			mListener.OnEvent(t, val, ch);
		}
	}
	#endregion
	
	#region Interface
	public void EnableControl(bool val){
		if (collider != null ){
			collider.enabled = val;
		}
		
		if ( renderer != null ){
			renderer.enabled = val;
		}
	}
	
	
	public void SetColor (Color val)
	{
		theColor = val;
		
		if (renderer != null)
			renderer.material.color = val;
	}
	#endregion
	
	

	
	protected virtual void onUpDown(){			
		state_on = !state_on;
		(this as iListener).OnEvent(event_t, state_on ? 1 : 0, this);
	}
	
	protected virtual void onMouseDrag()
	{
		if ( event_t == EVENT_T.ON_NODE ){
			(this as iListener).OnEvent(EVENT_T.ON_MOVE, 0, this);
		}
	}
	
	void OnMouseUpAsButton(){
		onUpDown();
		//Debug.Log ("OO");
		if ( event_t == EVENT_T.ON_NODE ){
			(this as iListener).OnEvent(EVENT_T.ON_MOUSEUP, 0, this);
		}
	}
	
	void OnMouseDown()
	{
		if ( event_t == EVENT_T.ON_NODE ){
			(this as iListener).OnEvent(EVENT_T.ON_MOUSEDOWN, 0, this);
		}
	}
	
	void OnMouseDrag ()
	{
		onMouseDrag();
	}
	
	void OnMouseEnter ()
	{
		if (renderer != null)
		{
			renderer.material.color = Color.blue;
		}
	}
	
	void OnMouseExit ()
	{
		if (renderer != null)
		{
			renderer.material.color = theColor;
		}		
	}
}
