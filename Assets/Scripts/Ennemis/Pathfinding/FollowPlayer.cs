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
                agent.SetDestination(target.position);
            }
        }
    

    }

}