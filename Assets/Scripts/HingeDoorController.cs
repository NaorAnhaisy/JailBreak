using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HingeDoorController : MonoBehaviour
{
    public float openAngle = -170f; // How far the door should open in degrees
    public float openSpeed = 3f;  // How fast the door opens
    public float bounceIntensity = 0.2f; // How much the door should bounce when opening/closing

    private bool isOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        initialRotation = transform.localRotation;
        targetRotation = initialRotation * Quaternion.Euler(0, openAngle, 0);
        initialPosition = transform.localPosition;
        targetPosition = initialPosition + Vector3.up * bounceIntensity; // Bounce up initially
    }

    private void Update()
    {
        if (isOpen)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * openSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation, Time.deltaTime * openSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * openSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
            targetPosition = initialPosition - Vector3.up * bounceIntensity; // Bounce down when opening
        }
    }
}
