using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Transform objectToRotate;
    public float rotationSpeed;
    public Vector3 rotationAxis;
    bool isRotating;

    void Update()
    {
        if (isRotating)
        {
            objectToRotate.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isRotating)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                isRotating = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isRotating)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                isRotating = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isRotating)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                isRotating = false;
            }
        }
    }
}
