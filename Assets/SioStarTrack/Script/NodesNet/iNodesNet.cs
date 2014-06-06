using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public interface iNodesNet{
	void ChangeNetNodesState (bool isEnabled);
	void ShowElementNetNode (Vector3 elementPostion);
	void HideElementNetNode ();
	Vector3 GetClothestNetNodePos (Vector3 elementPos);
	void Clear();
	List <Vector3> GetActiveNodesPositions ();
	void SetActiveNodes (List <Vector3> activeNodesArray);
	void UpdateActiveNodePosition (Vector3 oldPosition, Vector3 newPosition);
	 List <Vector3> GetGenPositions (STPattern pattern, Vector3 nodePosition, int prevPatternPos, List <int> paternVects, int branchLength);
	
	void BuildWithPrefab (STNetNode nodePrefab, float density, int nElementsNodes);

	void CheckActiveNodesForSolution (List <Vector3> leafPoses);

	void Refresh ();
}


