using Armes;
using Personnage;
using Personnage.Upgrade;
using Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Save
{
    public class DataHolder : Singleton<DataHolder>
    {
        [Header("Valeurs pour fin de run")]
        public int nbEnnemisTues = 0;
        public float tempsRun = 0;
        [Range(0, 10000)]
        public float monnaie = 0;
        /// <summary>
        /// Donne lors du choix des upgrades pour indiquer quelle map vient d'être terminé et quelle map la suit. 
        /// </summary>
        public SceneReference currentMapScene;
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

        }

        public void addUpgradeFloor(UpgradeStore upgrade)
        {
            Debug.Log(upgrade.stat.ToString());
            Debug.Log(upgradesFloor);
            if(upgradesFloor == null)
            {
                upgradesFloor = new List<UpgradeStore>();
            }
            upgradesFloor.Add(upgrade);
            allUpgrades.Add(upgrade);
        }

        public void setArme(Arme arme)
        {
            armechoisi = arme;
        }

        public void endOfRun(float t)
        {
            tempsRun += t;
            monnaie += 250; 
        }
    }

}

