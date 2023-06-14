using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemis
{

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