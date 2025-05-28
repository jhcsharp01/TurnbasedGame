using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//��Ʋ���� ����� �Ͽ� ���� ���¸� enum���� ǥ��
public enum BattleTurn
{
    Start               //����
    , Turn1, Turn2       //�÷��̾� ��, ��� ��
    , Action1, Action2   //�÷��̾��� ����, ����� ����
    , Win, Lose          // �� , ��
}

//UI������ �޴� ȭ�鿡 ���� ����
public enum UI_State
{
    MainMenu, SKillMenu
}


public class BattleSystem : MonoBehaviour
{
    //��Ʋ �ý����� ������ ��Ʋ�� ����
    //(�ϸ��� �ٲ�)
    public BattleTurn state;

    //���� UI�� ���� ����
    public UI_State ui_state;

    public Text PhaseText;     //�� ������� �� ����� ���� �ؽ�Ʈ

    public Unit playerUnit;   //�÷��̾�� ���濡 ���� ���
    public Unit otherUnit;


    public Button[] buttons; //�޴� ��ư
    public Button[] skill_buttons; //��ų�� ���� ��ư

    public GameObject mainmenuPanel; //�޴� �� ����
    public GameObject skillmenuPanel;


    //���� �޴��鿡 ���� �ε��� ��
    private int mainMenuIdx = 0;
    private int skillMenuIdx = 0;


    private void Start()
    {
        state = BattleTurn.Start;

        //���� �޴��� ���� ����
        OpenMainMenu();
    }

    private void Update()
    {
        switch (ui_state)
        {
            case UI_State.MainMenu:
                HandleMainMenuInput();
                break;
            case UI_State.SKillMenu:
                HandleSkillMenuInput();
                break;
        }
    }

    private void OpenMainMenu()
    {
        mainmenuPanel.SetActive(true);
        skillmenuPanel.SetActive(false);
        ui_state = UI_State.MainMenu;
        mainMenuIdx = 0;
        //���õ� UI�� ���� ���� - ���� ���õ� ��Ҹ� �����մϴ�.
        EventSystem.current.SetSelectedGameObject(null);

        //���� ��ġ�� ��ư���� ����
        EventSystem.current.SetSelectedGameObject(buttons[mainMenuIdx].gameObject);
    }

    //���� �޴��� ���� ����
    void HandleMainMenuInput()
    {
        //������ �Է¿� ���� �ڵ忡���� �ε��� ó��
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mainMenuIdx = (mainMenuIdx + 1) % buttons.Length;
            EventSystem.current.SetSelectedGameObject(buttons[mainMenuIdx].gameObject);

            Debug.Log("������ idx = " + mainMenuIdx);
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            //��ư�� ���� onClick �̺�Ʈ�� ����� ����� Invoke�� ���
            //buttons[mainMenuIdx].onClick.Invoke();

            //���� �ڵ�� ó���� ����� ���ù��� ���� ����
            switch (mainMenuIdx)
            {
                case 0:
                    OpenSkillMenu(); break;
                case 1:
                    OpenBagMenu(); break;
                case 2:
                    SelectPokemonMenu(); break;
                case 3:
                    Run(); break;

            }
        }
    }

    private void OpenSkillMenu()
    {
        mainmenuPanel.SetActive(false);
        skillmenuPanel.SetActive(true);
        ui_state = UI_State.SKillMenu;
        mainMenuIdx = 0;

        for (int i = 0; i < skill_buttons.Length; i++)
        {
            //�÷��̾� ����(���ϸ�)�� ���� ��ų ����ŭ �����ϴ� ���
            if (i < playerUnit.skills.Length)
            {
                skill_buttons[i].gameObject.SetActive(true);

                skill_buttons[i].GetComponentInChildren<Text>().text =
                    playerUnit.skills[i].skillName;
            }
            //������ false
            else
            {
                skill_buttons[i].gameObject.SetActive(false);
            }
        }
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);
    }

    private void HandleSkillMenuInput()
    {
        //�Ʒ� �Է¿� ���� �ڵ忡���� �ε��� ó��
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            skillMenuIdx = (skillMenuIdx + 1) % skill_buttons.Length;
            EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);

            Debug.Log("������ idx = " + skillMenuIdx);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            skillMenuIdx = (skillMenuIdx - 1) % skill_buttons.Length;
            EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);

            Debug.Log("������ idx = " + skillMenuIdx);
        }

        //Enter Ű �Է�
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Skill player_selected = playerUnit.skills[skillMenuIdx];
            //�÷��̾��� ����
            Debug.Log(playerUnit.name + "��" + playerUnit.skills[skillMenuIdx].skillName + "!!");
        }

        //ESC Ű �Է�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //���� �޴� Ų��.
            OpenMainMenu();
        }

    }


    private void OpenBagMenu()
    {
        Debug.Log("���� �޴� ����");
    }

    private void SelectPokemonMenu()
    {
        Debug.Log("���ϸ� �޴� ����");
    }

    private void Run()
    {
        Debug.Log("�������ϴ�~~~~");
    }
}
