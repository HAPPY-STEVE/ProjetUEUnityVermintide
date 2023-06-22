using Personnage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class BarPV : MonoBehaviour
    {
        [Header("Est-ce qu'on affiche le texte ?")]
        public bool showText = true;
        [Header("Est-ce qu'on affiche la bar ?")]
        public bool showBar = true;
        [Header("Variables UI")]
        public TextMeshProUGUI text;
        public GameObject bar;
        private PersonnageController pc;
        private int pvMax;

        // Start is called before the first frame update
        void Start()
        {
            pc = FindObjectOfType<PersonnageController>();
            text.gameObject.SetActive(false);
            bar.SetActive(false);
            if (showBar == true)
            {
                bar.SetActive(true);
            }

            if (showText == true)
            {
                text.gameObject.SetActive(true);
            }

            if(pc != null)
            {
                pvMax = pc.arme.pv;
                pc.onHitEvent.AddListener(() => updatePv());
            }

        }

        // Update is called once per frame
        void updatePv()
        {
            float percent = ((float)pc.Pv / (float)pvMax) * 100;
            if (percent < 0)
            {
                percent = 0; 
            }
            text.text = percent.ToString();
            bar.GetComponent<Image>().fillAmount = percent / 100;
        }
    }

}