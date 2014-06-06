using UnityEngine;
using System.Collections;
//using UnityEditor;

//[RequireComponent (typeof (TextMesh))]
public class InfoHUD : MonoBehaviour {
	
	public  float updateInterval = 0.5F;
 //	string add_string_ = "";
	//string version_number_ = string.Format("Ver.: {0} ", GameCore.BuildVersion() );
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	string format;
	//TextMesh textMesh = null;
	
	// Use this for initialization
//	public void AddonString(string add_str){
//		add_string_ = add_str;
//	}

	void OnGUI ()
	{
//		Debug.Log ("GUI");


		GUI.Label (new Rect (Screen.height / 2, 0, 20, 20) , format);

	}
	
	void Start () {
		Debug.Log ("START");
		//version_number_ = version_number_ + PlayerSettings.bundleVersion +"\n";
	//	textMesh = GetComponent<TextMesh>();
		timeleft = updateInterval;
	//	textMesh.transform.localPosition = new Vector3 (0, Screen.width / 2.2f, 200);
		//version_number_ = version_number_ + "\n" + Settings.APP_STREAMING + "\n" + Settings.DeviceScreenType.ToString() + "\n";
		//version_number_ = version_number_ + "\n";
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
	    accum += Time.timeScale/Time.deltaTime;
	    ++frames;
	 
	    // Interval ended - update GUI text and start new interval
	    if( timeleft <= 0.0 )
	    {
	        // display two fractional digits (f2 format)
			float fps = accum/frames;
			format = System.String.Format("{0:F0} FPS",fps);
			//textMesh.text = version_number_ + add_string_ + format;		 
			//textMesh.text = format;		 
			if(fps < 30){				
			//	textMesh.renderer.material.color = Color.yellow;
			}
			else if(fps < 10){
			//	textMesh.renderer.material.color = Color.red;
			}
			else{
			//	textMesh.renderer.material.color = Color.green;
			}
			//	DebugConsole.Log(format,level);
	        timeleft = updateInterval;
	        accum = 0.0F;
	        frames = 0;
	    }
	}
}
