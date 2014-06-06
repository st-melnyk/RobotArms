using UnityEngine;
using System.Collections;

public class SioNodeVizualizator : MonoBehaviour {

	public GameObject[] mChilds;


	public void SetState(SioNode.NODE_STATE_T new_state){
		if ( mChilds != null && mChilds.Length > 0 ){
			Color rgba = new Color(0,0,0,1);
			switch(new_state){
			case(SioNode.NODE_STATE_T.PRIMARY_SELECT):{
				rgba = Color.red;
				break;		
			}
			case(SioNode.NODE_STATE_T.SELECT):{
				rgba = Color.yellow;
				break;		
			}
			case(SioNode.NODE_STATE_T.UNSELECT):{
				rgba = Color.black;
				break;		
			}				
			}

			for(int i = 0; i < mChilds.Length; ++i ){
				if ( mChilds[i].transform.renderer != null ){
					mChilds[i].transform.renderer.material.color = rgba;
				}
			}
		}
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
