using System;
using System.Collections;
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
    
    //�÷��̾��� �����̴� �� ����
    public UnitHPBar player_bar;
    public UnitHPBar other_bar;

    //���� �޴��鿡 ���� �ε��� ��
    private int mainMenuIdx = 0;
    private int skillMenuIdx = 0;


    private void Start()
    {
        state = BattleTurn.Start;

        //�����̴� �� ����
        player_bar.SetSliderBar(playerUnit);
        other_bar.SetSliderBar(otherUnit);


        StartCoroutine(PlayerTurn());
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

    //��ų�� ����
    public IEnumerator PlayerAttack(Skill skill)
    {
        otherUnit.currentHP -= skill.damage;
        other_bar.UpdateHP(otherUnit.currentHP);

        yield return StartCoroutine(EnemyTurn());
    }

    public IEnumerator EnemyAttack(Skill skill)
    {
        playerUnit.currentHP -= skill.damage;
        player_bar.UpdateHP(playerUnit.currentHP);

        yield return StartCoroutine(PlayerTurn());
    }


    public IEnumerator PlayerTurn()
    {
        yield return StartCoroutine(Phase("Player's turn !!"));
        //���� �޴��� ���� ����
        OpenMainMenu();
    }

    public IEnumerator EnemyTurn()
    {
        yield return StartCoroutine(Phase("Enemy's turn !!"));

        //���� ������ �ൿ ���� (���� ���� 1��° ��ų�� ����Ѵ�.)
        Debug.Log(otherUnit.unitName + "��" + otherUnit.skills[0].skillName + "!!");

        yield return StartCoroutine(EnemyAttack(otherUnit.skills[0]));
    }

    //��Ȳ�� �°� ������ �ؽ�Ʈ�� ���� �Ͻ����� ǥ���� ���� �ڵ�
    public IEnumerator Phase(string message , float duration = 1.5f)
    {
        PhaseText.text = message;
        PhaseText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        PhaseText.gameObject.SetActive(false);
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

            //������ ���� ���� ���� �ѱ��.
            StartCoroutine(PlayerAttack(playerUnit.skills[skillMenuIdx]));
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
