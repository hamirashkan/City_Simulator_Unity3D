using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class Player : MonoBehaviour
{



    private BinarSave BS;

    [System.Serializable]
    public class BinarSave
    {
        public List<string> name = new List<string>();
        public List<string> tag = new List<string>();
        public List<SerializableVector3> position = new List<SerializableVector3>();
        public List<SerializableQuaternion> rotation = new List<SerializableQuaternion>();
        public List<SerializableVector3> scale = new List<SerializableVector3>();
    }

    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }

        public static implicit operator Vector3(SerializableVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        public static implicit operator SerializableVector3(Vector3 rValue)
        {
            return new SerializableVector3(rValue.x, rValue.y, rValue.z);
        }
    }

    [Serializable]
    public struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
        }

        public static implicit operator Quaternion(SerializableQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        public static implicit operator SerializableQuaternion(Quaternion rValue)
        {
            return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }







    public void Fun_SaveScene(string fileName = "\\Player.txt")
    {
       
        BinaryFormatter BF = new BinaryFormatter();
        FileStream Fs = File.Create(Application.dataPath + "\\Player.txt");
        Transform[] buildingSet = GameObject.Find("URBAN").GetComponentsInChildren<Transform>();

        Transform[] Clones = GameObject.Find("Predefiend").GetComponentsInChildren<Transform>();

        for (int m = 0; m < Clones.Length; m++)
        {
            BS.name.Add(Clones[m].name);
            BS.tag.Add(Clones[m].tag);
            BS.position.Add(Clones[m].position);
            BS.rotation.Add(Clones[m].rotation);
            BS.scale.Add(Clones[m].localScale);
        }

        //Transform[] sun = GameObject.Find("Sun_parent").GetComponentsInChildren<Transform>();
        for (int i = 0; i < buildingSet.Length ; i++)
        {
            BS.name.Add(buildingSet[i].name);
            BS.tag.Add(buildingSet[i].tag);
            BS.position.Add(buildingSet[i].position);
            BS.rotation.Add(buildingSet[i].rotation);
            BS.scale.Add(buildingSet[i].localScale);

        }

        BF.Serialize(Fs, BS);
        Fs.Close();
    }

    public void Fun_LoadScene(string fileName = "\\Player.txt")
    {
       
        BinaryFormatter Bf = new BinaryFormatter();
        if (File.Exists(Application.dataPath + "\\Player.txt"))
        {
            FileStream FS = File.Open(Application.dataPath + "\\Player.txt", FileMode.Open);
            BinarSave bs = (BinarSave)Bf.Deserialize(FS);

            for (int i = 0; i < bs.name.Count; i++)
            {
                GameObject obj = GameObject.Find(bs.name[i]);
                obj.transform.tag = bs.tag[i];
                obj.transform.position = bs.position[i];
                obj.transform.rotation = bs.rotation[i];
                obj.transform.localScale = bs.scale[i];
            }
            FS.Close();
        }
        else
        {
            Debug.Log("File is not found");
        }
    }

    void Start()
    {
        BS = new BinarSave();
    }



    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.F5))
        {
            Fun_SaveScene("Player.txt");

        }

        if (Input.GetKeyUp(KeyCode.F7))
        {
            Fun_LoadScene("Player.txt");
        }
   
        

    }


}




