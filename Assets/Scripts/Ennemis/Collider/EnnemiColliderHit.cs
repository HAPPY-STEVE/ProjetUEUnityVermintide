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

        // Start is called before the first frame update
        void Start()
        {
            ennemi = GetComponentInParent<EnnemiController>();
        }

        // Count how many colliders are overlapping this trigger.
        // If desired, you can filter here by tag, attached components, etc.
        // so that only certain collisions count. Physics layers help too.
        void OnTriggerEnter(Collider other)
        {
            if(lastCollider != other)
            {
                ennemi.OnHitPersonnage();

            }
        }
    }

}
