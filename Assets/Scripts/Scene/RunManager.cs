using Ennemis;
using Personnage;
using Save;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Fait pour gerer les debuts et fin de run, et garder le temps en compte. 
/// </summary>
public class RunManager : MonoBehaviour
{
    [Header("Scene Reference")]
    public SceneReference sceneReference;
    [Header("Ne pas modifier, seulement pour debug")]
    public float tempsRun = 0f;
    [Header("Ennemis à tuer pour clear")]
    public int limiteEnnemis = 100;
    [Header("Recompense fin niveau")]
    public int recompenseFinRun = 200;
    [Header("Est-ce qu'il faut tuer des ennemis ou atteindre un objectif ?")]
    public bool killWinBool = true;
    [HideInInspector]
    public bool endOfLevel = false; 
    private bool runStart = false;
    [HeaderAttribute("UI")]
    public GameObject endUIGameObject;
    public GameObject gameOverUIGameObject;
    private DataHolder dc;

    // Start is called before the first frame update
    void Start()
    {
        dc = FindObjectOfType<DataHolder>();
        runStart = true;
        dc.nbEnnemisTues = 0;
        dc.currentMapScene = sceneReference;

        endUIGameObject.SetActive(false);
        gameOverUIGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (runStart==true)
        {
            tempsRun += Time.deltaTime;
        }

        if((killWinBool == false && endOfLevel == true)||(dc.nbEnnemisTues >= limiteEnnemis & runStart == true && killWinBool == true))
        {
            endOfRun(); 
        }
    }

    public void setEndOfLevel(bool b)
    {
        endOfLevel = b;
    }
    /// <summary>
    /// Appele quand on a termine une map.
    /// </summary>
    void endOfRun()
    {
        dc.endOfRun(tempsRun, recompenseFinRun); 
        runStart = false;

        PersonnageController pc = FindObjectOfType<PersonnageController>();
        List <Spawner> spawners = FindObjectsOfType<Spawner>().ToList();
        List <EnnemiController> ec = FindObjectsOfType<EnnemiController>().ToList();


        foreach(Spawner s in spawners)
        {
            s.enabled = false; 
        }
        foreach (EnnemiController e in ec)
        {
            e.OnMort();
        }

        FindObjectOfType<InputManager>().ToggleActionMap("UI");
        Cursor.lockState = CursorLockMode.Confined; 
        endUIGameObject.SetActive(true);
    }

    public void gameOver()
    {

        dc.endOfRun(tempsRun, 0);
        runStart = false;

        PersonnageController pc = FindObjectOfType<PersonnageController>();
        List<Spawner> spawners = FindObjectsOfType<Spawner>().ToList();
        List<EnnemiController> ec = FindObjectsOfType<EnnemiController>().ToList();


        foreach (Spawner s in spawners)
        {
            s.enabled = false;
        }
        foreach (EnnemiController e in ec)
        {
            e.OnMort();
        }

        FindObjectOfType<InputManager>().ToggleActionMap("UI");
        Cursor.lockState = CursorLockMode.Confined;
        gameOverUIGameObject.SetActive(true);
    }
}
