using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectUpgradeState : IState
{
    private PlayerController player;
    private int currentMenuButtonIndex;

    private TMP_Text optionOneDescription;
    private TMP_Text optionTwoDescription;
    private TMP_Text optionThreeDescription;

    private List<Skill> skillOptions;

    public SelectUpgradeState(PlayerController player)
    {
        this.player = player;
        skillOptions = new List<Skill>();

        InitializeButtonEvents();
    }
    public void Enter()
    {
        Debug.Log("Entering SelectUpgradeState");
        player.SelectUpgradeScreen.SetActive(true);

        Color color;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.SelectUpgradeButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;

        GenerateSkillOptions();
        SetupSkillOptionText();
    }
    public void Update()
    {
        HandleMenuCursorInput();
    }
    public void Exit()
    {
        Debug.Log("Exiting SelectUpgradeState");

        player.SelectUpgradeScreen.SetActive(false);

        Color color;
        ColorUtility.TryParseHtmlString(Constants.DEFAULT_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        foreach (GameObject gameObject in player.SelectUpgradeButtons)
        {
            gameObject.GetComponent<Image>().color = color;
            
        }
        
        player.PlayerStateMachine.PreviousState = this;
    }
    private void HandleMenuCursorInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextMenuButton();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousMenuButton();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            player.SelectUpgradeButtons[currentMenuButtonIndex].GetComponent<Button>().onClick.Invoke();
        }
    }

    private void NextMenuButton()
    {
        Color color;
        ColorUtility.TryParseHtmlString(Constants.DEFAULT_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.SelectUpgradeButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
        currentMenuButtonIndex += 1;
        if (currentMenuButtonIndex >= player.SelectUpgradeButtons.Count) currentMenuButtonIndex = 0;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.SelectUpgradeButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
    }

    private void PreviousMenuButton()
    {
        Color color;
        ColorUtility.TryParseHtmlString(Constants.DEFAULT_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.SelectUpgradeButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
        currentMenuButtonIndex -= 1;
        if (currentMenuButtonIndex < 0) currentMenuButtonIndex = player.SelectUpgradeButtons.Count - 1;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.SelectUpgradeButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
    }

    private void InitializeButtonEvents()
    {
        foreach (GameObject go in player.SelectUpgradeButtons)
        {
            SetupButtonEventListeners(go);
        }
    }
    private void SetupButtonEventListeners(GameObject go)
    {
        go.GetComponent<Button>().onClick.AddListener(ApplyUpgrade);
    }

    private void ApplyUpgrade()
    {
        Debug.Log("Applying upgrade");
        skillOptions[currentMenuButtonIndex].ApplySkillEffect(player.UnitManager.playerUnitList);
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
    }
    private void GenerateSkillOptions()
    {
        int numberOfPossibleSkills = player.AllSkills.PossibleSkills.Count;
        skillOptions.Add(player.AllSkills.PossibleSkills[Random.Range(0, numberOfPossibleSkills - 1)]);
        skillOptions.Add(player.AllSkills.PossibleSkills[Random.Range(0, numberOfPossibleSkills - 1)]);
        skillOptions.Add(player.AllSkills.PossibleSkills[Random.Range(0, numberOfPossibleSkills - 1)]);
    }
    private void SetupSkillOptionText()
    {
        List<GameObject> upgradeButtons = player.SelectUpgradeButtons;
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            TMP_Text buttonSkillDescription = upgradeButtons[i].GetComponentInChildren<TMP_Text>();
            string skillName = skillOptions[i].SkillName;
            string skillDescription = skillOptions[i].SkillDescription;
            buttonSkillDescription.SetText(skillName + "\n\n\n" + skillDescription);
        }
    }
}
