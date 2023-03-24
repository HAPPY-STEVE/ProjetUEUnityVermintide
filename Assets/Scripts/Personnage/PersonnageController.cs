using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Personnage
{
    public class PersonnageController : MonoBehaviour
    {
        [Header("Arme actuelle")]
        public Arme arme;
        [Header("Caracteristiques primaires")]
        [Range(0, 100)]
        [SerializeField]
        private int pv = 100;
        public int degats = 0;
        public int defense = 0;
        public int magie = 0;
        [Header("Caracteristiques secondaires")]
        public float vitesseAttaque = 0;
        public float vitesseMouvement = 0;
        public float regenPV = 0;

        // Start is called before the first frame update
        void Start()
        {
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
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

