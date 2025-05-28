using UnityEngine;

//이 게임에서 사용할 타입에 대한 데이터입니다.
public enum Type
{
    노말,불,물,풀,전기,독,고스트
}



[CreateAssetMenu(fileName = "", menuName = "Battle/Unit")]
public class Unit : ScriptableObject
{
   public string unitName;
    public int maxHP;
    public int currentHP;

    //포켓몬 게임 기준 가질 수 있는 타입의 최대 수는 2개
    public Type type1;
    public Type type2;

    public Skill[] skills;
}
