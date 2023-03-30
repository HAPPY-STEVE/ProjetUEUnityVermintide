using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Permet de d�finir toutes les propri�t�s li�es � une arme, et d'initialiser le personnage en consequence.
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
    public bool armeProjectile;
    public GameObject projectilePrefab;
}

[CustomEditor(typeof(Arme))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as Arme;


        EditorGUI.indentLevel++;
        EditorGUILayout.PrefixLabel("Nom arme");
        myScript.nom = EditorGUILayout.TextField(myScript.nom);
        EditorGUILayout.PrefixLabel("PV");
        myScript.pv = EditorGUILayout.IntSlider(myScript.pv, 0, 200);
        EditorGUILayout.PrefixLabel("Degats");
        myScript.degats = EditorGUILayout.IntSlider(myScript.degats, 0, 200);
        EditorGUILayout.PrefixLabel("defense");
        myScript.defense = EditorGUILayout.IntSlider(myScript.defense, 0, 25);
        EditorGUILayout.PrefixLabel("magie");
        myScript.magie = EditorGUILayout.IntSlider(myScript.magie, 0, 200);
        EditorGUILayout.PrefixLabel("vitesse Attaque");
        myScript.vitesseAttaque = EditorGUILayout.FloatField(myScript.vitesseAttaque);
        EditorGUILayout.PrefixLabel("vitesse Mouvement");
        myScript.vitesseMouvement = EditorGUILayout.FloatField(myScript.vitesseMouvement);
        EditorGUILayout.PrefixLabel("Regeneration de pv/sec");
        myScript.regenPV = EditorGUILayout.FloatField(myScript.regenPV);
        EditorGUI.indentLevel--;

        myScript.armeProjectile = GUILayout.Toggle(myScript.armeProjectile, "Arme � projectiles");

        if (myScript.armeProjectile == true)
            myScript.projectilePrefab = (GameObject) EditorGUILayout.ObjectField(myScript.projectilePrefab, typeof(GameObject), true);

    }
}