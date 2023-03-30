using Ennemis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Armes
{
    public class Projectile : MonoBehaviour
    {

        private Rigidbody _rb;
        public int degats; 
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Fire(float speed, Vector3 direction)
        {
            _rb.velocity = direction * speed;
        }

        void OnCollisionEnter(Collision collision)
        {
            // Debug-draw all contact points and normals
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
                if (collision.gameObject.GetComponent<EnnemiController>())
                {
                    collision.gameObject.GetComponent<EnnemiController>().OnHit(degats);
                    Debug.Log("degats :"+degats + "pv ennemi " +collision.gameObject.GetComponent<EnnemiController>().pv);
                }
            }

            // Play a sound if the colliding objects had a big impact.
            //if (collision.relativeVelocity.magnitude > 2)
            //    audioSource.Play();
        }

    }
}
