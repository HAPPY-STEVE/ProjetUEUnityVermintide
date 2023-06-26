using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{

    [CreateAssetMenu(menuName = "Scene Reference")]
    /// <summary>
    /// Sert à repérer la prochaine map lors de la transition avec les choix des upgrades.
    /// </summary>
    public class SceneReference : ScriptableObject
    {
        public Object scenePrecedente;
        public Object sceneSuivante;
    }

}