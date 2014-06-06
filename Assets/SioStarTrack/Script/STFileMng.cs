using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;




	public class STFileMng
	{
	
	public static string AppDocumentPath = Application.persistentDataPath;
	public static string AppDataPath = Application.dataPath;
	public static string DirSeparator = Path.DirectorySeparatorChar.ToString();
	public static string LevelsExtention = ".lvl";
	
	public static bool SaveData(STSerializedLevel obj, ProtoSerialization serializer, string fileName, bool shouldRewrite)
    {
		string fullPath = Path.Combine (AppDocumentPath, (fileName  + LevelsExtention));
		//Debug.Log("Save Serialized Data to : " + fullPath);
		bool ret = false;
		
        if ( File.Exists(fullPath) ){
			
			Debug.Log("Save Serialized Data Delete Old File");
			if (shouldRewrite == false)
				return false;
			
			
			File.Delete(fullPath);
		}

		FileStream fs = new FileStream (fullPath, FileMode.Create);
		
		if (fs != null)
		{
			
			//ProtoSerialization pSerializer = serializer as ProtoSerialization;
            serializer.Serialize(fs, obj);
			
//			if (pSerializer.CanSerialize (typeof ( STSerializedLevel)))
//			{
//				Debug.Log ("CAN SER");
//			}
//			else
//				Debug.Log ("CAN NOT SER");
//			
			ret = true;
			

			Debug.Log("Save Serialization Complete");
			if (fs != null)
				fs.Close();
		}
		else
			Debug.Log ("FS ERROR");
			
        
		return ret;
    }

    public static bool LoadData(string fileName, ProtoSerialization serializer, out STSerializedLevel out_obj )
    {
		string fullPath = Path.Combine (AppDocumentPath, (fileName + LevelsExtention));
		bool ret = false;
		out_obj = null;
        if (!File.Exists(fullPath)){
			Debug.Log("Load Serialization File Not Exist At Path");
			return ret;
		}

        FileStream fs = null;
	
			Debug.Log("Load Serialization File");
			fs = File.OpenRead(fullPath);
			
			out_obj = serializer.Deserialize (fs, null, typeof (STSerializedLevel)) as STSerializedLevel;
		
			ret = true;
   
			Debug.Log("Load Serialization Complete");
			if ( fs != null ){
            	fs.Close();
			
       		 }
        return ret;
    }
	
	public static void DeleteData (string fileName)
	{
		string fullPath = Path.Combine (AppDocumentPath, (fileName + LevelsExtention));
	//	Debug.Log (fullPath);
		File.Delete (fullPath);
	}
	
	
  	public static string [] GetFilesList ()
	{
		//Debug.Log ("FL");
		string[] levelsArray = Directory.GetFiles (AppDocumentPath, "*.lvl");
		return levelsArray;
	}
	
	
	}


