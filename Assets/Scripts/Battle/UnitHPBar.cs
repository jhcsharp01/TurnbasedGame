using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnitHPBar : MonoBehaviour
{
    public Slider slider;

    public void SetSliderBar(Unit playerUnit)
    {
        slider.maxValue = playerUnit.maxHP;
        slider.value = playerUnit.currentHP;
    }

    public void UpdateHP(int value)
    {
        StartCoroutine(AnimateBar(value));
    }

    //체력이 Lerp로 줄어들거나 늘어나는 연출
    public IEnumerator AnimateBar(int value)
    {
        float start = slider.value;
        float duration = 0.5f;
        float time = 0.0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            slider.value = Mathf.Lerp(start, value, time / duration);

            yield return null;
        }
        slider.value = value;
    }

}
