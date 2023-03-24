using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Permet de définir toutes les propriétés liées à une arme, et d'initialiser le personnage en consequence.
/// A definir. 
/// </summary>
[CreateAssetMenu(menuName = "Arme")]
public class Arme : ScriptableObject
{
    public string nom;
    public int pv;
    public int degats;
    public int defense;
    public int magie;
    public float vitesseAttaque;
    public float vitesseMouvement;
    public float regenPV;

}
