using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        private CharacterController personnage;
        private string nom;
        private string description;
        [Header("Attaque")]
        //Determine si l'ennemi est melee ou ranged, et sa distance minimale pour attaquer
        [SerializeField, Range(0, 25f)]
        private float porteeAttaque;
        public float pv;
        [SerializeField, Range(0, 100f)]
        private float degatAttaque;
        [Header("Animations")]
        private Animation marcheAnimation;
        private Animation attaqueAnimation;
        private Animation mortAnimation;
        private Animator animator;
        [Header("Prefab")]
        private GameObject ennemiPrefab;
        [Header("Events")]
        public UnityEvent onMort;
        public UnityEvent onHit;

        void Start()
        {
            Init();
            Debug.Log(pv);
            personnage = FindObjectOfType<CharacterController>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if(pv <= 0)
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
            if(dist <= porteeAttaque)
            {
                StartCoroutine(Attaque());            
            }
        }

        IEnumerator Attaque()
        {
            animator.SetTrigger("Attaque");
            yield return null;
        }

        public void OnMort()
        {
            animator.SetTrigger("Death");
            StartCoroutine(despawn());
        }

        IEnumerator despawn()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }


        public void OnHit(float damage)
        {
            animator.SetTrigger("Hit");
            onHit?.Invoke();
            pv = pv - damage;
            Debug.Log(pv);
        }

    }

}