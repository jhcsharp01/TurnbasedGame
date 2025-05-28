using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//배틀에서 사용할 턴에 대한 상태를 enum으로 표현
public enum BattleTurn
{
    Start               //시작
    , Turn1, Turn2       //플레이어 턴, 상대 턴
    , Action1, Action2   //플레이어의 동작, 상대의 동작
    , Win, Lose          // 승 , 패
}

//UI에서의 메뉴 화면에 대한 정보
public enum UI_State
{
    MainMenu, SKillMenu
}


public class BattleSystem : MonoBehaviour
{
    //배틀 시스템이 감지할 배틀의 상태
    //(턴마다 바뀜)
    public BattleTurn state;

    //현재 UI에 대한 상태
    public UI_State ui_state;

    public Text PhaseText;     //맵 가운데에서 턴 페이즈에 대한 텍스트

    public Unit playerUnit;   //플레이어와 상대방에 대한 등록
    public Unit otherUnit;



    public Button[] buttons; //메뉴 버튼
    public Button[] skill_buttons; //스킬에 대한 버튼

    public GameObject mainmenuPanel; //메뉴 온 오프
    public GameObject skillmenuPanel;
    
    //플레이어의 슬라이더 바 연결
    public UnitHPBar player_bar;
    public UnitHPBar other_bar;

    //현재 메뉴들에 대한 인덱스 값
    private int mainMenuIdx = 0;
    private int skillMenuIdx = 0;


    private void Start()
    {
        state = BattleTurn.Start;

        //슬라이더 바 설정
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

    //스킬로 공격
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
        //메인 메뉴에 대한 오픈
        OpenMainMenu();
    }

    public IEnumerator EnemyTurn()
    {
        yield return StartCoroutine(Phase("Enemy's turn !!"));

        //적이 진행할 행동 구현 (적이 가진 1번째 스킬을 사용한다.)
        Debug.Log(otherUnit.unitName + "의" + otherUnit.skills[0].skillName + "!!");

        yield return StartCoroutine(EnemyAttack(otherUnit.skills[0]));
    }

    //상황에 맞게 페이즈 텍스트에 대한 일시적인 표현을 위한 코드
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
        //선택된 UI에 대한 설정 - 현재 선택된 요소를 해제합니다.
        EventSystem.current.SetSelectedGameObject(null);

        //선택 위치를 버튼으로 변경
        EventSystem.current.SetSelectedGameObject(buttons[mainMenuIdx].gameObject);
    }

    //메인 메뉴에 대한 선택
    void HandleMainMenuInput()
    {
        //오른쪽 입력에 대한 코드에서의 인덱스 처리
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mainMenuIdx = (mainMenuIdx + 1) % buttons.Length;
            EventSystem.current.SetSelectedGameObject(buttons[mainMenuIdx].gameObject);

            Debug.Log("현재의 idx = " + mainMenuIdx);
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            //버튼에 각각 onClick 이벤트를 등록한 경우라면 Invoke로 충분
            //buttons[mainMenuIdx].onClick.Invoke();

            //메인 코드로 처리할 경우라면 선택문을 통해 결정
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
            //플레이어 유닛(포켓몬)이 가진 스킬 수만큼 개방하는 기능
            if (i < playerUnit.skills.Length)
            {
                skill_buttons[i].gameObject.SetActive(true);

                skill_buttons[i].GetComponentInChildren<Text>().text =
                    playerUnit.skills[i].skillName;
            }
            //없으면 false
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
        //아래 입력에 대한 코드에서의 인덱스 처리
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            skillMenuIdx = (skillMenuIdx + 1) % skill_buttons.Length;
            EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);

            Debug.Log("현재의 idx = " + skillMenuIdx);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            skillMenuIdx = (skillMenuIdx - 1) % skill_buttons.Length;
            EventSystem.current.SetSelectedGameObject(skill_buttons[skillMenuIdx].gameObject);

            Debug.Log("현재의 idx = " + skillMenuIdx);
        }

        //Enter 키 입력
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Skill player_selected = playerUnit.skills[skillMenuIdx];
            //플레이어의 공격
            Debug.Log(playerUnit.name + "의" + playerUnit.skills[skillMenuIdx].skillName + "!!");

            //공격이 끝난 다음 턴을 넘긴다.
            StartCoroutine(PlayerAttack(playerUnit.skills[skillMenuIdx]));
        }

        //ESC 키 입력
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //메인 메뉴 킨다.
            OpenMainMenu();
        }

    }


    private void OpenBagMenu()
    {
        Debug.Log("가방 메뉴 선택");
    }

    private void SelectPokemonMenu()
    {
        Debug.Log("포켓몬 메뉴 선택");
    }

    private void Run()
    {
        Debug.Log("도망갑니다~~~~");
    }
}
