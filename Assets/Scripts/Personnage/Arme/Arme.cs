using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Armes
{

/// <summary>
/// Permet de définir toutes les propriétés liées à une arme, et d'initialiser le personnage en consequence.
/// A definir. 
/// </summary>
[CreateAssetMenu(menuName = "Arme")]
public class Arme : ScriptableObject
{
    public string nom;
    public AnimatorOverrideController controllerOverride;
    public GameObject armePrefab;
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
#if UNITY_EDITOR
    [CustomEditor(typeof(Arme))]
public class MyScriptEditor : Editor
{
    public bool displaysExpanded;

    override public void OnInspectorGUI()
    {

        serializedObject.Update();
        var myScript = target as Arme;


        EditorGUI.indentLevel++;
        EditorGUILayout.PrefixLabel("Nom arme");
        myScript.nom = EditorGUILayout.TextField(myScript.nom);
        EditorGUILayout.PrefixLabel("Animator Controller Override pour l'arme");
        myScript.controllerOverride = (AnimatorOverrideController)EditorGUILayout.ObjectField(myScript.controllerOverride, typeof(AnimatorOverrideController), true);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(myScript);
            EditorGUILayout.PrefixLabel("Prefab arme");
        myScript.armePrefab = (GameObject)EditorGUILayout.ObjectField(myScript.armePrefab, typeof(GameObject), true);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(myScript);
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

        myScript.armeProjectile = GUILayout.Toggle(myScript.armeProjectile, "Arme à projectiles");

        if (myScript.armeProjectile == true)
        {
            EditorPrefs.SetBool("FoldFlagArmeProjectile", true);
            myScript.armeProjectile = EditorPrefs.GetBool("FoldFlagArmeProjectile");
            myScript.projectilePrefab = (GameObject)EditorGUILayout.ObjectField(myScript.projectilePrefab, typeof(GameObject), true);
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(myScript);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
#endif
}