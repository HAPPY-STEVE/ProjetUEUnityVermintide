using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prefabs/Ennemis")]
public class EnnemiSO : ScriptableObject
{
    public string nom;
    [Multiline]
    public string description;
    [Header("Attaque")]
    //Determine si l'ennemi est melee ou ranged, et sa distance minimale pour attaquer
    [SerializeField, Range(0, 25f)]
    public float porteeAttaque;
    public float pv;
    [SerializeField, Range(0, 100f)]
    public float degatAttaque;
    [Header("Animations")]
    public Animation marcheAnimation;
    public Animation attaqueAnimation;
    public Animation mortAnimation;
    [Header("Prefab")]
    public GameObject ennemiPrefab; 

}
