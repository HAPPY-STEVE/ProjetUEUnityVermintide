using System.Collections;
using System.Collections.Generic;
using Personnage;
using Ennemis;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private PersonnageController pc;
    private int degats;

    void Start()
    {
        pc = FindObjectOfType<PersonnageController>();
        degats = pc.degats;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<EnnemiController>() == true)
        {
            Debug.Log(other.name);
            other.GetComponent<Animator>().SetTrigger("Hit");
            other.gameObject.GetComponent<EnnemiController>().OnHit(degats);



        }
    }

}
