using Armes;
using Cinemachine;
using Ennemis;
using Personnage;
using Save;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using UI.Tween;
using UnityEngine;
using UnityEngine.UI;
namespace Scenes
{

    public class NouvellePartieSetup : MonoBehaviour
    {
        List<Arme> armes;
        public GameObject armePrefab;
        public Transform armePositionParent;
        private bool dejaInstantiateArmes = false;
        [Header("Canvas")]
        public Canvas mainCanvas;
        public Canvas armeCanvas; 
        public void setArme(Arme arme)
        {
            DataHolder dh = FindObjectOfType<DataHolder>(); 
            dh.armechoisi = arme;
        }

        /// <summary>
        /// Crée les objets armes sur l'UI 
        /// </summary>
        IEnumerator InstantiateArmes()
        {
            yield return new WaitUntil(() => ObjectPool.sharedInstance.GetPooledObject("choixArme") != null);
            for (int i = 0; i < armes.Count; i++)
            {
                //Les objets armes sont mis dans l'object pool pour etre genere a l'init de la scene 
                Debug.Log(ObjectPool.sharedInstance);
                GameObject g = ObjectPool.sharedInstance.GetPooledObject("choixArme");
                Debug.Log(g);
                g.SetActive(true);
                g.GetComponent<Button>().onClick.AddListener(() => { EndChoix(); 
                FindObjectOfType<DataHolder>().armechoisi = armes[i];
                });
                g.GetComponent<RevealTween>().tempsDelay = 0.2f * i;
            }
        }

        /// <summary>
        /// On a choisi l'arme, transition vers map. 
        /// </summary>
        private void EndChoix()
        {
            //On verifie qu'on sort pas d'une partie pour eviter l'ajout des anciennes stats
            DataHolder dh = FindObjectOfType<DataHolder>();
            if(dh != null)
            {
                dh.currentMapScene = null;
                dh.tempsRun = 0;
                dh.nbEnnemisTues = 0;
            }

            //Code pour transition

            LevelLoader levelLoader = FindObjectOfType<LevelLoader>();
            armePositionParent.GetComponent<RevealTween>().OnClose();
            CinemachineVirtualCamera c = (CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            c.Priority = 0;
            StartCoroutine(levelLoader.LevelLoading("Map1"));

        }

    }

}