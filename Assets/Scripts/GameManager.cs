using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
namespace UnityEngine {
    static class Keyboard
    {
        public static KeyCode GetCurrentKeyDown()
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
        public static KeyCode GetCurrentKeyUp()
        {
            foreach (int key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyUp((KeyCode)Enum.ToObject(typeof(KeyCode), key)))
                {
                    return (KeyCode)Enum.ToObject(typeof(KeyCode), key);
                }
            }
            return KeyCode.None;
        }
        public static KeyCode GetKey()
        {
            foreach (int key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey((KeyCode)Enum.ToObject(typeof(KeyCode), key)))
                {
                    return (KeyCode)Enum.ToObject(typeof(KeyCode), key);
                }
            }
            return KeyCode.None;
        }
    }
}
