using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject selectUnitActionCursor;
    [SerializeField] private GameObject unitActionMenu;
    [SerializeField] private List<GameObject> unitActionMenuButtons;
    //[SerializeField] private List<GameObject> enemyUnitList;
    [SerializeField] private List<EnemyUnit> enemyUnitList;

    private StateMachine playerStateMachine;

    public PlayerUnit PlayerUnit;
    public StateMachine PlayerStateMachine => playerStateMachine;
    public GameObject SelectUnitActionCursor => selectUnitActionCursor;
    public GameObject UnitActionMenu => unitActionMenu;
    public List<GameObject> UnitActionMenuButtons => unitActionMenuButtons;
    //public List<GameObject> EnemyUnitList => enemyUnitList;
    public List<EnemyUnit> EnemyUnitList => enemyUnitList;
    private void Awake()
    {
        playerStateMachine = new StateMachine(this);
        //enemyUnitList = GameObject.FindGameObjectsWithTag("Enemy").ToList();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine.Initialize(playerStateMachine.viewMapState);
    }

    // Update is called once per frame
    void Update()
    {
        playerStateMachine.Update();

        if (Input.GetKeyDown(KeyCode.R))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }
    public void QuitGame()
    {
    #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
    #else
                        Application.Quit();
    #endif
    }
}