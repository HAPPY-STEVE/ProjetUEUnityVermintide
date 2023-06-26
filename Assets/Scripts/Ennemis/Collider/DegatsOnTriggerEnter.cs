using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemis
{
    /// <summary>
    /// Effectue des d�g�ts � un ennemi lors d'une collision avec le trigger associe au gameObject auquel ce script est attach�.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DegatsOnTriggerEnter : MonoBehaviour
    {
        [Header("Degats")]
        public float degats = 5f;
        private List<Collider> touched = new List<Collider>(); 

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<EnnemiController>() == true && touched.Contains(other)==false)
            {
                touched.Add(other);
                other.GetComponent<EnnemiController>().OnHit(degats);
            }
        }
    }

}