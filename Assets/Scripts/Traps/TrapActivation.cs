using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivation : MonoBehaviour
{
    public Animator animator;
    public List<GameObject> objectsToEnable;
    bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            isActive = true;

            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                animator.SetTrigger("activate");
                foreach (GameObject go in objectsToEnable)
                {
                    go.SetActive(true);
                }
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
                foreach (GameObject go in objectsToEnable)
                {
                    go.SetActive(false);
                }
            }
        }
    }
}
