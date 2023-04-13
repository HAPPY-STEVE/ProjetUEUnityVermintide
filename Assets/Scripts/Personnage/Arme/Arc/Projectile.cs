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
        private Collision lastCollider; 
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Fire(float speed, Vector3 direction)
        {
            _rb.velocity = direction * speed;
            StartCoroutine(despawn());
        }

        void OnCollisionEnter(Collision collision)
        {
            // Debug-draw all contact points and normals
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
                if (collision.gameObject.GetComponent<EnnemiController>() && collision != lastCollider)
                {
                    lastCollider = collision;
                    collision.gameObject.GetComponent<EnnemiController>().OnHit(degats);
                    Debug.Log("degats :"+degats + "pv ennemi " +collision.gameObject.GetComponent<EnnemiController>().pv);

                    Destroy(gameObject);
                }
            }

            // Play a sound if the colliding objects had a big impact.
            //if (collision.relativeVelocity.magnitude > 2)
            //    audioSource.Play();
        }
        IEnumerator despawn()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

    }
}
