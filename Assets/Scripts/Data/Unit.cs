using UnityEngine;

//�� ���ӿ��� ����� Ÿ�Կ� ���� �������Դϴ�.
public enum Type
{
    �븻,��,��,Ǯ,����,��,��Ʈ
}



[CreateAssetMenu(fileName = "", menuName = "Battle/Unit")]
public class Unit : ScriptableObject
{
   public string unitName;
    public int maxHP;
    public int currentHP;

    //���ϸ� ���� ���� ���� �� �ִ� Ÿ���� �ִ� ���� 2��
    public Type type1;
    public Type type2;

    public Skill[] skills;
}
