using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Ennemis
{

    /// <summary>
    /// Script pour suivre le joueur.
    ///Necessite un component NavMeshAgent
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class FollowPlayer : MonoBehaviour
    {
        [Header("Variables")]
        private NavMeshAgent agent;
        public Transform target;
        public float maxDistance = 1f; 
        public bool interrupt; 
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

                if(target == null)
                {
                    target = FindObjectOfType<CharacterController>().transform;
                }
            agent.SetDestination(target.position);
        }

        // Update is called once per frame
        void Update()
        {
            if(interrupt == false)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            } else
            {
                agent.isStopped=true;
            }

            if (!agent.isStopped)
            {
                if (Vector3.Distance(agent.destination, transform.position) <= maxDistance)
                {
                    agent.isStopped = true;
                }
            }


            // Determine which direction to rotate towards
            Vector3 targetDirection = target.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = agent.speed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    

    }

}