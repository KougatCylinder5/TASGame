using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class LevelFileBuilder : MonoBehaviour
{
    public new string name;
    public string description;

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("LevelOverlay", LoadSceneMode.Additive);

        WorldInfo info = new();
        info.name = name;
        info.description = description;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("WorldTiles"))
        {
            
            string[] value = Regex.Match(obj.name, ".*\\ |\\ .*").Value.Split();
            string type = value[1];
            string size = value[0];
            Enum.TryParse(type, true, out ObjectType Type);
            Enum.TryParse(size, true, out ObjectSize Size);
            
            info.objects.Add(new WorldObject(obj.transform.position.x, obj.transform.position.y, Type, Size));
        }


        string output = JsonUtility.ToJson(info);
        Debug.Log(output);
    }
    void Update()
    {

    }
}