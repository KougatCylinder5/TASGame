using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject visibleCover;
    private Image image;
    private TextMeshProUGUI loadingText;
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
        image = visibleCover.GetComponent<Image>();
        loadingText = visibleCover.GetComponentInChildren<TextMeshProUGUI>();

        while (true)
        {
            if (LevelBuilder.LevelLoaded)
            {
                image.color = new Color(43, 43, 43, image.color.a - 1f);
                loadingText.color = new Color(255, 255, 255, loadingText.color.a - 1f);
            }
            if(image.color.a < 0 && op.isDone)
            {
                SceneManager.MergeScenes( SceneManager.GetActiveScene(),SceneManager.GetSceneByName("Level"));
                Destroy(visibleCover.gameObject);
                break;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
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
