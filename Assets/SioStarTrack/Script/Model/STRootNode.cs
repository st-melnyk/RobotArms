#region Usings

using UnityEngine;

#endregion

public class STRootNode : STNode {
    #region Properties

    protected static STRootNode mInstance = null;

    public STNode activeNode;
    public STNode prevActiveNode;
    public STNode prevAnimActiveNode;

    public int hideAnimationsCount = 0;
    public int showAnimationsCount = 0;
    public int scaleAnimationsCount = 0;

    public bool isShowingBranchAnimation = false;
    public bool isShowingAnyBranchAnimation = false;

    //public int showNodesCount = 0;
    public bool isHidedAllNodes = true;
    public bool isMadeScaleAnimation = true;
    public int activeNodesCount = 0;

    public bool selectionContainsActiveNodes = false;

    public int treeDepth;

    protected float NetWidth;

    private bool editMode = true;
    //public STNodesNet NodesNet;

    public STNode NodePref;

    #endregion

    #region Methods


    public void SetTreeDepth( int depth ) {
        if ( treeDepth < depth )
            treeDepth = depth;
    }

    public bool CheckForIntersection( float X1, float Y1, float X2, float Y2, float X3, float Y3 ) {
        bool result = false;
        float X4 = this.transform.position.x;
        float Y4 = this.transform.position.y;
        foreach ( STNode child in mRotor.mChilds ) {
            result = child.CheckForIntersection( X1, Y1, X2, Y2, X4, Y4 );
            if ( result ) {
                return result;
            }
        }
        return result;
    }

    public void GenerateRandomLevel( int depth ) {
        treeDepth = 0;
        GenNextNode( depth, 1, 0, 0 );
       
        //CutBranches (6);
    }

    public STNode GetActiveNode() {
        return activeNode;
    }


    public bool GetEditMode() {
        return editMode;
    }

    public void Reset() {
        if ( NodeType == NODE_T.COLLECTABLE ) {
            this.changeNodeType();
        }
        brenchLength = 0;
        this.RemoveChild();
        this.transform.position = new Vector3( 0, 0, 0 );
        //HideSelection ();
        HideAllGUIS();
        activeNode = null;
    }


    public void Rotate( float angleDeg ) {
        if ( STLevel.GetRootNode().activeNode != null ) {
            STLevel.GetRootNode().activeNode.mRotor.transform.rotation = Quaternion.Euler( 0, 0, angleDeg );
            //	STLevel.GetRootNode().activeNode.UpdateLinkPosition(STLevel.GetRootNode().activeNode.transform.position);
        }
    }


    public void SetActiveNode( STNode node ) {
        activeNode = node;
    }

    public void SetEditMode( bool mode ) {
        editMode = mode;
        this.HideAllGUIS();
        //STLevel.GetRootNode().startHideAnimationFromLeafes();
    }


    private void Start() {
        //mVisual.mesh.renderer.material.color = Color.black;
        nodeDepth = 0;
    }

    private void Update() {
    }

    #endregion
}