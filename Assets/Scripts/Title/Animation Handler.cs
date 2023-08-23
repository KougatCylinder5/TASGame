using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowLevels()
    {
        animator.SetTrigger("Show Levels");
    }
    
    public void ShowMenu()
    {
        animator.SetTrigger("Show Menu");
    }
}
