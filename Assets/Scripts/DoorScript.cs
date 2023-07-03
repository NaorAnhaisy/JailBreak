using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator doorAnimator;
    public bool isOpen;

    void Start()
    {
        doorAnimator.SetBool("isOpen", isOpen);
    }

    public void ChangeDoorState()
    {
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);

        if (isOpen)
        {
            doorAnimator.Play("DoorOpen");
        }
        else
        {
            doorAnimator.Play("DoorClose");
        }
    }
}