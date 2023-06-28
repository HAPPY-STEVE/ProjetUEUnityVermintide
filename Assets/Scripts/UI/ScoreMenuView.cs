using Save;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreMenuView : MonoBehaviour
    {
        [TextArea]
        public string objective; 
        public TextMeshProUGUI objectiveText;
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

            if(objective != null)
            {
                objectiveText.text = objective; 
            }
        }

        // Update is called once per frame
        void Update()
        {
            scoreText.text = dc.nbEnnemisTues.ToString();
            tempsText.text = ((int)rm.tempsRun).ToString(); 
        }
    }


}