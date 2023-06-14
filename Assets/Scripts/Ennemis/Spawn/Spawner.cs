using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemis
{
    /// <summary>
    /// Script spawner, gère deux types de vagues : Burst (grosse vague), continu (vague petite plus regulière)
    /// Fait spawn les ennemis sur sa position.
    /// </summary>
    public class Spawner : MonoBehaviour
    {

        private ObjectPool objPool;
        [Header("Nb Spawn par vague")]
        public int tailleContinu = 3; 

        [Header("Timer Spawn (en s)")]
        public float timerBurst = 360f; 
        public float timerContinu = 120f; 
        private float timeContinu = 0f; 
        private float timeBurst = 0f; 

        [Header("Vagues (Loop)")]
        public List<Vague> vagues = new List<Vague>();
        private List<Vague> vaguesOriginal; 
        void Start()
        {
            vaguesOriginal = vagues; 
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
            Vague vagueActuel = vagues[0];
            for (int i = 0; i < vagueActuel.contenuVagues.Count; i++)
            {

                for (int x = 0; x < vagueActuel.contenuVagues[i].tailleVague; x++)
                {
                    GameObject e = objPool.GetPooledObject(vagueActuel.contenuVagues[i].typeEnnemi.ToString());
                    e.GetComponent<FollowPlayer>().target = FindObjectOfType<CharacterController>().transform;
                    e.transform.position = gameObject.transform.position;
                    e.SetActive(true);
                }
            }
        }
        public void spawnContinu()
        {
            for (int x = 0; x < tailleContinu; x++)
            {
                GameObject e = objPool.GetPooledObject("Melee");
                e.GetComponent<FollowPlayer>().target = FindObjectOfType<CharacterController>().transform;
                e.transform.position = gameObject.transform.position;
                e.SetActive(true);
            }
        }
    }

    public enum typeEnnemi
    {
        Melee,
        Ranged,
        HeavyMelee
    }
    [System.Serializable]
    public class Vague
    {
        public List<ContenuVague> contenuVagues = new List<ContenuVague>();
    }
    [System.Serializable]
    public class ContenuVague
    {

        public int tailleVague = 50;
        public typeEnnemi typeEnnemi;
    }
}
