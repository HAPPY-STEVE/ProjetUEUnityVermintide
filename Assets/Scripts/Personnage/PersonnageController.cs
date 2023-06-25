using Armes;
using Cinemachine;
using Ennemis;
using Personnage.Upgrade;
using Save;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
        private bool inAttaque, peutAttaquer;
        private CinemachineVirtualCamera cinemachine;
        private float delaiMinAttaqueDistance = 0.5f;
        private float time = 0f;
        [Header("Evenements")]
        public UnityEvent onHitEvent; 
        public int Pv { get => pv; set => pv = value; }


        // Start is called before the first frame update
        void Start()
        {
            DataHolder dh  = DataHolder.GetInstance();
            //A chaque map, on recupere upgrade(s) choisis
            if (dh != null)
            {
                if (dh.armechoisi)
                {
                    arme = dh.armechoisi;
                    Debug.Log(arme);
                }
            }
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
            if(arme != null)
            {
                if(arme.controllerOverride != null)
                {
                    anim.runtimeAnimatorController = arme.controllerOverride;
                }
                pv = arme.pv;
                degats = arme.degats;
                defense = arme.defense;
                magie = arme.magie;
                vitesseAttaque = arme.vitesseAttaque;
                vitesseMouvement = arme.vitesseMouvement;
                regenPV = arme.regenPV;
                Instantiate(arme.armePrefab, gameObject.transform.Find("PlayerCameraRoot").transform.Find("WeaponHolder").transform);
            }

            if (dh != null)
            {
                if (dh.upgradesFloor.Count > 0)
                {
                    ajoutUpgrades(dh.upgradesFloor);
                }
            }

            delaiMinAttaqueDistance = (delaiMinAttaqueDistance * (1 - vitesseAttaque / 10));
                cinemachine = (CinemachineVirtualCamera)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        }

        // Update is called once per frame
        void Update()
        {
            if(pv == 0)
            {
                onDeath();
            }

            if(peutAttaquer == false)
            {

                time += Time.deltaTime;

                if (time > delaiMinAttaqueDistance)
                {
                    time = 0;
                    peutAttaquer = true; 
                }
            }

        }

        public void attaque()
        {
            time += Time.deltaTime;
            //StartCoroutine(routineAttaque());
            if (inAttaque == false && peutAttaquer == true)
            {
                inAttaque = true;
                anim.speed = 1 * vitesseAttaque;
                if (arme.armeProjectile == true)
                {
                    fireProjectile();
                }

                peutAttaquer = false;
                anim.SetTrigger("Attack");
                //yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
                anim.speed = 1;
                inAttaque = false;
            }
        }

        public IEnumerator routineAttaque()
        {
            if (inAttaque == false && peutAttaquer == true)
            {
                inAttaque = true;
                anim.speed = 1 * vitesseAttaque;
                if(arme.armeProjectile == true)
                {
                    fireProjectile();
                }

                peutAttaquer = false;
                anim.SetTrigger("Attack");
                yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
                anim.speed = 1;
                inAttaque = false;
            }

        }

        private void fireProjectile()
        {
            if (peutAttaquer == true)
            {
                var position = transform.position + Camera.main.transform.forward * 2;
                position.y = (float)(position.y + 1.2);
                
                var rotation = transform.rotation;
                rotation.x = transform.GetChild(0).rotation.x;
                var projectile = Instantiate(arme.projectilePrefab, position, rotation);
                projectile.GetComponent<Projectile>().degats = degats; 
                projectile.GetComponent<Projectile>().Fire(10 * arme.vitesseAttaque, Camera.main.transform.forward * vitesseAttaque);
            }
        }

        public void onHit(float damage)
        {
            pv -= (int)damage;
            onHitEvent?.Invoke();
            Debug.Log("pv perso:" + pv);

        }

        public void onDeath()
        {
            RunManager rm = FindObjectOfType<RunManager>();
            if(rm != null)
            {
                rm.gameOver();

            }
        }

        /// <summary>
        /// Traite la liste d'upgrades passés par DataHolder. 
        /// </summary>
        /// <param name="upgrades">Liste d'upgrades.</param>
        public void ajoutUpgrades(List<Upgrade.UpgradeStore> upgrades)
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                switch (upgrades[i].stat)
                {
                    case Stats.pv:
                        pv *= upgrades[i].valeurUpgrade; 
                        break;
                    case Stats.degats:
                        degats *= upgrades[i].valeurUpgrade;
                        break;
                    case Stats.defense:
                        defense *= upgrades[i].valeurUpgrade;
                        break;
                    case Stats.magie:
                        magie *= upgrades[i].valeurUpgrade;
                        break;
                    case Stats.vitesseAttaque:
                        vitesseAttaque *= upgrades[i].valeurUpgrade;
                        break;
                    case Stats.vitesseMouvement:
                        vitesseMouvement *= upgrades[i].valeurUpgrade;
                        break;
                    case Stats.regenPV:
                        regenPV *= upgrades[i].valeurUpgrade;
                        break;
                    default:
                        regenPV *= upgrades[i].valeurUpgrade;
                        break;
                }
            }
            
        }

    }
}

