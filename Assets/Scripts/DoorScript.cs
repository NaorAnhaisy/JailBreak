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

    private void OnTriggerEnter(Collider other)
    {
        CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
        if (other.CompareTag("Player") && characterMovement != null && characterMovement.hasCollectedKeys)
        {
            doorAnimator.SetBool("isOpen", true);
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            doorAnimator.SetBool("isOpen", false);
            isOpen = false;
        }
    }
}