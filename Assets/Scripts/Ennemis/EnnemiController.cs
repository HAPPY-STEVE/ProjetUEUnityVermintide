using Personnage;
using Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Ennemis
{

    /// <summary>
    /// A fixer sur un ennemi pour gerer PV, attaque etc...
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class EnnemiController : MonoBehaviour
    {
        public EnnemiSO referenceSO;
        private PersonnageController personnage;
        private string nom;
        private string description;
        [Header("Attaque")]
        //Determine si l'ennemi est melee ou ranged, et sa distance minimale pour attaquer
        [SerializeField, Range(0, 25f)]
        private float porteeAttaque;
        public float pv;
        public float delaiMinAttaqueDistance = 3f;
        private float time = 0f;
        public bool peutAttaquer = false; 
        [SerializeField, Range(0, 100f)]
        private float degatAttaque;
        [Header("Animations")]
        private Animation marcheAnimation;
        private Animation attaqueAnimation;
        private Animation mortAnimation;
        private Animator animator;
        [Header("Collider")]
        public Collider colliderHit;
        [Header("Prefab")]
        private GameObject ennemiPrefab;
        [Header("Events")]
        public UnityEvent onMort;
        public UnityEvent onHit;
        private NavMeshAgent nva; 
        private FieldOfView fov;
        private bool mourant = false; 

        void Start()
        {
            Init();
            personnage = FindObjectOfType<PersonnageController>();
            animator = GetComponent<Animator>();
            nva = GetComponent<NavMeshAgent>();
            fov = GetComponent<FieldOfView>();
            if (GetComponent<FollowPlayer>() != null)
            {
                GetComponent<FollowPlayer>().maxDistance = porteeAttaque;

            }
            if(colliderHit != null)
            {
                colliderHit.enabled = false;

            }
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetFloat("Velocity", nva.speed);

            //Check s'il est possible d'attaquer 
            checkDistancePersonnage();



            if (peutAttaquer == false)
            {

                time += Time.deltaTime;

                if (time > delaiMinAttaqueDistance)
                {
                    time = 0;
                    peutAttaquer = true;
                }
            }


            if (pv <= 0)
            {
                onMort?.Invoke();
            }
        }

        public void Init()
        {
            mourant = false; 
            nom = referenceSO.nom;
            description = referenceSO.description;
            pv = referenceSO.pv;
            porteeAttaque = referenceSO.porteeAttaque;
            porteeAttaque = referenceSO.porteeAttaque;
            degatAttaque = referenceSO.degatAttaque;
            marcheAnimation = referenceSO.marcheAnimation;
            attaqueAnimation = referenceSO.attaqueAnimation;
            mortAnimation = referenceSO.mortAnimation;
            ennemiPrefab = referenceSO.ennemiPrefab;
            onMort.AddListener(() => OnMort());
        }
        public void checkDistancePersonnage()
        {

            float dist = Vector3.Distance(personnage.transform.position, transform.position);
            if (dist <= porteeAttaque & peutAttaquer==true)
            {
                if(fov != null)
                {
                    if(fov.canSeePlayer == true)
                    {
                        peutAttaquer = false;
                        nva.speed = Mathf.Lerp(nva.speed, 0, 0.1f);
                        StartCoroutine(Attaque());
                        GetComponent<FollowPlayer>().interrupt = true;
                    }
                } else
                {
                    peutAttaquer = false;
                    nva.speed = Mathf.Lerp(nva.speed, 0, 0.1f);
                    StartCoroutine(Attaque());
                    GetComponent<FollowPlayer>().interrupt = true;
                }
            } else
            {
                GetComponent<FollowPlayer>().interrupt = false;
            }
        }

        IEnumerator Attaque()
        {
            if (colliderHit != null && mourant == false)
            {
                colliderHit.enabled = true;
            }
            yield return new WaitForSeconds(0.1f);
            animator.SetTrigger("Attack");
            if (referenceSO.projectilePrefab != null && mourant == false)
            {
                Instantiate(referenceSO.projectilePrefab, gameObject.transform);
            }
            //On attend la fin de l'anim
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
            //On remet la velocite a l'ennemi 
            nva.speed = Mathf.Lerp(0, 15, 0.1f);
            yield return new WaitForSeconds(0.2f);
            if (colliderHit != null)
            {
                colliderHit.enabled = false;
            }
        }

        public void OnMort()
        {
            DataHolder dc = FindObjectOfType<DataHolder>();

            if(dc != null && mourant == false)
            {
                if(colliderHit !=null)
                    colliderHit.enabled = false; 
                dc.nbEnnemisTues += 1;
                mourant = true;

                nva.speed = 0f;
                animator.SetTrigger("Death");
                StartCoroutine(despawn());
            }

        }

        IEnumerator despawn()
        {
            //On les laisse terminer leur animation et le temps de se reset
            yield return new WaitForSeconds(2f);
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
            //On reinitialise les stats pour que l'objet soit réutilisable 
            Init();
            gameObject.SetActive(false);
        }


        public void OnHit(float damage)
        {
            //On applique l'animation
            animator.SetTrigger("Hit");
            //On arrête le personnage pour qu'il effectue l'anim sans courir
            nva.speed = Mathf.Lerp(nva.speed, 0, 0.1f); 
            
            pv = pv - damage;
            //Si des evenements supp sont necessaires pour cet ennemi, les invoque 
            onHit?.Invoke();
        }

        public void OnHitPersonnage()
        {
            personnage.onHit(degatAttaque);
        }

    }

}