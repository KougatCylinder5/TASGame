using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image visibleCover;
    AsyncOperation op;
    private void Start()
    {
        //SceneManager.MergeScenes(SceneManager.)
        op = SceneManager.LoadSceneAsync("Level", LoadSceneMode.Additive);
        StartCoroutine(nameof(removeCover));
    }


    // Update is called once per frame
    void FixedUpdate()
    {


    }

    private IEnumerator removeCover()
    {
        while (true)
        {
            if (LevelBuilder.LevelLoaded)
            {
                visibleCover.color = new Color(255, 255, 255, visibleCover.color.a-1);
            }
            if(visibleCover.color.a < 0 && op.isDone)
            {
                SceneManager.MergeScenes( SceneManager.GetActiveScene(),SceneManager.GetSceneByName("Level"));
                Destroy(visibleCover.gameObject);
                break;
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
        yield return null;
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
