using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STSkinsManager : MonoBehaviour {

	int current_skin_index_ = 0;
	List<STSkin> skins_ = new List<STSkin>();

	// Use this for initialization
	void Start () {
	
	}

	public STSkinsManager ()
	{
		skins_.Clear();

		setupFirstSkin();
		setupSecondSkin();
		setupThirdSkin();
		setup4thSkin();
		setup5thSkin();
		setup6thSkin();
	}

	public STSkin GetCurrentSkin()
	{
		return skins_[current_skin_index_];
	}

	public void ChangeCurrentSkin (int index)
	{
		if (index < skins_.Count)
			current_skin_index_ = index;
	}

	void setupFirstSkin ()
	{
		STSkin sk1 = new STSkin();
		sk1.BackgroundColor 				= new Color ( 0.21f, 0.25f, 0.35f, 1.00f );
		
		sk1.RootColor 						= new Color ( 0.02f, 0.13f, 0.22f, 1.00f );
		sk1.NodeColor 						= new Color ( 0.02f, 0.13f, 0.22f, 1.00f );
		sk1.LeafColor 						= new Color ( 0.02f, 0.13f, 0.22f, 1.00f );
		sk1.LineColor 						= new Color ( 0.02f, 0.13f, 0.22f, 1.00f );
		
		sk1.NodeSelectedColor 				= new Color ( 0.95f, 0.8f, 0.53f, 1.00f );
		sk1.NodeRootSelectedColor 			= new Color ( 0.95f, 0.8f, 0.53f, 1.00f );
		sk1.NodeActiveSelectColor 			= new Color ( 0.02f, 0.13f, 0.22f, 1.00f );
		sk1.LeafSelectedColor 				= new Color ( 0.95f, 0.8f, 0.53f, 1.00f );
		sk1.LinkSelectedColor 				= new Color ( 0.95f, 0.8f, 0.53f, 1.00f );
		
		sk1.StarColor 						= new Color ( 0.95f, 0.71f, 0.71f, 1.00f );
		sk1.PulsarColor 					= new Color ( 0.29f, 0.3f, 0.38f, 1.00f );
		sk1.RadiusColor 					= new Color ( 0f, 0f, 0f, 1.00f );
		skins_.Add(sk1);
	}

	void setupSecondSkin ()
	{
		STSkin sk2 = new STSkin();
		sk2.BackgroundColor 				= new Color ( 1f, 0.87f, 0.77f, 1.00f );
                                                        
		sk2.RootColor 						= new Color ( 0.11f, 0.29f, 0.47f, 1.00f );
		sk2.NodeColor 						= new Color ( 0.11f, 0.29f, 0.47f, 1.00f );
		sk2.LeafColor 						= new Color ( 0.11f, 0.29f, 0.47f, 1.00f );
		sk2.LineColor 						= new Color ( 0.11f, 0.29f, 0.47f, 1.00f );
                                                        
		sk2.NodeSelectedColor 				= new Color ( 1f, 0.6f, 0.2f, 1.00f );
		sk2.NodeRootSelectedColor 			= new Color ( 1f, 0.6f, 0.2f, 1.00f );
		sk2.NodeActiveSelectColor 			= new Color ( 0.11f, 0.29f, 0.47f, 1.00f );
		sk2.LeafSelectedColor 				= new Color ( 1f, 0.6f, 0.2f, 1.00f );
		sk2.LinkSelectedColor 				= new Color ( 1f, 0.6f, 0.2f, 1.00f );
                                                        
		sk2.StarColor 						= new Color ( 0.1f, 0.64f, 1f, 1.00f );                                       
		sk2.PulsarColor 					= new Color ( 0.29f, 0.3f, 0.38f, 1.00f );                                            
		sk2.RadiusColor 					= new Color ( 0f, 0f, 0f, 1.00f );
		skins_.Add(sk2);
	}

	void setupThirdSkin ()
	{
		STSkin sk3 = new STSkin();
		sk3.BackgroundColor 				= new Color ( 0.68f, 0.68f, 0.68f, 1.00f );
                                    
		sk3.RootColor 						= new Color ( 0.35f, 0.35f, 0.35f, 1.00f );
		sk3.NodeColor 						= new Color ( 0.35f, 0.35f, 0.35f, 1.00f );
		sk3.LeafColor 						= new Color ( 0.35f, 0.35f, 0.35f, 1.00f );
		sk3.LineColor 						= new Color ( 0.35f, 0.35f, 0.35f, 1.00f );
                                      
		sk3.NodeSelectedColor 				= new Color ( 1f, 1f, 1f, 1.00f );
		sk3.NodeRootSelectedColor 			= new Color ( 1f, 1f, 1f, 1.00f );
		sk3.NodeActiveSelectColor 			= new Color ( 0.35f, 0.35f, 0.35f, 1.00f );
		sk3.LeafSelectedColor 				= new Color ( 1f, 1f, 1f, 1.00f );
		sk3.LinkSelectedColor 				= new Color ( 1f, 1f, 1f, 1.00f );
                                               
		sk3.StarColor 						= new Color ( 1f, 0.89f, 0.07f, 1.00f );                                             
		sk3.PulsarColor 					= new Color ( 0.74f, 0.72f, 0.56f, 1.00f );                                              
		sk3.RadiusColor 					= new Color ( 0.7f, 0.7f, 0.7f, 1.00f );

		skins_.Add(sk3);
	}

	void setup4thSkin ()
	{
		STSkin sk4 = new STSkin();
		sk4.BackgroundColor 				= new Color ( 0.96f, 0.96f, 0.96f, 1.00f );
                                   
		sk4.RootColor 						= new Color ( 0.65f, 0.66f, 0.67f, 1.00f );
		sk4.NodeColor 						= new Color ( 0.65f, 0.66f, 0.67f, 1.00f );
		sk4.LeafColor 						= new Color ( 0.65f, 0.66f, 0.67f, 1.00f );
		sk4.LineColor 						= new Color ( 0.65f, 0.66f, 0.67f, 1.00f );
                                      
		sk4.NodeSelectedColor 				= new Color ( 0.35f, 0.35f, 0.36f, 1.00f );
		sk4.NodeRootSelectedColor 			= new Color ( 0.35f, 0.35f, 0.36f, 1.00f );
		sk4.NodeActiveSelectColor 			= new Color ( 0.65f, 0.66f, 0.67f, 1.00f );
		sk4.LeafSelectedColor 				= new Color ( 0.35f, 0.35f, 0.36f, 1.00f );
		sk4.LinkSelectedColor 				= new Color ( 0.35f, 0.35f, 0.36f, 1.00f );
                                            
		sk4.StarColor 						= new Color ( 0.93f, 0.16f, 0.48f, 1.00f );                                             
		sk4.PulsarColor 					= new Color ( 0.95f, 0.72f, 0.81f, 1.00f );                                            
		sk4.RadiusColor 					= new Color ( 0f, 0f, 0f, 1.00f );

		skins_.Add(sk4);
	}
	
	void setup5thSkin ()
	{
		STSkin sk5 = new STSkin();
		sk5.BackgroundColor 				= new Color ( 0.01f, 0.32f, 0.4f, 1.00f );
                                                        
		sk5.RootColor 						= new Color ( 0.02f, 0.49f, 0.62f, 1.00f );
		sk5.NodeColor 						= new Color ( 0.02f, 0.49f, 0.62f, 1.00f );
		sk5.LeafColor 						= new Color ( 0.02f, 0.49f, 0.62f, 1.00f );
		sk5.LineColor 						= new Color ( 0.02f, 0.49f, 0.62f, 1.00f );
                                                        
		sk5.NodeSelectedColor 				= new Color ( 0.28f, 0.87f, 1f, 1.00f );
		sk5.NodeRootSelectedColor 			= new Color ( 0.28f, 0.87f, 1f, 1.00f );
		sk5.NodeActiveSelectColor 			= new Color ( 0.02f, 0.49f, 0.62f, 1.00f );
		sk5.LeafSelectedColor 				= new Color ( 0.28f, 0.87f, 1f, 1.00f );
		sk5.LinkSelectedColor 				= new Color ( 0.28f, 0.87f, 1f, 1.00f );
                                                        
		sk5.StarColor 						= new Color ( 1f, 0.32f, 0.32f, 1.00f );                                             
		sk5.PulsarColor 					= new Color ( 0.31f, 0.32f, 0.38f, 1.00f );        
		sk5.RadiusColor 					= new Color ( 0.07f, 0.36f, 0.45f, 1.00f );
                                                        
		skins_.Add(sk5);                                
	}
	
	void setup6thSkin ()
	{
		STSkin sk6 = new STSkin();
		sk6.BackgroundColor 				= new Color ( 0f, 0.39f, 0.39f, 1.00f );
                                                        
		sk6.RootColor 						= new Color ( 0.11f, 0.45f, 0.45f, 1.00f );
		sk6.NodeColor 						= new Color ( 0.11f, 0.45f, 0.45f, 1.00f );
		sk6.LeafColor 						= new Color ( 0.11f, 0.45f, 0.45f, 1.00f );
		sk6.LineColor 						= new Color ( 0.11f, 0.45f, 0.45f, 1.00f );
                                                        
		sk6.NodeSelectedColor 				= new Color ( 0.2f, 0.8f, 0.8f, 1.00f );
		sk6.NodeRootSelectedColor 			= new Color ( 0.2f, 0.8f, 0.8f, 1.00f );
		sk6.NodeActiveSelectColor 			= new Color ( 0.11f, 0.45f, 0.45f, 1.00f );
		sk6.LeafSelectedColor 				= new Color ( 0.2f, 0.8f, 0.8f, 1.00f );
		sk6.LinkSelectedColor 				= new Color ( 0.2f, 0.8f, 0.8f, 1.00f );
                                                        
		sk6.StarColor 						= new Color ( 1f, 0.45f, 0f, 1.00f );                                          
		sk6.PulsarColor 					= new Color ( 0.2f, 0.4f, 0.31f, 1.00f );        
		sk6.RadiusColor 					= new Color ( 0.1f, 0.45f, 0.45f, 1.00f );
                                                        
		skins_.Add(sk6);                                
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
