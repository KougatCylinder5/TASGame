using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public Queue<GameObject> Objects;

    public string chosenLevel;
    public TextAsset levelInfo;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("ChosenLevel", "Level 1");

        chosenLevel = "Data/Levels/"+PlayerPrefs.GetString("ChosenLevel");

        levelInfo = Resources.Load(chosenLevel) as TextAsset;
        WorldInfo info = JsonUtility.FromJson<WorldInfo>(levelInfo.text);
        Debug.Log(info.ToString());
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
}
[System.Serializable]
public class WorldObject
{
    public double x;
    public double y;
    public ObjectType type;
    public ObjectSize size;
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
    End
}
