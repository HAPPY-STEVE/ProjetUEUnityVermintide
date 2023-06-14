using Save;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreMenuView : MonoBehaviour
    {
        public TextMeshProUGUI tempsText;
        public TextMeshProUGUI scoreText;
        public float tempsRun;
        public int score;
        private DataHolder dc;
        private RunManager rm;
        void Start()
        {

            dc = FindObjectOfType<DataHolder>();
            rm = FindObjectOfType<RunManager>(); 
        }

        // Update is called once per frame
        void Update()
        {
            tempsText.text = dc.nbEnnemisTues.ToString();
            scoreText.text = rm.tempsRun.ToString(); 
        }
    }


}