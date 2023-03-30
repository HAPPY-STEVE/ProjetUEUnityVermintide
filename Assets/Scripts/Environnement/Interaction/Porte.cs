using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environnement
{

[RequireComponent(typeof(Animator))]
public class Porte : MonoBehaviour, IInteractable
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Interact()
    {
        anim.SetTrigger("open");
    }
}

}