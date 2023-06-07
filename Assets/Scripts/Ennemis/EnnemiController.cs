using Personnage;
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
        private bool peutAttaquer = false; 
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

        void Start()
        {
            Init();
            personnage = FindObjectOfType<PersonnageController>();
            animator = GetComponent<Animator>();
            nva = GetComponent<NavMeshAgent>();
            if(GetComponent<FollowPlayer>() != null)
            {
                GetComponent<FollowPlayer>().maxDistance = porteeAttaque;

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
                peutAttaquer = false; 
                nva.speed = Mathf.Lerp(nva.speed, 0, 0.1f);
                StartCoroutine(Attaque());
                GetComponent<FollowPlayer>().interrupt = true; 
            } else
            {
                GetComponent<FollowPlayer>().interrupt = false;
            }
        }

        IEnumerator Attaque()
        {

            animator.SetTrigger("Attack");
            if(referenceSO.projectilePrefab != null)
            {
                Instantiate(referenceSO.projectilePrefab, gameObject.transform);
            }
            //On attend la fin de l'anim
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
            //On remet la velocite a l'ennemi 
            nva.speed = Mathf.Lerp(0, 15, 0.1f);
        }

        public void OnMort()
        {
            nva.speed = 0f;
            animator.SetTrigger("Death");
            StartCoroutine(despawn());
            Debug.Log("death");
        }

        IEnumerator despawn()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
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