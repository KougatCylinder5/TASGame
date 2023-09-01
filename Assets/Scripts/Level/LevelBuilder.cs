using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    public static List<StartPosition> Objects = new();

    public string chosenLevel;
    public TextAsset levelInfo;

    public static bool LevelLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        
        PlayerPrefs.SetString("ChosenLevel", "Level 1");
        chosenLevel = PlayerPrefs.GetString("ChosenLevel");
        levelInfo = Resources.Load("Data/Levels/"+chosenLevel) as TextAsset;
        WorldInfo info = JsonUtility.FromJson<WorldInfo>(levelInfo.text);
        foreach (WorldObject obj in info.objects)
        {
            string toLoad = "Level Generator/" + obj.size + " " + obj.type;
            GameObject gameObject = Resources.Load(toLoad) as GameObject;
            Objects.Add(new StartPosition(new Vector2(obj.x,obj.y), Instantiate(gameObject, new Vector2(obj.x, obj.y), Quaternion.Euler(0, 0, 0))));
        }
        LevelLoaded = true;
    }
    void Update()
    {
        
    }

    public static void RestartLevel()
    {
        foreach(StartPosition obj in Objects)
        {
            obj.ResetPos();
        }
    }
}
public class WorldInfo
{
    public string name;
    public string description;
    public List<WorldObject> objects;

    public override string ToString()
    {
        string output = name + " " + description + " ";

        foreach(WorldObject Object in objects)
        {
            output += Object.x;
            output += Object.y;
            output += Object.size;
            output += Object.type;
        }


        return output;
    }
    public WorldInfo()
    {
        objects = new();
    }
}
[System.Serializable]
public class WorldObject
{
    public float x;
    public float y;
    public ObjectType type;
    public ObjectSize size;
    public WorldObject(float x, float y, ObjectType type, ObjectSize size)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.size = size;
    }
} 
public enum ObjectSize
{
    _1x1,
    _1x2,
    _1x3,
    _1x4,
    _2x1,
    _2x2,
    _2x3,
    _2x4,
    _3x1,
    _3x2,
    _3x3,
    _3x4,
    _4x1,
    _4x2,
    _4x3,
    _4x4,
    
}

public enum ObjectType
{
    Chain,
    Tile,
    Boost,
    Checkpoint,
    End,
    Robot
}


public class StartPosition
{
    public Vector2 pos;
    public GameObject obj;

    public StartPosition(Vector2 pos, GameObject obj)
    {
        this.pos = pos;
        this.obj = obj;
    }

    public void ResetPos()
    {
        obj.transform.position = pos;
        obj.transform.rotation = Quaternion.identity;
        try
        {
            obj.gameObject.GetComponent<Rigidbody>().velocity = new();
        }
        catch{}
    }
}