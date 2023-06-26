using Save;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Personnage.Upgrade
{

    public class Upgrade : MonoBehaviour
    {
        [Header("Texte UI")]
        public TextMeshProUGUI nom; 
        public TextMeshProUGUI valeur; 
        public TextMeshProUGUI description; 
        [Header("Variables")]
        public Stats stat; 
        public string titreUpgrade; 
        public string texteDescriptionUpgrade; 
        public int valeurUpgrade;
        public bool generateTexteEtDescription = false;


        public void Start()
        {
            if(generateTexteEtDescription == true)
            {
                generationTexteEtDescription();
                afficherUpgrade(); 
            }
        }

        public void generationTexteEtDescription()
        {
            switch (stat)
            {
                case Stats.pv:
                    titreUpgrade = "Points de vie";
                    texteDescriptionUpgrade = "Augmente les points de vie.";
                    break;
                case Stats.degats:
                    titreUpgrade = "D�g�ts";
                    texteDescriptionUpgrade = "Augmente les d�g�ts de m�l�e/distance.";
                    break;
                case Stats.defense:
                    titreUpgrade = "D�fense";
                    texteDescriptionUpgrade = "Augmente la d�fense contre tout type de d�g�ts.";
                    break;
                case Stats.magie:
                    titreUpgrade = "Magie";
                    texteDescriptionUpgrade = "Augmente les d�g�ts de magie.";
                    break;
                case Stats.vitesseAttaque:
                    titreUpgrade = "Vitesse d'attaque";
                    texteDescriptionUpgrade = "Augmente la vitesse d'attaque global.";
                    break;
                case Stats.vitesseMouvement:
                    titreUpgrade = "Vitesse de mouvement";
                    texteDescriptionUpgrade = "Augmente la vitesse de mouvement.";
                    break;
                case Stats.regenPV:
                    titreUpgrade = "R�g�neration de vie";
                    texteDescriptionUpgrade = "Augmente la r�g�n�ration passive de vie par seconde.";
                    break;
                default:
                    titreUpgrade = "Points de vie";
                    texteDescriptionUpgrade = "Augmente les points de vie.";
                    break;
            }
        }
        public void generateRandomUpgrade(Stats statChoisi, int min, int max)
        {
            stat = statChoisi; 
            switch (statChoisi)
            {
                case Stats.pv:
                    titreUpgrade = "Points de vie";
                    texteDescriptionUpgrade = "Augmente les points de vie.";
                    valeurUpgrade = UnityEngine.Random.Range(min, max);
                    break;
                case Stats.degats:
                    titreUpgrade = "D�g�ts";
                    texteDescriptionUpgrade = "Augmente les d�g�ts de m�l�e/distance.";
                    valeurUpgrade = UnityEngine.Random.Range(min, max);
                    break;
                case Stats.defense:
                    titreUpgrade = "D�fense";
                    texteDescriptionUpgrade = "Augmente la d�fense contre tout type de d�g�ts.";
                    valeurUpgrade = UnityEngine.Random.Range(min, max);
                    break;
                case Stats.magie:
                    titreUpgrade = "Magie";
                    texteDescriptionUpgrade = "Augmente les d�g�ts de magie.";
                    valeurUpgrade = UnityEngine.Random.Range(min, max);
                    break;
                case Stats.vitesseAttaque:
                    titreUpgrade = "Vitesse d'attaque";
                    texteDescriptionUpgrade = "Augmente la vitesse d'attaque global.";
                    valeurUpgrade = UnityEngine.Random.Range(min, max);
                    break;
                case Stats.vitesseMouvement:
                    titreUpgrade = "Vitesse de mouvement";
                    texteDescriptionUpgrade = "Augmente la vitesse de mouvement.";
                    valeurUpgrade = UnityEngine.Random.Range(min, max);
                    break;
                case Stats.regenPV:
                    titreUpgrade = "R�g�neration de vie";
                    texteDescriptionUpgrade = "Augmente la r�g�n�ration passive de vie par seconde.";
                    valeurUpgrade = UnityEngine.Random.Range(min, max);
                    break;
                default:
                    titreUpgrade = "Points de vie";
                    texteDescriptionUpgrade = "Augmente les points de vie.";
                    valeurUpgrade = UnityEngine.Random.Range(min, max);
                    break;
            }
        }

        public void afficherUpgrade()
        {
            nom.text = titreUpgrade;
            valeur.text = valeurUpgrade.ToString() + "%";
            description.text = texteDescriptionUpgrade;
        }



        public void resolveUpgrade()
        {
            DataHolder dh = FindObjectOfType<DataHolder>();
            dh.addUpgradeFloor(new UpgradeStore(stat, titreUpgrade, texteDescriptionUpgrade, valeurUpgrade));
        }
    }
    public class UpgradeStore
    {
        public Stats stat;
        public string titreUpgrade;
        public string texteDescriptionUpgrade;
        public int valeurUpgrade;

        public UpgradeStore(Stats s, string titre, string texte, int val)
        {
            stat = s;
            titreUpgrade = titre;
            texteDescriptionUpgrade = texte;
            valeurUpgrade = val; 
        }
    }

}