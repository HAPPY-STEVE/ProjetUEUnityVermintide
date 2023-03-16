using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemis
{
    /// <summary>
    /// Script spawner, gère deux types de vagues : Burst (grosse vague), continu (vague petite plus regulière)
    /// Fait spawn les ennemis sur sa position.
    /// Il n'y a pas actuellement plusieurs types d'ennemis gérés.
    /// </summary>
    public class Spawner : MonoBehaviour
    {

        private ObjectPool objPool;
        [Header("Nb Spawn par vague")]
        public int tailleBurst = 50; 
        public int tailleContinu = 3; 

        [Header("Timer Spawn (en s)")]
        public float timerBurst = 360f; 
        public float timerContinu = 120f; 
        private float timeContinu = 0f; 
        private float timeBurst = 0f; 
        void Start()
        {
            objPool = ObjectPool.sharedInstance;
        }

        // Update is called once per frame
        void Update()
        {
            timeBurst += Time.deltaTime;
            timeContinu += Time.deltaTime;

            if(timeContinu >= timerContinu)
            {
                timeContinu = 0f;
                spawnContinu();

            } else if (timeBurst >= timerBurst)
            {
                timeBurst = 0f;
                spawnBurst();
            }
        }
        public void spawnBurst()
        {
            for(int x = 0; x < tailleBurst; x++)
            {
                GameObject e = objPool.GetPooledObject("Ennemi");
                e.GetComponent<FollowPlayer>().target = FindObjectOfType<CharacterController>().transform;
                e.transform.position = gameObject.transform.position;
                e.SetActive(true);
            }
        }
        public void spawnContinu()
        {
            for (int x = 0; x < tailleContinu; x++)
            {
                GameObject e = objPool.GetPooledObject("Ennemi");
                e.GetComponent<FollowPlayer>().target = FindObjectOfType<CharacterController>().transform;
                e.transform.position = gameObject.transform.position;
                e.SetActive(true);
            }
        }
    }
}
