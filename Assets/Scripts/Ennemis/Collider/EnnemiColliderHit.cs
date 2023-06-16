using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemis
{
    /// <summary>
    /// Est utilise pour detecter les hits avec le personnage, avec le collider attache. 
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class EnnemiColliderHit : MonoBehaviour
    {
        private EnnemiController ennemi; 
        private Collider lastCollider;
        private bool hit=false; 
        // Start is called before the first frame update
        void Start()
        {
            ennemi = GetComponentInParent<EnnemiController>();
        }

        private void Update()
        {
            if(ennemi.peutAttaquer == true)
            {
                hit = false; 
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(lastCollider != other & hit == false)
            {
                hit = true; 
                ennemi.OnHitPersonnage();

            }
        }
    }

}
