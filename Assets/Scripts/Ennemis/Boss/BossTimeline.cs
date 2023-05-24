using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Ennemis.Boss
{
    /// <summary>
    /// Timeline du boss avec % de vie et les events qui viennent avec. 
    /// </summary>
    [RequireComponent(typeof(EnnemiController))]
    public class BossTimeline : MonoBehaviour
    {
        public SerializedDictionary<int, UnityEvent> timeline;
        private EnnemiController ennemiRef;
        private float pvMax; 
        // Start is called before the first frame update
        void Start()
        {
            ennemiRef = GetComponent<EnnemiController>();
            pvMax = ennemiRef.referenceSO.pv; 
        }

        // Update is called once per frame
        void Update()
        {
            float pvpourcent = (ennemiRef.pv / pvMax) * 100;
            Debug.Log(ennemiRef.pv);
            Debug.Log(pvMax);
            Debug.Log(timeline.Where(x => x.Key >= pvpourcent).ToList().Count());

            if (timeline.Where(x => x.Key >= pvpourcent).ToList().Count() > 0)
            {
                List<KeyValuePair<int, UnityEvent>> c = timeline.Where(x => x.Key >= pvpourcent).ToList();
                Debug.Log(c[0].Key);
                c[0].Value?.Invoke(); 
                timeline.Remove(c[0].Key); 
            }
        }
    }

}