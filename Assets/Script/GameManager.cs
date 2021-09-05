using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        LoadBuilding();
    }

    public void LoadBuilding()
    {
        for (int i = 0; i < GameData.ObjectDataList.Count; i++)
        {
            if (GameData.ObjectDataList[i].TypeComponent == "Floor")
            {
                GameObject floor = Instantiate(BuildManager.instance.Floor, BuildManager.instance.FloorParent.transform);
                floor.transform.position = GameData.ObjectDataList[i].Position;
                floor.transform.GetChild(0).Rotate(GameData.ObjectDataList[i].Rotation);
                floor.GetComponent<Props>().ColorId = GameData.ObjectDataList[i].ColorId;
                floor.GetComponent<Props>().SetColor();
            }
            else if (GameData.ObjectDataList[i].TypeComponent == "Wall")
            {
                GameObject wall = Instantiate(BuildManager.instance.Wall, BuildManager.instance.WallParent.transform);
                wall.transform.position = GameData.ObjectDataList[i].Position;
                wall.transform.Rotate(GameData.ObjectDataList[i].Rotation);
                wall.GetComponent<Props>().ColorId = GameData.ObjectDataList[i].ColorId;
                wall.GetComponent<Props>().SetColor();
            }
            else if (GameData.ObjectDataList[i].TypeComponent == "Door")
            {
                GameObject door = Instantiate(BuildManager.instance.Door, BuildManager.instance.DoorParent.transform);
                door.transform.position = GameData.ObjectDataList[i].Position;
                door.transform.Rotate(GameData.ObjectDataList[i].Rotation);
                door.GetComponent<Props>().ColorId = GameData.ObjectDataList[i].ColorId;
                door.GetComponent<Props>().SetColor();
            }
            else if (GameData.ObjectDataList[i].TypeComponent == "Furniture")
            {
                GameObject obj = null;
                for (int j = 0; j < BuildManager.instance.Furniture.Length; j++)
                {
                    if (BuildManager.instance.Furniture[j].GetComponent<Props>().name == GameData.ObjectDataList[i].NameComponent)
                    {

                        obj = BuildManager.instance.Furniture[j];
                    }
                }

                if (obj != null)
                {
                    GameObject furniture = Instantiate(obj, BuildManager.instance.FurnitureParent.transform);
                    furniture.transform.position = GameData.ObjectDataList[i].Position;
                    furniture.transform.Rotate(GameData.ObjectDataList[i].Rotation);
                    furniture.GetComponent<Props>().ColorId = GameData.ObjectDataList[i].ColorId;
                    furniture.GetComponent<Props>().SetColor();
                }

            }
        }
    }

    public void SaveObject(Vector3 pos, string name, string type, Vector3 rot, string colorId)
    {
        Data data = new Data();
        data.Position = pos;
        data.NameComponent = name;
        data.TypeComponent = type;
        data.Rotation = rot;
        data.ColorId = colorId;

        GameData.ObjectDataList.Add(data);
        writeFile();
    }

    public void DeleteObject(Vector3 pos, string type)
    {
        Data data = new Data();
        for (int i = 0; i < GameData.ObjectDataList.Count; i++)
        {
            if (GameData.ObjectDataList[i].TypeComponent == type && GameData.ObjectDataList[i].Position == pos)
            {
                data = GameData.ObjectDataList[i];
            }
        }

        if (data.NameComponent != "")
        {
            GameData.ObjectDataList.Remove(data);
            writeFile();
        }
    }

    public void UpdateObjet(Vector3 pos, string name, string type, Vector3 rot, string colorId)
    {
        Data data = new Data();
        for (int i = 0; i < GameData.ObjectDataList.Count; i++)
        {
            if (GameData.ObjectDataList[i].TypeComponent == type && GameData.ObjectDataList[i].Position == pos)
            {
                data = GameData.ObjectDataList[i];
            }
        }

        if (data.NameComponent != "")
        {
            data.Rotation = rot;
            data.ColorId = colorId;
            writeFile();
        }
    }


    [Serializable]
    public class Data
    {
        public Vector3 Position;
        public string NameComponent;
        public string TypeComponent;
        public Vector3 Rotation;
        public string ColorId;
    }

    [Serializable]
    public class SaveData
    {
        public List<Data> ObjectDataList = new List<Data>();
    }

    // Create a field for the save file.
    string saveFile;

    // Create a GameData field.
    public SaveData GameData = new SaveData();

    void Awake()
    {
        // Update the path once the persistent path exists.
        saveFile = Application.persistentDataPath + "/gamedata.json";
        readFile();
    }

    public void readFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            GameData = JsonUtility.FromJson<SaveData>(fileContents);
        }
        else
        {
            writeFile();
        }
    }

    public void writeFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(GameData);

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
    }

}
