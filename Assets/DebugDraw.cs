using UnityEngine;
using System.Collections;

public class DebugDraw : MonoBehaviour {
	
	static public void DrawCrossMark (Vector3 point, float size, Color color)
	{

		float time = 10.0f;
		Vector3 FminP = new Vector3(point.x - size, point.y - size, -100);
		Vector3 FmaxP = new Vector3(point.x + size, point.y + size, -100);
		
		Vector3 LminP = new Vector3(point.x + size, point.y - size, -100);
		Vector3 LmaxP = new Vector3(point.x - size, point.y + size, -100);
		
		
		Debug.DrawLine (FminP, FmaxP, color, time);
		Debug.DrawLine (LminP, LmaxP, color, time);

	}

	static public void DrawRect (Vector2 minPoint, Vector2 maxPoint, Color color)
	{
		float time = 10.0f;
		Debug.DrawLine (new Vector3 (minPoint.x, minPoint.y, -100), new Vector3 (maxPoint.x, minPoint.y, -100), color, time, false);
		Debug.DrawLine (new Vector3 (minPoint.x, minPoint.y, -100), new Vector3 (minPoint.x, maxPoint.y, -100), color, time, false);
		Debug.DrawLine (new Vector3 (maxPoint.x, maxPoint.y, -100), new Vector3 (minPoint.x, maxPoint.y, -100), color, time, false);
		Debug.DrawLine (new Vector3 (maxPoint.x, maxPoint.y, -100), new Vector3 (maxPoint.x, minPoint.y, -100), color, time, false);
	}
	

}