using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STPattern
{
	List <int> verticesArray;


	public int minBranches = 1;
	public int maxBranches = 1;

	public int minLength = 1;
	public int maxLength = 1;

	
	public STPattern()
	{
		verticesArray = new List<int> ();
	}
	
	public STPattern (List <int> array)
	{
		verticesArray = array;
	}
	
	public void AddVertics (int vertN)
	{
		verticesArray.Add (vertN);
	}
	
	public List <int> GetVertices ()
	{
		return verticesArray;
	}
	
}

