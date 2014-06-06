using UnityEngine;
using System.Collections;

public enum SCENE_MODE
{
	EDIT,
	PLAY,
	TEST_ANIM
}

public class STScene : MonoBehaviour {

	public SCENE_MODE SceneMode = SCENE_MODE.EDIT;
	public static STScene _instance = null;
	public Vector2 ScreenSize  = new Vector2(1024, 768);
	void Awake ()
	{
		STScene.Instance();
		STLevel.Instance();
	}

	public static STScene Instance()
	{
		if (_instance == null)
		{
			GameObject scene = GameObject.Find("Main Scene") ;
			_instance = scene.GetComponent<STScene>();
		}

		return _instance;
	}
	
	void Start () {

	}

	void Update () {

	}

	public static SCENE_MODE getMode ()
	{
		return _instance.SceneMode;
	}


	public static void UpdateCameraPosition ()
	{
		float time = 0.3f;

		Vector2 branchPos2D = new Vector2();

		Vector3 branchPos = STLevel.GetRootNode().activeNode.transform.position;

		branchPos2D.x = branchPos.x;
		branchPos2D.y = branchPos.y;

		float maxBranchLength = STLevel.GetBranchSize();

		STScene.Instance().moveCamera ( branchPos2D, maxBranchLength, time);
	}


	Vector2 AbsVect (Vector2 vect)
	{
		vect.x = Mathf.Abs (vect.x);
		vect.y = Mathf.Abs (vect.y);

		return vect;
	}

	Vector2 GreaterZeroVect (Vector2 vect)
	{
		if (vect.x < 0)
			vect.x = 0;

		if (vect.y < 0)
			vect.y = 0;

		return vect;
	}
	

	public void moveCamera( Vector2 branchPos, float maxBranchLenght, float time)
	{

		Vector2 outOfScreenVect = AbsVect (branchPos) - ScreenSize;
		outOfScreenVect.x += maxBranchLenght;
		outOfScreenVect.y += maxBranchLenght;

		if (outOfScreenVect.x < 0)
			outOfScreenVect.x = 0;

		if (outOfScreenVect.y < 0)
			outOfScreenVect.y = 0;

		if (maxBranchLenght > ScreenSize.y)
		{
			iTween.ValueTo (STScene.Instance().gameObject, iTween.Hash ("from", Camera.main.orthographicSize, "to", maxBranchLenght, "looptype", "none", "time", time, "onUpdate","cameraSizeChange","onComplete", "cameraScaleComplete" ));
			iTween.ValueTo (STScene.Instance().gameObject, iTween.Hash ("from", Camera.main.transform.position, "to",  new Vector3 ( branchPos.x, branchPos.y, Camera.main.transform.position.z) , "looptype", "none", "time", time, "onUpdate","cameraPositionChange","onComplete", "cameraPositionComplete" ));
		}
		else
		{

			iTween.ValueTo (STScene.Instance().gameObject, iTween.Hash ("from", Camera.main.transform.position, "to",  new Vector3 ( outOfScreenVect.x * Mathf.Sign(branchPos.x), outOfScreenVect.y * Mathf.Sign(branchPos.y), Camera.main.transform.position.z) , "looptype", "none", "time", time, "onUpdate","cameraPositionChange","onComplete", "cameraPositionComplete" ));
			iTween.ValueTo (STScene.Instance().gameObject, iTween.Hash ("from", Camera.main.orthographicSize, "to",  ScreenSize.y , "looptype", "none", "time", time, "onUpdate","cameraSizeChange","onComplete", "cameraScaleComplete" ));
		}
	}

	void cameraScaleComplete ()
	{

	}

	void cameraPositionComplete ()
	{
		
	}

	void cameraSizeChange (float val)
	{
		Camera.main.orthographicSize = val;
	}

	void cameraPositionChange (Vector3 pos)
	{
		Camera.main.transform.position = pos; 
	}




}
