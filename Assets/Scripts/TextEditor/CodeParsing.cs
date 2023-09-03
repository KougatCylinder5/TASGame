using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CodeParsing : MonoBehaviour
{
    public static CodeMemory textLines;
    private static Queue<Task> tasks = new();

    public static Queue<Task> Tasks { get { return tasks; } }

    // Start is called before the first frame update
    void Start()
    {
        textLines = GameObject.Find("TextHolders").GetComponent<CodeMemory>();
    }

    // Update is called once per frame

    public static void GenerateCommands()
    {

        tasks.Clear();

        foreach (List<char> partial in textLines.rawLines)
        {
            string rawLine = new(partial.ToArray());
            rawLine = rawLine.Trim();
            List<string> splitLine = rawLine.Split(' ').ToList();
            for(int i = 0; i < splitLine.Count; i++)
            {
                if (splitLine[i].Equals(String.Empty) || splitLine[i].Equals(" "))
                {
                    splitLine.RemoveAt(i);
                }
            }
            if (splitLine.Count == 0 || splitLine[0].Equals(string.Empty))
            {
                continue;
            }
            if (!Enum.TryParse<Commands>(splitLine[0], true, out Commands result))
            {
                throw new NotSupportedException();
            }

            Task task = new Task();

            switch (result)
            {
                case Commands.Stop:
                    if (splitLine.Count == 1)
                    {

                        task.command = result;
                        task.direction = Direction.None;
                        task.waitType = WaitForType.None;
                        task.value = double.NaN;
                        break;
                    }
                    throw new NotSupportedException();

                case Commands.Wait:
                case Commands.WaitUntil:

                    if (result == Commands.Wait)
                    {
                        if (!double.TryParse(splitLine[1], out double value) || value < 0.02f)
                        {
                            throw new NotSupportedException();
                        }
                        task.value = value;
                    }
                    else
                    {
                        if (!Enum.TryParse(splitLine[1], true, out WaitForType type) || double.TryParse(splitLine[1], out double _))
                        {
                            throw new NotSupportedException();
                        }
                        task.waitType = type;
                    }
                    task.command = result;
                    task.direction = Direction.None;
                    break;
                case Commands.Hook:
                    if (Enum.TryParse(splitLine[1], true, out Direction direction))
                    {
                        task.command = result;
                        task.direction = direction;
                        task.waitType = WaitForType.None;
                        task.value = double.NaN;
                        try{
                            if (Enum.TryParse(splitLine[2], true, out Direction direction2))
                            {
                                task.direction2 = direction2;
                            }
                        }catch{}
                        break;
                    }
                    throw new NotSupportedException();
                case Commands.Jump:
                    task.command = result;
                    task.direction = Direction.None;
                    task.waitType = WaitForType.None;
                    task.value = double.NaN;
                    break;
                default:
                    if (Enum.TryParse(splitLine[1], true, out Direction direction3))
                    {
                        if(direction3 == Direction.Up || direction3 == Direction.Down)
                        {
                            throw new NotSupportedException();
                        }
                        task.command = result;
                        task.direction = direction3;
                        task.waitType = WaitForType.None;
                        task.value = double.NaN;
                        try{
                            if (splitLine[2] != "" && splitLine[2] != " ")
                            {
                                throw new NotSupportedException();
                            }
                        }catch(ArgumentOutOfRangeException ex){}
                        break;
                    }
                    throw new NotSupportedException();

            }
            tasks.Enqueue(task);
        }
        foreach(var task in tasks)
        {
            Debug.Log(task);
        }
    }
}
public enum Commands
{
    None,
    Jump,
    Walk,
    Run,
    Stop,
    Slide,
    Hook,
    Wait,
    WaitUntil
}
public enum Direction
{
    None,
    Left,
    Up,
    Right,
    Down
}
public enum WaitForType
{
    None,
    Checkpoint,
    Boost,
    Time
}
[Serializable]
public class Task
{

    public Commands command;
    public Direction direction;
    public Direction direction2;
    public WaitForType waitType;
    public double value = double.NaN;

    public override string ToString()
    {
        return Enum.GetName(typeof(Commands), command) + Enum.GetName(typeof(Direction), direction) + Enum.GetName(typeof(Direction), direction2) + Enum.GetName(typeof(WaitForType), waitType) + value.ToString();
    }
}
