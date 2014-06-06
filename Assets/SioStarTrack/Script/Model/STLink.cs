#region Usings

using UnityEngine;

#endregion

public class STLink : MonoBehaviour {
    #region Properties

    public GameObject lineSelSprite;
    public GameObject lineUnselSprite;


    private Vector2 parent2D;
    private Vector2 pos2D;

    public float animProgress = 0;
    public bool animFlag = false;

    private float zCoord;

    //float textSize = 1.0f;
    private float linkLenght;

    #endregion

    #region Methods

    public void ChangeState( STATE_T val ) {

        //Debug.Log( val );

        switch ( val ) {
            case ( STATE_T.collectableSelected ):
            case ( STATE_T.simpleSelected ):
            case ( STATE_T.rootSelected ): {

                break;
            }
            case ( STATE_T.simple ):
            case ( STATE_T.collectable ): {

                break;
            }
        }
    }


    public float GetLength( Vector3 parentPos, Vector3 nodePos ) {
        parent2D.x = parentPos.x;
        parent2D.y = parentPos.y;
        pos2D.x = nodePos.x;
        pos2D.y = nodePos.y;
        linkLenght = Vector2.Distance( parent2D, pos2D );
        return linkLenght;
    }

//    public void SetSelectLinkScale( float yScale ) {
//        lineSelSprite.transform.localScale = new Vector3(
//                lineSelSprite.transform.localScale.x,
//                yScale,
//                lineSelSprite.transform.localScale.z );
//    }

    public float UpdatePos( Vector3 parentPos, Vector3 nodePos ) {
        linkLenght = 1;
        parent2D.x = parentPos.x;
        parent2D.y = parentPos.y;
        pos2D.x = nodePos.x;
        pos2D.y = nodePos.y;
        linkLenght = Vector2.Distance( parent2D, pos2D );
        linkLenght -= 25;
        //Debug.Log (textSize);
        lineUnselSprite.transform.localScale = new Vector3( 0.3f, linkLenght / 64, 1.0f );
        //  lineUnselSprite.transform.position = new Vector3();
        lineUnselSprite.transform.position = new Vector3(parent2D.x, parent2D.y, 0);
        lineSelSprite.transform.position = new Vector3(parent2D.x, parent2D.y, -1);
        return linkLenght;
    }


    private void Start() {
        SpriteRenderer rendUnSel = lineUnselSprite.GetComponent<SpriteRenderer>();
        SpriteRenderer rendSel = lineSelSprite.GetComponent<SpriteRenderer>();

        //textSize = rendSel.sprite.texture.height;
        rendUnSel.color = STLevel.GetCurrentSkin().LineColor;
        rendSel.color = STLevel.GetCurrentSkin().LinkSelectedColor;

       
        //rendUnSel.material.color = ;
        //rendSel.material.color = ;

        //lineSelSprite.transform.localScale = new Vector3 (0.3f, 0.0f, 1.0f);
    }

    private void Update() {
        if ( animFlag ) {
        }
    }

    #endregion
}