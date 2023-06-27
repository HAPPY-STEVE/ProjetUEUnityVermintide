using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Environnement
{

    public class Interactible : MonoBehaviour, IInteractable
    {
        public UnityEvent onInteract;
        private bool inInteraction = false;
        void Start()
        {

        }

        public void Interact()
        {
            onInteract?.Invoke();
        }

    }

}