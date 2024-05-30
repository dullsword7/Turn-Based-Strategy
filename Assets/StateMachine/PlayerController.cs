using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerUnit playerUnit;
    [SerializeField] private GameObject selectUnitActionCursor;
    [SerializeField] private GameObject unitActionMenu;
    [SerializeField] private GameObject[] unitActionMenuButtons;

    private StateMachine playerStateMachine;
    public StateMachine PlayerStateMachine => playerStateMachine;
    public GameObject SelectUnitActionCursor => selectUnitActionCursor;
    public GameObject[] UnitActionMenuButtons => unitActionMenuButtons;
    public GameObject UnitActionMenu => unitActionMenu;
    private void Awake()
    {
        playerStateMachine = new StateMachine(this);
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
    }
}
