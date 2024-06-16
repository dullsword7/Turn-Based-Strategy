using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject selectUnitActionCursor;
    [SerializeField] private GameObject unitActionMenu;
    [SerializeField] private List<GameObject> unitActionMenuButtons;
    [SerializeField] private GameObject playerToEnemyTurnTransition;
    [SerializeField] private GameObject enemyToPlayerTurnTransition;
    [SerializeField] private UnitManager unitManager;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject selectUpgradeScreen;
    [SerializeField] private List<GameObject> selectUpgradeButtons;

    [SerializeField] private GameObject battleResultHolder;
    [SerializeField] private GameObject attackingUnit;
    [SerializeField] private GameObject battleResult;
    [SerializeField] private TMP_Text unitAttackCount;
    [SerializeField] private GameObject defendingUnit;

    private StateMachine playerStateMachine;
    private BattleResultHandler battleResultHandler;
    private AllSkills allSkills;

    public PlayerUnit PlayerUnit;
    public StateMachine PlayerStateMachine => playerStateMachine;
    public GameObject SelectUnitActionCursor => selectUnitActionCursor;
    public GameObject PlayerToEnemyTurnTransition => playerToEnemyTurnTransition;
    public GameObject EnemyToPlayerTurnTransition => enemyToPlayerTurnTransition;
    public UnitManager UnitManager => unitManager;
    public GameObject GameOverCanvas => gameOverCanvas;
    public GameObject SelectUpgradeScreen => selectUpgradeScreen;
    public List<GameObject> SelectUpgradeButtons => selectUpgradeButtons;
    public BattleResultHandler BattleResultHandler => battleResultHandler;
    public AllSkills AllSkills => allSkills;

    private Camera mainCamera;
    private Vector3 leftBound;
    private Vector3 rightBound;
    private Vector3 topBound;
    private Vector3 bottomBound;
    private Vector3 originalPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
        CalculateCursorBounds();
    }
    // Start is called before the first frame update
    void Start()
    {
        allSkills = new AllSkills();
        // playerStateMachine needs to be placed in Start() so that UnitManager can find all player and enemy objects in Awake() before anything else happens
        playerStateMachine = new StateMachine(this);
        battleResultHandler = new BattleResultHandler(battleResultHolder, attackingUnit, battleResult, unitAttackCount, defendingUnit);

        playerStateMachine.Initialize(playerStateMachine.selectUpgradeState);
    }

    // Update is called once per frame
    void Update()
    {
        playerStateMachine.Update();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ZoomOutCamera();
            CalculateCursorBounds();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ZoomInCamera();
            CalculateCursorBounds();
        }
    }
    public bool CheckCursorInBounds()
    {
        bool verticalCheck = false; 
        bool horizontalCheck = false; 
        if (transform.position.x > leftBound.x && transform.position.x < rightBound.x)
        {
            horizontalCheck = true; 
        }
        if (transform.position.y > bottomBound.y && transform.position.y < topBound.y)
        {
            verticalCheck = true;
        }
        return horizontalCheck && verticalCheck;
    }
    public void CalculateCursorBounds()
    {
        leftBound = mainCamera.ViewportToWorldPoint(new Vector3(.02f, .02f, 0));
        rightBound = mainCamera.ViewportToWorldPoint(new Vector3(.98f, 0, 0));
        topBound = mainCamera.ViewportToWorldPoint(new Vector3(0, .98f, 0));
        bottomBound = mainCamera.ViewportToWorldPoint(new Vector3(.02f, .02f, 0));

    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(leftBound, .2f);
        Gizmos.DrawSphere(rightBound, .2f);
        Gizmos.DrawSphere(topBound, .2f);
    }
    public void ZoomOutCamera()
    {
        if (Camera.main.orthographicSize >= 10) return;
        Camera.main.orthographicSize += 1;
        ResetCursorPosition();
    }
    public void ZoomInCamera()
    {
        if (Camera.main.orthographicSize <= 5) return;
        Camera.main.orthographicSize -= 1;
        ResetCursorPosition();
    }
    public void ResetCursorPosition()
    {
        if (PlayerUnit != null)
        {
            transform.position = PlayerUnit.transform.position;
        }
        else
        {
            transform.position = originalPosition;
        }
    }
    public void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
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