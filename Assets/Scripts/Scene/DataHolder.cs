using Armes;
using Personnage;
using Personnage.Upgrade;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    public class DataHolder : Singleton<DataHolder>
    {
        [Header("Valeurs pour fin de run")]
        public int nbEnnemisTues;
        public int tempsRun; 
        public int tempsRunMapActuel;
        private PersonnageController pc;
        public Arme armechoisi; 
        /// <summary>
        /// Upgrades choisis et appliques au prochain floor
        /// </summary>
        public List<UpgradeStore> upgradesFloor = new List<UpgradeStore>();
        /// <summary>
        /// Garde en memoire tout les upgrades choisis 
        /// </summary>
        public List<UpgradeStore> allUpgrades = new List<UpgradeStore>();

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake(); 

            pc = FindObjectOfType<PersonnageController>();
            if(pc != null & armechoisi != null)
            {
                pc.arme = armechoisi; 
            }
        }

        public void addUpgradeFloor(UpgradeStore upgrade)
        {
            Debug.Log(upgrade);
            Debug.Log(upgradesFloor);
            if(upgradesFloor == null)
            {
                upgradesFloor = new List<UpgradeStore>();
            }
            upgradesFloor.Add(upgrade);
            allUpgrades.Add(upgrade);
        }

    }

}
