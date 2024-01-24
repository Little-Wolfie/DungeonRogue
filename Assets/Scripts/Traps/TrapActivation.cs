using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivation : MonoBehaviour
{
    public Animator animator;
    bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            isActive = true;

            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                animator.SetTrigger("activate");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive)
        {
            isActive = true;

            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                animator.SetTrigger("activate");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isActive)
        {
            isActive = false;

            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                animator.SetTrigger("deactivate");
            }
        }
    }
}
