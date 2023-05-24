using Cinemachine;
using Ennemis;
using Save;
using Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Tween;
using UnityEngine;
using UnityEngine.UI;

namespace Personnage.Upgrade
{
    public enum Stats
    {
        pv,
        degats,
        defense,
        magie,
        vitesseAttaque,
        vitesseMouvement,
        regenPV
    }

    /// <summary>
    /// Gere les upgrades affiches apres un niveau. 
    /// </summary>
    public class UpgradeManager : MonoBehaviour
    {
        [Header("Variables upgrades")]
        [Range(1,7)]
        public int nbUpgrade =  5;
        [Header("Canvas Parent des upgrades UI")]
        public GameObject parent;
        /// <summary>
        /// La stat, et la valeur min/max possible pour l'upgrade 
        /// </summary>
        public Dictionary<Stats, Tuple<int, int>> valeursStats;


        // Start is called before the first frame update
        void Start()
        {
            //On vide les upgrades floors quand on arrive au choix des upgrades
            DataHolder dh = FindObjectOfType<DataHolder>();
            if(dh != null)
            {
                dh.upgradesFloor = null;
            }

            valeursStats = new Dictionary<Stats, Tuple<int, int>>()
            {
            { Stats.pv, new Tuple<int, int>( 9, 11)},
            { Stats.degats, new Tuple<int, int>( 9, 11)},
            { Stats.defense, new Tuple<int, int>( 9, 11)},
            { Stats.magie, new Tuple<int, int>( 9, 11)},
            { Stats.vitesseAttaque, new Tuple<int, int>( 9, 11)},
            { Stats.vitesseMouvement, new Tuple<int, int>( 9, 11)},
            { Stats.regenPV, new Tuple<int, int>( 9, 11)},
            };

            StartCoroutine(InstantiateUpgrades());
        }

        /// <summary>
        /// Crée les objets upgrades sur l'UI 
        /// </summary>
        IEnumerator InstantiateUpgrades()
        {
            yield return new WaitUntil(() => ObjectPool.sharedInstance.GetPooledObject("choixUpgrade") != null);
            for (int i = 0; i < nbUpgrade; i++)
            {
                //Les objets upgrades sont mis dans l'object pool pour etre genere a l'init de la scene 
                Debug.Log(ObjectPool.sharedInstance);
                GameObject g = ObjectPool.sharedInstance.GetPooledObject("choixUpgrade");
                Debug.Log(g);
                int randomIndex = UnityEngine.Random.Range(0, valeursStats.Count);
                g.SetActive(true);
                KeyValuePair<Stats, Tuple<int, int>> statChoisi = valeursStats.ElementAt(randomIndex);
                g.GetComponent<Button>().onClick.AddListener(() => { EndUpgrades(); });
                g.GetComponent<Upgrade>().generateRandomUpgrade(valeursStats.ElementAt(randomIndex).Key, statChoisi.Value.Item1, statChoisi.Value.Item2);
                g.GetComponent<Upgrade>().afficherUpgrade();
                g.GetComponent<RevealTween>().tempsDelay = 0.2f * i; 
            }
        }

        private void EndUpgrades()
        {
            LevelLoader levelLoader = FindObjectOfType<LevelLoader>();
            parent.GetComponent<RevealTween>().OnClose(); 
            CinemachineVirtualCamera c = (CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            c.Priority = 0;
            StartCoroutine(levelLoader.LevelLoading("MainMenu"));

        }
    }

}