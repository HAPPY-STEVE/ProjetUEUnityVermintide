using Ennemis;
using Personnage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Armes
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {

        private Rigidbody _rb;
        [Header("Variables")]
        public int degats;
        public int vitesseProjectile=1;
        public float despawnTime=0.1f;
        private float startTime;
        private float journeyLength;
        private Vector3 startPos;
        private Vector3 endPos;

        private Collider lastCollider;
        [Header("Ennemis")]
        public bool projectileEnnemi = false;
        [Header("Event")]
        public UnityEvent onCollision;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            // Keep a note of the time the movement started.
            PersonnageController pc = FindObjectOfType<PersonnageController>();
            startTime = Time.time;
            startPos = gameObject.transform.position;
            endPos = pc.transform.position;
            // Calculate the journey length.
            journeyLength = Vector3.Distance(startPos, pc.transform.position);
        }
        public void Fire(float speed, Vector3 direction)
        {
            _rb.velocity = direction * speed;
            StartCoroutine(despawn());
        }


        void Update()
        {
            if(projectileEnnemi == true)
            {

                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * vitesseProjectile;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / journeyLength;

                // Set our position as a fraction of the distance between the markers.
                transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
            }
        }

        void OnTriggerEnter(Collider collision)
        {
            if (projectileEnnemi == true)
            {
                if (collision.transform.parent?.gameObject.GetComponent<PersonnageController>()!=null && collision != lastCollider)
                {
                    
                    Debug.Log(collision.transform.parent.gameObject.GetComponent<PersonnageController>());
                    lastCollider = collision;
                    collision.transform.parent.gameObject.GetComponent<PersonnageController>().onHit(degats);
                    Debug.Log("degats :" + degats + "pv personnage " + collision.transform.parent.gameObject.GetComponent<PersonnageController>().Pv);
                    onCollision?.Invoke();
                    StartCoroutine(despawn());
                }
                else if (collision.gameObject.GetComponent<EnnemiController>())
                {
                }
                else if (collision.gameObject.layer == 8)
                {
                    //Le projectile est dans le mur, delete 
                    Destroy(gameObject);
                }
                else
                {
                    StartCoroutine(despawn());
                }
            }
            else if (collision.gameObject.GetComponent<EnnemiController>() && collision != lastCollider)
            {
                lastCollider = collision;
                collision.gameObject.GetComponent<EnnemiController>().OnHit(degats);
                Debug.Log("degats :" + degats + "pv ennemi " + collision.gameObject.GetComponent<EnnemiController>().pv);
                onCollision?.Invoke();
                StartCoroutine(despawn());
            }
            else
            {
                StartCoroutine(despawn());
            }

        }

        IEnumerator despawn()
        {
            yield return new WaitForSeconds(despawnTime);
            Destroy(gameObject);
        }

    }
}
