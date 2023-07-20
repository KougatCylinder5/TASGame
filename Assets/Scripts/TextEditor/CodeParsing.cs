using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CodeParsing : MonoBehaviour
{
    public CodeMemory textLines;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Task> RunCommands()
    {
        List<Task> tasks = new List<Task>();

        return tasks;
        //textLines
    }
}
public enum Commands
{

}
public enum Direction
{
    LeftDown,
    Left,
    LeftUp,
    Up,
    RightUp,
    Right,
    RightDown,
    Down,

}
public class Task
{
    
    public Commands command;
    public Direction direction;
}
