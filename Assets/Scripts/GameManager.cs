using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
namespace UnityEngine {
    static class Keyboard
    {
        public static KeyCode CurrentKeyDown()
        {
            foreach (int key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown((KeyCode)Enum.ToObject(typeof(KeyCode), key)))
                {
                    return (KeyCode)Enum.ToObject(typeof(KeyCode), key);
                }
            }
            return KeyCode.None;
        }
    }
}
