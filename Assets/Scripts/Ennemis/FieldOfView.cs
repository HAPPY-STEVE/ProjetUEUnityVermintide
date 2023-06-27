using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        playerRef = FindObjectOfType<Personnage.PersonnageController>().gameObject;
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        if (playerRef != null)
        {
            Transform target = playerRef.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                RaycastHit hit; 
                if (!Physics.Raycast(origin: transform.position, direction: directionToTarget, hitInfo: out hit, maxDistance: distanceToTarget, layerMask: obstructionMask))
                { 
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;

                }
            }
            else
                canSeePlayer = false;
        }
    }
}
