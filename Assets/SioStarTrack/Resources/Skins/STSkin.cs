using UnityEngine;
using System.Collections;

public class STSkin : MonoBehaviour {


	public Color BackgroundColor 				= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	// State: Unactive                                      
	public Color RootColor 						= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	public Color NodeColor 						= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	public Color LeafColor 						= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	public Color LineColor 						= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	// State: Active                                        
	public Color NodeSelectedColor 				= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	public Color NodeRootSelectedColor 			= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	public Color NodeActiveSelectColor 			= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	public Color LeafSelectedColor 				= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	public Color LinkSelectedColor 				= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	// Star                                                 
	public Color StarColor 						= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	// Pulsar                                               
	public Color PulsarColor 					= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	// Radius                                               
	public Color RadiusColor 					= new Color ( 0.0f, 0.0f, 0.0f, 1.00f );
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
