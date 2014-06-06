using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class STControl : MonoBehaviour {

	enum TouchType
	{
		none_t,
		not_definded_t,
		select_t,
		move_t,
		//inertion_t,
		reach_t
	}

	List <STNode> nodesList = new List<STNode> ();
	Queue <float> angleQueue = new Queue <float> (3);

	TouchType touchType = TouchType.none_t;

	float delayMaxTime = 0.2f;
	float delayTime = 0.0f;
	float delayMaxDist = STNode.nodeSize / 2;
	float delayDist = 0.0f;

	float dirAngle = 1;

	Vector3 prevRotVector = new Vector3 ();
	Vector3 rotAngle = new Vector3 ();
	Vector3 touchPos = new Vector3 ();

	public iListener mListener = null;

	public float StartRotateValue;
	public float IncreaseRotateValue;
	
	public STRootNode ControlableNode;


	Vector3 prevTouchPosition = new Vector3();
	Vector3 touchBeginPosition = new Vector3();
	Vector3 nodeRotatePosition = new Vector3();
	Vector3 beginVector = new Vector3();
	
	public bool isEnabledState = false;


	int currentTouchedObject = 0;

	Vector3 getTouchPosition ()
	{

		Vector3 mouse_pos = Input.mousePosition;
		mouse_pos = Camera.main.ScreenToWorldPoint(mouse_pos);
		mouse_pos.z = -100;

		return mouse_pos;
	}

	void OnMouseDown()
	{
		if (touchType != TouchType.none_t)
			return;

		if (Input.touchCount > 1) 
			return;

		if (STLevel.isEditMode == false)
		{
			angleQueue.Clear();
			delayTime = delayDist = 0;
			prevTouchPosition = getTouchPosition();

			if (STLevel.GetRootNode().activeNode != null)
				nodeRotatePosition = STLevel.GetRootNode().activeNode.transform.position;

			touchBeginPosition = getTouchPosition();
			beginVector =  (touchBeginPosition - nodeRotatePosition);

//			Debug.Log ("MOUSE_DOWN - " + beginVector);

			prevRotVector = beginVector;

			touchType = TouchType.not_definded_t;
		}
		else
		{
			SelectNode();

			if (currentTouchedObject < nodesList.Count)
				nodesList [currentTouchedObject].onMouseDown();

			touchType = TouchType.move_t;
		}
	}

	void RotateNode ()
	{
		if (STLevel.GetRootNode().activeNode == null)
			return;

		rotAngle = STLevel.GetRootNode().activeNode.mRotor.transform.rotation.eulerAngles;
//		nodeRotatePosition = STLevel.GetRootNode().activeNode.transform.position;

		touchType = TouchType.move_t;
		STLevel.HideOverlayedNodes ();
	}

	bool SelectNode ()
	{
			touchType = TouchType.select_t;
			touchBeginPosition = getTouchPosition ();

			nodesList.Clear();
			nodesList = STLevel.GetTouchedNodesList(touchBeginPosition);
			
			currentTouchedObject ++;
			
			if (currentTouchedObject > nodesList.Count - 1)
				currentTouchedObject = 0;
			
			if (nodesList.Count > 0)
			{
				nodesList [currentTouchedObject].onMousePressed();

				return true;
			}
			return false;
	}

	public void ChangeEnabledState(bool state)
	{
		isEnabledState = state;
	}

	void OnMouseDrag()
	{
		if ((touchType == TouchType.move_t) || (touchType == TouchType.not_definded_t))
		{
			Vector3 mouse_pos = getTouchPosition();
			Vector3 touchVector =  (mouse_pos - nodeRotatePosition);

//			Debug.Log ("DRAG - " + touchVector);
			
			Vector2 beg2D = new Vector2(beginVector.x, beginVector.y);
			Vector2 touch2D = new Vector2(touchVector.x, touchVector.y);
			
			float angle = Vector2.Angle (beg2D, touch2D);
			float rotDir = AngleDir(touch2D, beg2D);
			dirAngle = angle * rotDir;
			
			Vector2 prevTouch2D = new Vector2 (prevRotVector.x, prevRotVector.y);
			
			float prevTouchAngle = Vector2.Angle (touch2D, prevTouch2D);
			float prevRotAngleDir = AngleDir (touch2D, prevTouch2D);
			float prevDirAngle = prevTouchAngle * prevRotAngleDir;
			
			//				Debug.Log (prevDirAngle);
			
			if (prevDirAngle != 0)
			{
				if (angleQueue.Count > 2)
					angleQueue.Dequeue();
				
				angleQueue.Enqueue (prevDirAngle);
			}
			
			prevRotVector = touchVector;

			//globalMouseMoveCount ++;
		}
	}


	
	void OnMouseUp ()
	{
		if (touchType == TouchType.none_t)
			return;

		if (STLevel.isEditMode == false)
		{
			if ((touchType == TouchType.move_t) || (touchType == TouchType.not_definded_t))
			{
				bool isNotDefTouch = false;
				if (touchType == TouchType.not_definded_t)
				{
					isNotDefTouch = true;
					if (SelectNode() == true)
					{
						touchType = TouchType.none_t;
						return;
					}
					touchType = TouchType.move_t;
				}

				OnMouseDrag();
				touchType = TouchType.reach_t;
			
				if (STLevel.GetRootNode().activeNode != null)
					rotAngle = STLevel.GetRootNode().activeNode.mRotor.transform.rotation.eulerAngles;

				float avarageAngle = 0;

				foreach (float angle in angleQueue)
				{
					avarageAngle += angle;
				}

				if (angleQueue.Count > 0)
					avarageAngle /= angleQueue.Count;


				if ((Mathf.Abs (avarageAngle) > 2.0f) || ((isNotDefTouch == true) && (Mathf.Abs (avarageAngle) > 1.5f)) )
				{
//					Debug.Log ("Fast slide");
					
					if (Mathf.Sign (avarageAngle) < 0)
					{
						if ( Mathf.Abs (rotAngle.z % 45.0f) > 0.01f)
							nodeRotate ( - rotAngle.z % 45.0f , 0.2f);
						else
							nodeRotate ( -45.0f, 0.2f);
					}
					else
					{
						nodeRotate (45.0f - rotAngle.z % 45.0f  , 0.2f);
					}
				}
				else
				{
//					Debug.Log ("Slow slide");
					if (rotAngle.z % 45.0f > 22.5)
						nodeRotate (45.0f - rotAngle.z % 45.0f , 0.2f);
					else
						nodeRotate (-rotAngle.z % 45.0f , 0.2f);
				}
	//			Debug.Log (globalMouseMoveCount);
				//Debug.Log ("Last Angle - " + lastAngle);
//				Debug.Log ("Avarage Angle - " + avarageAngle);
//				Debug.Log ("COUNT - " + angleQueue.Count);
//				foreach (float angle in angleQueue)
//				{
//					Debug.Log (angle);
//				}
			}
			else
			{
				touchType = TouchType.none_t;
			}
		}
		else
		{
			if (currentTouchedObject < nodesList.Count)
				nodesList [currentTouchedObject].onMousePressed();

			touchType = TouchType.none_t;
		}


	}
	
	void Start () 
	{
//		Input.simulateMouseWithTouches = true;
//		Input.multiTouchEnabled = false;
		//Input.touches[0].position;


	}

	void Update () 
	{

		if (STLevel.isEditMode == false)
		{
			touchPos = getTouchPosition();


			if (touchType == TouchType.not_definded_t)
			{
				delayTime += Time.deltaTime;

				if (delayTime > delayMaxTime)
				{
					SelectNode ();
				}

				delayDist += Vector3.Distance (touchPos, prevTouchPosition);
				prevTouchPosition = touchPos;

				if (delayDist > delayMaxDist)
				{
					RotateNode ();
				}
			}
				if (touchType == TouchType.move_t)
					//angleRotateChange (dirAngle);
					angleRotateChange (dirAngle);
		}
		else
		{
			if (touchType == TouchType.move_t)
			{
				if (currentTouchedObject < nodesList.Count)
					nodesList [currentTouchedObject].onMove();
			}
		}
	}

	float AngleDir(Vector2 A, Vector2 B)
	{
		Vector3 crossVector = Vector3.Cross (A, B);

		if (crossVector.z > 0)
			return -1;
		else
			return 1;
	}

	void nodeRotate (float angle, float time)
	{
		STLevel.HideOverlayedNodes ();

		if (STLevel.GetRootNode().activeNode != null)
			rotAngle = STLevel.GetRootNode().activeNode.mRotor.transform.rotation.eulerAngles;

		iTween.ValueTo (gameObject, iTween.Hash ("from", 0, "to", angle , "looptype", "none", "time", time, "onUpdate","angleRotateChange","onComplete", "angleRotateChangeComplete" ));
	}
			
	void angleRotateChange (float currentAngle)
	{
		//STLevel.UpdateInfoPos();
		STLevel.GetRootNode().Rotate (rotAngle.z + currentAngle);
	}
		
	void angleRotateChangeComplete ()
	{
		touchType = TouchType.none_t;
		STLevel.CheckForSolution();
		STLevel.CalcTreeRects();
		STLevel.HandleOverlayedNodes ();
	}
}
