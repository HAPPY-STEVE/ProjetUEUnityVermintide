using Armes;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Personnage
{
    public class PersonnageController : MonoBehaviour
    {
        [Header("Arme actuelle")]
        public Arme arme;
        [Header("Caracteristiques primaires")]
        [Range(0, 200)]
        [SerializeField]
        private int pv = 100;
        public int degats = 0;
        public int defense = 0;
        public int magie = 0;
        [Header("Caracteristiques secondaires")]
        public float vitesseAttaque = 0;
        public float vitesseMouvement = 0;
        public float regenPV = 0;
        private Animator anim;
        private bool inAttaque;
        private CinemachineVirtualCamera cinemachine;
        private float delaiMinAttaqueDistance;


        // Start is called before the first frame update
        void Start()
        {
            if(anim == null)
            {
                anim = GetComponent<Animator>();
            }
            if(arme != null)
            {
                pv = arme.pv;
                degats = arme.degats;
                defense = arme.defense;
                magie = arme.magie;
                vitesseAttaque = arme.vitesseAttaque;
                vitesseMouvement = arme.vitesseMouvement;
                regenPV = arme.regenPV;
            }
            cinemachine = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        }

        // Update is called once per frame
        void Update()
        {
            if(pv == 0)
            {
                onDeath();
            }
        }

        public void attaque()
        {
            StartCoroutine(routineAttaque());
        }

        public IEnumerator routineAttaque()
        {
            if (inAttaque == false)
            {
                inAttaque = true;
                anim.speed = 1 * vitesseAttaque;
                if(arme.armeProjectile == true)
                {
                    fireProjectile();
                    yield return new WaitForSeconds(delaiMinAttaqueDistance);
                }
                anim.SetTrigger("Attack");
                yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
                anim.speed = 1;
                inAttaque = false;
            }

        }

        private void fireProjectile()
        {
            var position = transform.position + transform.forward;
            position.y = (float)(position.y + 1.2);
            var rotation = transform.rotation;
            rotation.x = transform.GetChild(0).rotation.x;
            var projectile = Instantiate(arme.projectilePrefab, position, rotation);
            projectile.GetComponent<Projectile>().degats = degats;
            projectile.GetComponent<Projectile>().Fire(10*arme.vitesseAttaque, transform.forward);
        }

        public void onHit(float damage)
        {
            pv -= (int)damage; 
        }

        public void onDeath()
        {

        }

    }
}

