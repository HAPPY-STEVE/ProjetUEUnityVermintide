using Personnage.Upgrade;
using Save;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Affichage pour les stats dans le megasin. 
    /// </summary>
    public class ShopView : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI pv;
        public TextMeshProUGUI degats;
        public TextMeshProUGUI magie;
        public TextMeshProUGUI defense;
        public TextMeshProUGUI vitesseAttaque;
        public TextMeshProUGUI vitesseMouvement;
        public TextMeshProUGUI regenPV;
        public TextMeshProUGUI or;
        [Header("Variables")]
        public int nbUpgradePv = 0;
        public int nbUpgradeDegats = 0;
        public int nbUpgradeMagie = 0;
        public int nbUpgradeDefense = 0;
        public int nbUpgradeVitesseAttaque = 0;
        public int nbUpgradeVitesseMouvement = 0;
        public int nbUpgradeRegenPV = 0; 
        private DataHolder dh;
        void Start()
        {
            dh = FindObjectOfType<DataHolder>(); 

        }

        // Update is called once per frame
        void Update()
        {
            calculUpgradesNb();
            updateTextUpgrades();
            or.text = dh.monnaie.ToString(); 
        }

        void calculUpgradesNb()
        {
            nbUpgradePv = dh.allUpgrades.Where(x => x.stat == Stats.pv).Count(); 
            nbUpgradeDegats = dh.allUpgrades.Where(x => x.stat == Stats.degats).Count(); 
            nbUpgradeMagie = dh.allUpgrades.Where(x => x.stat == Stats.magie).Count(); 
             nbUpgradeDefense = dh.allUpgrades.Where(x => x.stat == Stats.defense).Count(); 
           nbUpgradeVitesseAttaque = dh.allUpgrades.Where(x => x.stat == Stats.vitesseAttaque).Count(); 
            nbUpgradeVitesseMouvement = dh.allUpgrades.Where(x => x.stat == Stats.vitesseMouvement).Count(); 
            nbUpgradeRegenPV = dh.allUpgrades.Where(x => x.stat == Stats.regenPV).Count(); 
        }

        void updateTextUpgrades()
        {
            pv.text = "+" + nbUpgradePv + "%";
            degats.text = "+" + nbUpgradeDegats + "%";
            magie.text = "+" + nbUpgradeMagie + "%";
            defense.text = "+" + nbUpgradeDefense + "%";
            vitesseAttaque.text = "+" + nbUpgradeVitesseAttaque + "%";
            vitesseMouvement.text = "+" + nbUpgradeVitesseMouvement + "%";
            regenPV.text = "+" + nbUpgradeRegenPV + "%";
        }
    }

}