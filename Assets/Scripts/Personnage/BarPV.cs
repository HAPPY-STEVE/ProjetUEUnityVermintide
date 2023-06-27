using Personnage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Bar de pv lie au personnageController. 
    /// </summary>
    public class BarPV : MonoBehaviour
    {
        [Header("Est-ce qu'on affiche le texte ?")]
        public bool showText = true;
        [Header("Est-ce qu'on affiche la bar ?")]
        public bool showBar = true;
        [Header("Variables UI")]
        public TextMeshProUGUI text;
        public GameObject bar;
        [Range(0, 5)]
        public float tempsAnim=0.1f;
        private PersonnageController pc;
        private int pvMax;
        //Partie animation bar
        private Coroutine animBar = null;
        private Color startColor; 

        // Start is called before the first frame update
        void Start()
        {
            pc = FindObjectOfType<PersonnageController>();
            text.gameObject.SetActive(false);
            bar.SetActive(false);
            startColor = Color.red; 

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
                //On ajoute à l'evenement de hit du joueur pour l'appeler à chaque dégât reçu. 
                pc.onHitEvent.AddListener(() => updatePv());
            }

        }

        void updatePv()
        {
            float percent = ((float)pc.Pv / (float)pvMax) * 100;
            if (percent < 0 || pc.Pv < 0)
            {
                percent = 0; 
            }
            Debug.Log(pc.Pv);
            text.text = percent.ToString();
            float f = percent / 100;
            if(animBar == null)
            {
                animBar = StartCoroutine(AnimationBar(bar.GetComponent<Image>().fillAmount, f));

            } else
            {
                Debug.Log("test");
                StopAllCoroutines();
                animBar = StartCoroutine(AnimationBar(bar.GetComponent<Image>().fillAmount, f));
            }
        }

        IEnumerator AnimationBar(float startValue, float endValue)
        {
            float displayValue = 0f; // value during animation
            float timer = 0f;
            float endTimer = tempsAnim;
            Color endColor = startColor;
            bar.GetComponent<Image>().color = Color.white;
            while (timer <= endTimer)
            {
                timer += Time.deltaTime;
                displayValue = Mathf.Lerp(startValue, endValue, timer / endTimer);
                bar.GetComponent<Image>().color = Color.Lerp(bar.GetComponent<Image>().color, endColor, timer/ endTimer);
                bar.GetComponent<Image>().fillAmount = displayValue;

                yield return null;
            }
        }

        public static float EaseOut(float t)
        {
            return Flip(Mathf.Sqrt(Flip(t)));
        }

        static float Flip(float x)
        {
            return 1 - x;
        }

    }

}