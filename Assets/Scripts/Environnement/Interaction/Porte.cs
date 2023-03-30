using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Environnement
{

    public class Porte : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Animator anim;
        public UnityEvent onInteract;
        private bool inInteraction=false;
        void Start()
        {
            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }
        }

        public void Interact()
        {
            if(inInteraction == false)
            {
                onInteract?.Invoke();
                inInteraction = true;
                StartCoroutine(interaction());
            }
        }

        public IEnumerator interaction()
        {

            anim.SetTrigger("open");
            yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
            inInteraction = false;
    }
    }

}