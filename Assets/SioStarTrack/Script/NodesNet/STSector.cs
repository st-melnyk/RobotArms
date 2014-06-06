using UnityEngine;

public class STSector
{
	float k1;
	float b1;
	
	float k2;
	float b2;
	
	public STSector (float a1, float b1, float a2, float b2)
	{
		SetFirstLine (a1, b1);
		SetSecondLine (a2, b2);
	}
	
	float getBFromAngle (float angle)
	{
		return Mathf.Tan (angle / 180 * Mathf.PI);
	}
	
	public void SetFirstLine (float alpha, float b)
	{
		k1 = getBFromAngle (alpha);
		b1 = b;
		
		Debug.Log ("FIRST LINE - " + k1 + " " + b1);
	}
	public void SetSecondLine (float alpha, float b)
	{
		k2 = getBFromAngle (alpha);
		b2 = b;
		
		Debug.Log ("SECOND LINE - "+ k2 + " " + b2);
	}
	
//	public bool CheckIfHigherThanFirstLine  (Vector3 point)
//	{
//		return point.y > k1 * point.x + b1;
//	}
	
//	public bool CheckIfHigherThanSecondLine (Vector3 point)
//	{
//		return point.y > k2 * point.x + b2;
//	}
	
	
	public bool CheckIfInSector (Vector3 point)
	{
		Debug.Log ("CHECK IN SECTOR " + point);
		
		if (k2 > 0)
			return (point.y >= k1 * point.x + b1) && (point.y <= k2 * point.x + b2);
		else
			return (point.y >= k1 * point.x + b1) && (point.y >= k2 * point.x + b2);
	}
}