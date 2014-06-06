using UnityEngine;
using System.Collections;

using System.Collections.Generic;


public enum GenMode
{
	patternMode,
	randomMode,
}

public enum SymetryMode
{
	symetry_0,
	symetry_2,
	symetry_2fliped,
	symetry_4,
}

public class STPatternManager
{
	
	
	static STPatternManager _instance = null;
	
		List <STPattern> firstLevelPatterns;
		List <STPattern> secondLevelPatterns;
		List <STPattern> thirdLevelPatterns;

		List <STPattern> randomPaterns;

	SymetryMode symetryMode;
	int levelDepth;
	
	GenMode genMode;
	
	
	public static void SetMode (GenMode mode)
	{
		STPatternManager.GetInstance().genMode = mode;
	}

	public static GenMode GetMode ()
	{
		return STPatternManager.GetInstance().genMode;
	}
	
	public static STPatternManager GetInstance()
	{
		if (_instance == null)
		{
			_instance = new STPatternManager ();
			_instance.setPatterns ();	
		}
		return _instance;
	}
	
	
	void setPatterns ()
	{
		firstLevelPatterns = new List<STPattern> ();
		secondLevelPatterns = new List<STPattern> ();
		thirdLevelPatterns = new List<STPattern> ();
		randomPaterns = new List<STPattern> (5);
		
		setFirstLevelPatterns();
		setSecondLevelPatterns();
		setThirdLevelPatterns();
		setRandomPatterns();
	}

	void setRandomPatterns ()
	{
		for (int i = 0; i < 6; i++)
		{
			STPattern pattern = new STPattern ();
			randomPaterns.Add (pattern);
		}
	

	}
	
	void setFirstLevelPatterns ()
	{
		STPattern p = new STPattern();
		
		p.AddVertics (10);
		p.AddVertics (12);
		p.AddVertics (14);
		p.AddVertics (16);
		
		firstLevelPatterns.Add (p);
	}
	
	void setSecondLevelPatterns ()
	{
		STPattern p = new STPattern();
		
		
		p.AddVertics (10);
		p.AddVertics (16);
		
		secondLevelPatterns.Add (p);
	}
	
	void setThirdLevelPatterns ()
	{
		STPattern p = new STPattern();
		
		p.AddVertics (1);
		p.AddVertics (2);
		p.AddVertics (8);
		
		thirdLevelPatterns.Add (p);
	}

	static public int GetDepth()
	{
		return STPatternManager.GetInstance().levelDepth;
	}

	static List <int> GetPatternVertices (STPattern pattern)
	{
		return pattern.GetVertices();
	}
	
	public static STPattern GetPattern (int depth)
	{
		
		if (_instance.genMode == GenMode.randomMode)
		{
			return STPatternManager.GetInstance().randomPaterns [depth - 1];
		}
		
		if (depth == 1)
					return STPatternManager.GetInstance().firstLevelPatterns[0];
				else
				if (depth == 2)
					return STPatternManager.GetInstance().secondLevelPatterns[0];
				else
				if (depth == 3)
					return STPatternManager.GetInstance().thirdLevelPatterns[0];
				else
					return STPatternManager.GetInstance().firstLevelPatterns[0];
	}

	public static void SetGenParameters (int depth, List <int> minDepthValues, List <int> maxDepthValues, List <int> minLengthValues, List <int> maxLengthValues, SymetryMode mode)
	{
		STPatternManager.GetInstance().levelDepth = depth;

		STPatternManager.GetInstance().symetryMode = mode;

		for (int i = 0; i < depth; i++)
		{
			STPatternManager.GetInstance().randomPaterns [i].minBranches = minDepthValues[i];
			STPatternManager.GetInstance().randomPaterns [i].maxBranches = maxDepthValues[i];

			STPatternManager.GetInstance().randomPaterns [i].minLength = minLengthValues[i];
			STPatternManager.GetInstance().randomPaterns [i].maxLength = maxLengthValues[i];

	//		Debug.Log (minLengthValues[i] + " " + maxLengthValues[i]);

//			Debug.Log (minDepthValues[i]);
		}
	}

	public static SymetryMode GetSymetryMode ()
	{
		return STPatternManager.GetInstance().symetryMode;
	}
}

