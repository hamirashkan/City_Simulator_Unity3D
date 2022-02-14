/*-----------------------------------------------------------------------------------------------------------------------------
 *-----------------------------------------------------------------------------------------------------------------------------
 *----------------------------------------------------------------------------------------------------------------------------- 
 *-----------------------------------------------------------------------------------------------------------------------------
 *----------------------------------------------------------------------------------------------------------------------------- 
 *--------------------------------BY MATTS CREATION 2016-----------------------------------------------------------------------
 *----------------------------------------------------------------------------------------------------------------------------- 
 *----------------------------Assign This Script Only ONCE!--------------------------------------------------------------------
 *----------------------------Assign This On Main Player or Main Camera--------------------------------------------------------
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;


[AddComponentMenu("Matts_Creations/Save-It_Main System")]

public class Save_Load_SYSTEM_V2 : MonoBehaviour {


	[Header("SAVE and LOAD SYSTEM BY MATT 2016")]
	public string MainSave_LoadPath;
	private string AlternativeSave_LoadPath;
	
	[Header("All Available Objects To Save")]
	public List<GameObject> AllObjectsToSave_Load;
	
	[Header("Ignore Objects By Tag")]
	public string IgnoreObjects;
	public List<GameObject> IgnoredObjects;
	[Header("Ignore This Object (Self)")]
	public bool IgnoreSelf = true;

	public List<GameObject> AllObjectsInScene;
	
	private bool Saving;
	private bool ReplaceSavedSlot;

	private bool Loading;
	

	private int CounterToLoadAndSaveEachObject;

	private string CurrentSaving;

	void Start () 
	{
		//----------------------------------------------------------------------------------
		//----------Checking if the path is not empty or if exist-----------------------
		//----------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------
		if(string.IsNullOrEmpty(MainSave_LoadPath))
		{
            if (Directory.Exists(Application.dataPath + "/Data"))
            {
                if (Directory.Exists(Application.dataPath + "/Data/Save"))
                {
                    if (File.Exists(Application.dataPath + "/Data/Save/Objects.txt"))
                    {
                        MainSave_LoadPath = Application.dataPath + "/Data/Save"; 

                        Debug.LogWarning("*Save-It script [WARNING] message: Main path to save and load datas has been set to main start up path*");
                        Debug.LogWarning("*Save-It additional message: Main path to save and load has been set to: " + MainSave_LoadPath + "  *");
                    }
                    else
                    {
                        MainSave_LoadPath = Application.dataPath + "/Data/Save"; 

                        File.Create(MainSave_LoadPath + "/Objects.txt").Dispose();

                        Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms");
                        Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Position");
                        Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Rotation");
                        Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Scale");
                        Directory.CreateDirectory(MainSave_LoadPath + "/_Identity");

                        string PosDirectory = MainSave_LoadPath + "/_Transforms/Position";
                        string RotDirectory = MainSave_LoadPath + "/_Transforms/Rotation";
                        string ScaDirectory = MainSave_LoadPath + "/_Transforms/Scale";
                        string IdentDirectory = MainSave_LoadPath + "/_Identity";

                        File.Create(PosDirectory + "/Pos_X.txt").Dispose();
                        File.Create(PosDirectory + "/Pos_Y.txt").Dispose();
                        File.Create(PosDirectory + "/Pos_Z.txt").Dispose();

                        File.Create(RotDirectory + "/Rot_X.txt").Dispose();
                        File.Create(RotDirectory + "/Rot_Y.txt").Dispose();
                        File.Create(RotDirectory + "/Rot_Z.txt").Dispose();
                        File.Create(RotDirectory + "/Rot_W.txt").Dispose();

                        File.Create(ScaDirectory + "/Sca_X.txt").Dispose();
                        File.Create(ScaDirectory + "/Sca_Y.txt").Dispose();
                        File.Create(ScaDirectory + "/Sca_Z.txt").Dispose();

                        File.Create(IdentDirectory + "/Type_.txt").Dispose();
                        File.Create(IdentDirectory + "/Texture_.txt").Dispose();

                        Debug.LogWarning("*Save-It script [WARNING] message: Main path to save and load datas has been set to main start up path*");
                        Debug.LogWarning("*Save-It additional message: Main path to save and load has been set to: " + MainSave_LoadPath + "  *");
                    }
                }
                else
                {
                    Directory.CreateDirectory(Application.dataPath + "/Data");
                    Directory.CreateDirectory(Application.dataPath + "/Data/Save");

                    MainSave_LoadPath = Application.dataPath + "/Data/Save"; 

                    File.Create(MainSave_LoadPath + "/Objects.txt").Dispose();

                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Position");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Rotation");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Scale");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Identity");

                    string PosDirectory = MainSave_LoadPath + "/_Transforms/Position";
                    string RotDirectory = MainSave_LoadPath + "/_Transforms/Rotation";
                    string ScaDirectory = MainSave_LoadPath + "/_Transforms/Scale";
                    string IdentDirectory = MainSave_LoadPath + "/_Identity";

                    File.Create(PosDirectory + "/Pos_X.txt").Dispose();
                    File.Create(PosDirectory + "/Pos_Y.txt").Dispose();
                    File.Create(PosDirectory + "/Pos_Z.txt").Dispose();

                    File.Create(RotDirectory + "/Rot_X.txt").Dispose();
                    File.Create(RotDirectory + "/Rot_Y.txt").Dispose();
                    File.Create(RotDirectory + "/Rot_Z.txt").Dispose();
                    File.Create(RotDirectory + "/Rot_W.txt").Dispose();

                    File.Create(ScaDirectory + "/Sca_X.txt").Dispose();
                    File.Create(ScaDirectory + "/Sca_Y.txt").Dispose();
                    File.Create(ScaDirectory + "/Sca_Z.txt").Dispose();

                    File.Create(IdentDirectory + "/Type_.txt").Dispose();
                    File.Create(IdentDirectory + "/Texture_.txt").Dispose();

                    Debug.LogWarning("*Save-It script [WARNING] message: Main path to save and load datas has been set to main start up path*");
                    Debug.LogWarning("*Save-It additional message: Main path to save and load has been set to: " + MainSave_LoadPath + "  *");
                }
            }
            else
            {
                Directory.CreateDirectory(Application.dataPath + "/Data");
                Directory.CreateDirectory(Application.dataPath + "/Data/Save");

                MainSave_LoadPath = Application.dataPath + "/Data/Save"; 

                File.Create(MainSave_LoadPath + "/Objects.txt").Dispose();

                Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms");
                Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Position");
                Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Rotation");
                Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Scale");
                Directory.CreateDirectory(MainSave_LoadPath + "/_Identity");

                string PosDirectory = MainSave_LoadPath + "/_Transforms/Position";
                string RotDirectory = MainSave_LoadPath + "/_Transforms/Rotation";
                string ScaDirectory = MainSave_LoadPath + "/_Transforms/Scale";
                string IdentDirectory = MainSave_LoadPath + "/_Identity";

                File.Create(PosDirectory + "/Pos_X.txt").Dispose();
                File.Create(PosDirectory + "/Pos_Y.txt").Dispose();
                File.Create(PosDirectory + "/Pos_Z.txt").Dispose();

                File.Create(RotDirectory + "/Rot_X.txt").Dispose();
                File.Create(RotDirectory + "/Rot_Y.txt").Dispose();
                File.Create(RotDirectory + "/Rot_Z.txt").Dispose();
                File.Create(RotDirectory + "/Rot_W.txt").Dispose();

                File.Create(ScaDirectory + "/Sca_X.txt").Dispose();
                File.Create(ScaDirectory + "/Sca_Y.txt").Dispose();
                File.Create(ScaDirectory + "/Sca_Z.txt").Dispose();

                File.Create(IdentDirectory + "/Type_.txt").Dispose();
                File.Create(IdentDirectory + "/Texture_.txt").Dispose();

                Debug.LogWarning("*Save-It script [WARNING] message: Main path to save and load datas has been set to main start up path*");
                Debug.LogWarning("*Save-It additional message: Main path to save and load has been set to: " + MainSave_LoadPath + "  *");
            }
		}
		else
		{
			if(Directory.Exists(MainSave_LoadPath))
			{
				Debug.Log("*Save-It script [LOG] message: Your main path to save and load exist*");
				return;
			}
			else
			{
				Debug.LogError("*Save-It script [ERROR] message: Main path to save and load does not exist [ "+MainSave_LoadPath+" ] *");
				if(!Directory.Exists(MainSave_LoadPath))
				{
					Directory.CreateDirectory(MainSave_LoadPath);

                    File.Create(MainSave_LoadPath+"/Objects.txt").Dispose();

                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Position");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Rotation");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Scale");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Identity");

                    string PosDirectory = MainSave_LoadPath+"/_Transforms/Position";
                    string RotDirectory = MainSave_LoadPath+"/_Transforms/Rotation";
                    string ScaDirectory = MainSave_LoadPath+"/_Transforms/Scale";
                    string IdentDirectory = MainSave_LoadPath+"/_Identity";

                    File.Create(PosDirectory+"/Pos_X.txt").Dispose();
                    File.Create(PosDirectory+"/Pos_Y.txt").Dispose();
                    File.Create(PosDirectory+"/Pos_Z.txt").Dispose();

                    File.Create(RotDirectory+"/Rot_X.txt").Dispose();
                    File.Create(RotDirectory+"/Rot_Y.txt").Dispose();
                    File.Create(RotDirectory+"/Rot_Z.txt").Dispose();
                    File.Create(RotDirectory+"/Rot_W.txt").Dispose();

                    File.Create(ScaDirectory+"/Sca_X.txt").Dispose();
                    File.Create(ScaDirectory+"/Sca_Y.txt").Dispose();
                    File.Create(ScaDirectory+"/Sca_Z.txt").Dispose();

                    File.Create(IdentDirectory+"/Type_.txt").Dispose();
                    File.Create(IdentDirectory+"/Texture_.txt").Dispose();
				}
				Debug.LogWarning("*Save-It script [WARNING] message: Main path to save and load datas has been created as "+MainSave_LoadPath+" *");
				return;
			}
		}
		//----------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------
		//----------------------------------------------------------------------------------
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.J))
		{
			SAVE_();
		}
		if(Input.GetKeyDown(KeyCode.L))
		{
			LOAD_();
		}

		
		
		
		if(Loading)
		{
                if(AllObjectsToSave_Load.ToArray().Length == File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length)
				{
					Loading = false;
					Debug.Log("*Save-It script [LOG] message: Datas has been successfully loaded*");
                    Debug.Log("*Save-It additional message: Datas has been loaded from: "+MainSave_LoadPath+"  *");

					CounterToLoadAndSaveEachObject = 0;
					
					AllObjectsToSave_Load.Clear();
				}
				else
				{
					CreateBlock_Load();
				}
			
		}
		
		




		if(Saving)
		{
			Refresh_Saving_ObjectTransform();
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
	}
	
	
	
	
	void SAVE_()
	{

		GameObject gm = new GameObject ();
		gm.AddComponent<Camera> ();
		gm.name = "HelpCam";

		foreach(GameObject GM in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			AllObjectsInScene.Add(GM);
		}
		if(AllObjectsInScene.ToArray().Length == GameObject.FindObjectsOfType(typeof(GameObject)).Length)
		{
			foreach(GameObject GM in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				foreach(GameObject GM2 in AllObjectsInScene)
				{
					if(GM.name == GM2.name && GM.transform.position != GM2.transform.position)
					{
                        GM.name = GM.name+" Copy_"+Random.Range(1,999).ToString()+"_Double_"+File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length.ToString()+GameObject.FindObjectsOfType(typeof(GameObject)).Length.ToString();
					}
				}
			}
		}

		foreach(GameObject OneObject in GameObject.FindGameObjectsWithTag(IgnoreObjects))
		{
			IgnoredObjects.Add(OneObject);

			OneObject.SetActive(false);
			Time.timeScale = 0;
		}
		
		if(GameObject.FindObjectOfType(typeof(GameObject)) != null)
		{
			foreach(GameObject OneObject in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if(IgnoreSelf)
				{
					if(OneObject.name != transform.name && OneObject.name != "HelpCam")
					{
						//---------------IGNORE SELF OBJECT--------------------------
						AllObjectsToSave_Load.Add(OneObject);
					}
				}
				else
				{
					AllObjectsToSave_Load.Add(OneObject);
				}
			}




            if(File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length>0)
			{
				if(ReplaceSavedSlot)
				{}
				else
				{
                    AlternativeSave_LoadPath = MainSave_LoadPath;
					
                    Directory.Delete(MainSave_LoadPath,true);
					
					ReplaceSavedSlot = true;
					
					MainSave_LoadPath = AlternativeSave_LoadPath;

                    Directory.CreateDirectory(MainSave_LoadPath);

                    File.Create(MainSave_LoadPath+"/Objects.txt").Dispose();

                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Position");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Rotation");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Scale");
                    Directory.CreateDirectory(MainSave_LoadPath + "/_Identity");

                    string PosDirectory = MainSave_LoadPath+"/_Transforms/Position";
                    string RotDirectory = MainSave_LoadPath+"/_Transforms/Rotation";
                    string ScaDirectory = MainSave_LoadPath+"/_Transforms/Scale";
                    string IdentDirectory = MainSave_LoadPath+"/_Identity";

                    File.Create(PosDirectory+"/Pos_X.txt").Dispose();
                    File.Create(PosDirectory+"/Pos_Y.txt").Dispose();
                    File.Create(PosDirectory+"/Pos_Z.txt").Dispose();

                    File.Create(RotDirectory+"/Rot_X.txt").Dispose();
                    File.Create(RotDirectory+"/Rot_Y.txt").Dispose();
                    File.Create(RotDirectory+"/Rot_Z.txt").Dispose();
                    File.Create(RotDirectory+"/Rot_W.txt").Dispose();

                    File.Create(ScaDirectory+"/Sca_X.txt").Dispose();
                    File.Create(ScaDirectory+"/Sca_Y.txt").Dispose();
                    File.Create(ScaDirectory+"/Sca_Z.txt").Dispose();

                    File.Create(IdentDirectory+"/Type_.txt").Dispose();
                    File.Create(IdentDirectory+"/Texture_.txt").Dispose();
				}
			}
			else
			{
                File.Create(MainSave_LoadPath+"/Objects.txt").Dispose();

                Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms");
                Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Position");
                Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Rotation");
                Directory.CreateDirectory(MainSave_LoadPath + "/_Transforms/Scale");
                Directory.CreateDirectory(MainSave_LoadPath + "/_Identity");

                string PosDirectory = MainSave_LoadPath+"/_Transforms/Position";
                string RotDirectory = MainSave_LoadPath+"/_Transforms/Rotation";
                string ScaDirectory = MainSave_LoadPath+"/_Transforms/Scale";
                string IdentDirectory = MainSave_LoadPath+"/_Identity";

                File.Create(PosDirectory+"/Pos_X.txt").Dispose();
                File.Create(PosDirectory+"/Pos_Y.txt").Dispose();
                File.Create(PosDirectory+"/Pos_Z.txt").Dispose();

                File.Create(RotDirectory+"/Rot_X.txt").Dispose();
                File.Create(RotDirectory+"/Rot_Y.txt").Dispose();
                File.Create(RotDirectory+"/Rot_Z.txt").Dispose();
                File.Create(RotDirectory+"/Rot_W.txt").Dispose();

                File.Create(ScaDirectory+"/Sca_X.txt").Dispose();
                File.Create(ScaDirectory+"/Sca_Y.txt").Dispose();
                File.Create(ScaDirectory+"/Sca_Z.txt").Dispose();

                File.Create(IdentDirectory+"/Type_.txt").Dispose();
                File.Create(IdentDirectory+"/Texture_.txt").Dispose();
			}
			
			Saving = true;
		}
		else
		{
			Debug.LogError("*Save-It script [ERROR] message: There is no objects to save of type <GameObject> *");
		}
	}
	
	
	
	
	
	void Refresh_Saving()
	{
        
        if(File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length == AllObjectsToSave_Load.ToArray().Length)
		{
			foreach(GameObject EachIgnoredObject in IgnoredObjects)
			{
				EachIgnoredObject.SetActive(true);
				Time.timeScale = 1;
			}
			



			Saving = false;

			Debug.Log("*Save-It script [LOG] message: Datas has been successfully saved*");
            Debug.Log("*Save-It additional message: Datas has been saved to: "+MainSave_LoadPath+"  *");
			Destroy(GameObject.Find("HelpCam"));

			ReplaceSavedSlot = false;
			
			CounterToLoadAndSaveEachObject = 0;

			IgnoredObjects.Clear();
			AllObjectsToSave_Load.Clear();
			AllObjectsInScene.Clear();
		}
		else
		{
			Saving = true;
		}
	}
	
	void Refresh_Saving_ObjectTransform()
	{

        string PosDirectory = MainSave_LoadPath+"/_Transforms/Position";
        string RotDirectory = MainSave_LoadPath+"/_Transforms/Rotation";
        string ScaDirectory = MainSave_LoadPath+"/_Transforms/Scale";
        string IdentDirectory = MainSave_LoadPath+"/_Identity";

        string FileAdder_Objects = File.ReadAllText(MainSave_LoadPath + "/Objects.txt");

        string FileAdder_Pos_x = File.ReadAllText(PosDirectory+ "/Pos_X.txt");
        string FileAdder_Pos_y = File.ReadAllText(PosDirectory+ "/Pos_Y.txt");
        string FileAdder_Pos_z = File.ReadAllText(PosDirectory+ "/Pos_Z.txt");

        string FileAdder_Rot_x = File.ReadAllText(RotDirectory+ "/Rot_X.txt");
        string FileAdder_Rot_y = File.ReadAllText(RotDirectory+ "/Rot_Y.txt");
        string FileAdder_Rot_z = File.ReadAllText(RotDirectory+ "/Rot_Z.txt");
        string FileAdder_Rot_w = File.ReadAllText(RotDirectory+ "/Rot_W.txt");

        string FileAdder_Sca_x = File.ReadAllText(ScaDirectory+ "/Sca_X.txt");
        string FileAdder_Sca_y = File.ReadAllText(ScaDirectory+ "/Sca_Y.txt");
        string FileAdder_Sca_z = File.ReadAllText(ScaDirectory+ "/Sca_Z.txt");

        string FileAdder_Type = File.ReadAllText(IdentDirectory+"/Type_.txt");
        string FileAdder_Text = File.ReadAllText(IdentDirectory+"/Texture_.txt");

	    CurrentSaving = AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].name;


        File.WriteAllText(MainSave_LoadPath+"/Objects.txt",FileAdder_Objects+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].name+System.Environment.NewLine);


			//---------------------POSITION---------------------------------
			
        File.WriteAllText(PosDirectory+"/Pos_X.txt",FileAdder_Pos_x+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.position.x.ToString()+System.Environment.NewLine);
        File.WriteAllText(PosDirectory+"/Pos_Y.txt",FileAdder_Pos_y+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.position.y.ToString()+System.Environment.NewLine);
        File.WriteAllText(PosDirectory+"/Pos_Z.txt",FileAdder_Pos_z+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.position.z.ToString()+System.Environment.NewLine);

			//---------------------ROTATION---------------------------------
			
        File.WriteAllText(RotDirectory+"/Rot_X.txt",FileAdder_Rot_x+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.rotation.x.ToString()+System.Environment.NewLine);
        File.WriteAllText(RotDirectory+"/Rot_Y.txt",FileAdder_Rot_y+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.rotation.y.ToString()+System.Environment.NewLine);
        File.WriteAllText(RotDirectory+"/Rot_Z.txt",FileAdder_Rot_z+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.rotation.z.ToString()+System.Environment.NewLine);
        File.WriteAllText(RotDirectory+"/Rot_W.txt",FileAdder_Rot_w+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.rotation.w.ToString()+System.Environment.NewLine);

			//---------------------SCALE------------------------------------
			
        File.WriteAllText(ScaDirectory+"/Sca_X.txt",FileAdder_Sca_x+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.localScale.x.ToString()+System.Environment.NewLine);
        File.WriteAllText(ScaDirectory+"/Sca_Y.txt",FileAdder_Sca_y+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.localScale.y.ToString()+System.Environment.NewLine);
        File.WriteAllText(ScaDirectory+"/Sca_Z.txt",FileAdder_Sca_z+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].transform.localScale.z.ToString()+System.Environment.NewLine);

			//---------------------IDENTITY---------------------------------
			
	if(AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].GetComponent<MeshFilter>())
	{
        File.WriteAllText(IdentDirectory+"/Type_.txt",FileAdder_Type+AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].GetComponent<MeshFilter>().sharedMesh.name+System.Environment.NewLine);

            if (AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].GetComponent<Renderer>() != null)
            {
                if (AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].GetComponent<Renderer>().material.mainTexture != null)
                {
                    File.WriteAllText(IdentDirectory + "/Texture_.txt", FileAdder_Text + AllObjectsToSave_Load[CounterToLoadAndSaveEachObject].GetComponent<Renderer>().material.mainTexture.name + System.Environment.NewLine);
                }
                else
                {
                    File.WriteAllText(IdentDirectory + "/Texture_.txt", FileAdder_Text + "NoTexture" + System.Environment.NewLine);
                }
            }
            else
            {
                File.WriteAllText(IdentDirectory + "/Texture_.txt", FileAdder_Text + "NoTexture" + System.Environment.NewLine);
            }
	}
	else
	{
    File.WriteAllText(IdentDirectory+"/Type_.txt",FileAdder_Type+"Cube"+System.Environment.NewLine);
	}
			

        CountAdder_Save();
        Refresh_Saving();
	}
	
	//-------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------
	
	
	
	
	void LOAD_()
	{
		if(Directory.Exists(MainSave_LoadPath))
		{
            if(File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length>0)
			{
				Loading = true;
			}
			else
			{
				Debug.LogWarning("*Save-It script [WARNING] message: There is no saved datas yet on "+MainSave_LoadPath+" *");
				return;
			}
		}
		else
		{
			Debug.LogError("*Save-It script [ERROR] message: Main path to save and load does not exist!*");

			try
			{
			    Directory.CreateDirectory(MainSave_LoadPath);
				Debug.LogWarning("*Save-It script [WARNING] message: Main path to save and load datas has been created as "+MainSave_LoadPath+" *");
				return;
			}
			catch(IOException e)
			{
			    Debug.LogError("*Save-It script [ERROR] message: Cannot create this path: "+MainSave_LoadPath+" , Exception: "+e.Message+" *");
				return;
			}
		}
	}
	
	void CreateBlock_Load()
	{

		GameObject newBlock = new GameObject ();

		newBlock.AddComponent<MeshFilter> ();
		newBlock.AddComponent<MeshCollider> ();
		newBlock.AddComponent<MeshRenderer> ();
		Material mat = new Material (Shader.Find("Standard"));
		newBlock.GetComponent<Renderer> ().material = mat;
		
		string[] myDirectory = Directory.GetDirectories(MainSave_LoadPath);
		int Loader = CounterToLoadAndSaveEachObject;

        string PosDirectory = MainSave_LoadPath+"/_Transforms/Position";
        string RotDirectory = MainSave_LoadPath+"/_Transforms/Rotation";
        string ScaDirectory = MainSave_LoadPath+"/_Transforms/Scale";
        string IdentDirectory = MainSave_LoadPath+"/_Identity";
        		
        string info =  File.ReadAllLines(MainSave_LoadPath + "/Objects.txt")[CounterToLoadAndSaveEachObject];
		newBlock.name = info;

		CurrentSaving = newBlock.name;

		newBlock.SetActive (false);


		if(GameObject.Find(info) != null)
		{
			foreach(GameObject AllObjectsOfThisName in GameObject.FindObjectsOfType(typeof(GameObject)))
			{
				if(AllObjectsOfThisName.transform.name == newBlock.name)
				{
					Destroy(GameObject.Find(info));
				}
			}
		}

		AllObjectsToSave_Load.Add (newBlock);
		
        string POS_X = File.ReadAllLines(PosDirectory+"/Pos_X.txt")[Loader];
		
        string POS_Y = File.ReadAllLines(PosDirectory+"/Pos_Y.txt")[Loader];
		
        string POS_Z = File.ReadAllLines(PosDirectory+"/Pos_Z.txt")[Loader];
		
        newBlock.transform.position = new Vector3(float.Parse(POS_X),float.Parse(POS_Y),float.Parse(POS_Z));
		
        string ROT_X = File.ReadAllLines(RotDirectory+"/Rot_X.txt")[Loader];
	
        string ROT_Y = File.ReadAllLines(RotDirectory+"/Rot_Y.txt")[Loader];
		
        string ROT_Z = File.ReadAllLines(RotDirectory+"/Rot_Z.txt")[Loader];
		
        string ROT_W = File.ReadAllLines(RotDirectory+"/Rot_W.txt")[Loader];
		
        newBlock.transform.rotation = new Quaternion(float.Parse(ROT_X),float.Parse(ROT_Y),float.Parse(ROT_Z),float.Parse(ROT_W));

        string SCA_X = File.ReadAllLines(ScaDirectory+"/Sca_X.txt")[Loader];
		
        string SCA_Y = File.ReadAllLines(ScaDirectory+"/Sca_Y.txt")[Loader];
		
        string SCA_Z = File.ReadAllLines(ScaDirectory+"/Sca_Z.txt")[Loader];
		
        newBlock.transform.localScale = new Vector3(float.Parse(SCA_X),float.Parse(SCA_Y),float.Parse(SCA_Z));

        string IDENT_Type = File.ReadAllLines(IdentDirectory + "/Type_.txt")[Loader];




		
        if(GameObject.Find(IDENT_Type) != null)
		{
            newBlock.GetComponent<MeshFilter> ().sharedMesh = GameObject.Find(IDENT_Type).GetComponent<MeshFilter>().sharedMesh;
		}
		else
		{
            GameObject newType = Instantiate(Resources.Load(IDENT_Type)as GameObject,Vector3.zero,Quaternion.identity)as GameObject;
			newBlock.GetComponent<MeshFilter> ().sharedMesh = newType.GetComponent<MeshFilter>().sharedMesh;
			Destroy(newType);
		}

        if (File.ReadAllLines(IdentDirectory + "/Texture_.txt").Length>0)
        {
            if (!string.IsNullOrEmpty(File.ReadAllLines(IdentDirectory + "/Texture_.txt")[Loader]))
            {
                
            string IDENT_Texture = File.ReadAllLines(IdentDirectory + "/Texture_.txt")[Loader];

                if (IDENT_Texture == "NoTexture")
                {
                    newBlock.GetComponent<Renderer>().material.mainTexture = null;
                }
                else
                {
                    Texture tex = Resources.Load(IDENT_Texture)as Texture;
                    newBlock.GetComponent<Renderer>().material.mainTexture = tex;
                }

            }
        }
      
		newBlock.GetComponent<MeshCollider> ().sharedMesh = newBlock.GetComponent<MeshFilter> ().sharedMesh;

		newBlock.SetActive (true);

		CounterToLoadAndSaveEachObject++;

		
	}
	
	
	
	void OnGUI()
	{
		GUIStyle style = new GUIStyle ();
		style.fontSize = 30;

		GUIStyle style2 = new GUIStyle ();
		style2.fontSize = 20;
		style2.normal.textColor = Color.blue;

		if(Saving)
		{
            GUI.Label (new Rect (Screen.width/2, Screen.height/2, 200,200), "Saving..."+File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length.ToString()+"/"+AllObjectsToSave_Load.ToArray().Length.ToString(), style);

			GUI.Label (new Rect (Screen.width/2-100, Screen.height/2+100, 200,200), "Current Saving: "+CurrentSaving, style);
		}
		if(Loading)
		{
            GUI.Label (new Rect (Screen.width/2, Screen.height/2, 200,200), "Loading... "+AllObjectsToSave_Load.ToArray().Length.ToString()+"/"+File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length.ToString(), style);

			GUI.Label (new Rect (Screen.width/2-100, Screen.height/2+100, 200,200), "Current Loading: "+CurrentSaving, style);
		}

		if(Directory.Exists(MainSave_LoadPath))
		{
            if(File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length>0)
			{
                GUI.Label (new Rect (20, Screen.height-50, 200, 200), "K- Save, L- Load [ 1 Slot To Load * "+File.ReadAllLines(MainSave_LoadPath+"/Objects.txt").Length.ToString()+" Saved Blocks * ]", style);
			}
			else
			{
				GUI.Label (new Rect (20, Screen.height-50, 200, 200), "K- Save, L- Load [No saved data yet]", style);
			}
		}

		GUI.Label (new Rect (Screen.width-450, Screen.height-70, 200, 200), "Block Creator with Save & Load System", style2);
		GUI.Label (new Rect (Screen.width-250, Screen.height-30, 200, 200), "by Matt 2016", style2);

	}

	
	
	
	
	void CountAdder_Save()
	{
		if(CounterToLoadAndSaveEachObject<AllObjectsToSave_Load.ToArray().Length-1)
		{
			CounterToLoadAndSaveEachObject++;
		}
	}
	
}