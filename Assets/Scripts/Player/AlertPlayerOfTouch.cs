using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertPlayerOfTouch : MonoBehaviour
{
    List<WaitForType> waitForTypes= new(){WaitForType.Boost,WaitForType.Checkpoint};

    public void OnTriggerEnter(Collider other)
    {
        WaitForType obj = WaitForType.None;
        foreach(WaitForType type in waitForTypes)
        {
            if (other.transform.parent.name.Contains(Enum.GetName(typeof(WaitForType),type)))
            {
                obj = type;
                RobotController.waitingForObject = other.transform.parent.gameObject;
                break;
            }
        }
        if (obj.Equals(WaitForType.Boost))
        {
            Rigidbody rb = GetComponentInParent<Rigidbody>();
            rb.AddForce(rb.velocity.normalized * 5, ForceMode.VelocityChange);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.transform.parent.Equals(RobotController.waitingForObject)) 
        {
            RobotController.waitingForObject = null;
        }
            
    }
}
