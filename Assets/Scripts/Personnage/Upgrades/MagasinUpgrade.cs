using Personnage.Upgrade;
using Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Personnage.Upgrade
{
    [RequireComponent(typeof(Upgrade))]
    public class MagasinUpgrade : MonoBehaviour
    {
        public int prix; 
        private Upgrade up;
        private DataHolder dh; 
        // Start is called before the first frame update
        void Start()
        {
            up = GetComponent<Upgrade>();
            dh = FindObjectOfType<DataHolder>(); 
        }

        public void Achat()
        {
            if(dh.monnaie > prix)
            {
                dh.monnaie -= prix;
                up.resolveUpgrade(); 
            }
        }

    }
}
