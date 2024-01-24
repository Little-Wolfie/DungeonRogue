using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectBetweenPoints : MonoBehaviour
{
    public Transform objectToMove;
    public float moveSpeed;
    public List<Transform> points;
    public bool reverseAtEnd = true;
    public bool isMoving = false;
    public int nextPoint = 0;
    public bool isInReverse = false;

    void Update()
    {
        if (points.Count >= 2 && isMoving)
        {
            
            MoveObjectBetweenLocations(objectToMove, points[nextPoint].position, moveSpeed);
        }
    }

    void MoveObjectBetweenLocations(Transform objectToMove, Vector3 endPosition, float speed)
    {
        float step = speed * Time.deltaTime;
        objectToMove.position = Vector3.MoveTowards(objectToMove.position, endPosition, step);
        if (objectToMove.transform.position == endPosition)
        {
            if (!isInReverse)
            {
                nextPoint++;
            }
            else
            {
                nextPoint--;
            }
            
            if(nextPoint >= points.Count)
            {
                if (reverseAtEnd)
                {
                    isInReverse = !isInReverse;

                    nextPoint = points.Count - 2;
                }
                else
                {
                    nextPoint = 0;
                }
            }
            else if (nextPoint < 0)
            {
                if (reverseAtEnd)
                {
                    isInReverse = !isInReverse;

                    nextPoint = 1;
                }
                else
                {
                    nextPoint = points.Count - 1;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMoving)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                isMoving = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isMoving)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                isMoving = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isMoving)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                isMoving = false;
            }
        }
    }
}
