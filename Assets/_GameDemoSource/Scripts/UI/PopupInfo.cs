using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfo : Popup
{
    [SerializeField]
    RectTransform content;

    [SerializeField]
    Slider hpbar;
    [SerializeField]
    Slider randombar;
    [SerializeField]
    Slider damage;

    [SerializeField]
    TextMeshProUGUI Title, lbHP, lbHPMax, lbRandom, lbDamage;
    [SerializeField]
    RectTransform mark;

    Character character;

    private void OnEnable()
    {
        CallBackService.OnCharacterHPChanged += OnHpChanged;
        CallBackService.OnCharacterTargetChanged += OnTargetChanged;
    }

    private void OnDisable()
    {
        CallBackService.OnCharacterHPChanged -= OnHpChanged;
        CallBackService.OnCharacterTargetChanged -= OnTargetChanged;
    }

    private void OnHpChanged()
    {
        if (!character.IsAlive()) return;

        float value = (float)character.HP / character.MaxHP;
        hpbar.DOValue(value, 0.7f);

        int curHP = Int32.Parse(lbHP.text);

        lbHP.DOCounter(curHP, character.HP, 0.5f);
    }

    private void OnTargetChanged()
    {
        if (!character.IsAlive()) return;

        if (character.Target == null)
        {
            return;
        }
        float value = (float)character.Damage / 5;
        damage.DOValue(value, 0.7f);
    }

    public void FillData(Character charInfo)
    {
        character = charInfo;

        Title.text = character.Team == Team.Defense ? "Denfender" : "Attacker";
        lbHPMax.text = $"/{character.MaxHP}";

        OnHpChanged();
        OnTargetChanged();

        randombar.DOValue(character.RandomNumber, 0.7f);
        lbRandom.text = $"{character.Damage } / 5";
    }

    private void SetContentPosition(Vector3 charPosition)
    {
        Vector2 myPositionOnScreen = Camera.main.WorldToScreenPoint(charPosition);

        Canvas copyOfMainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        float scaleFactor = copyOfMainCanvas.scaleFactor;

        Vector2 offsetScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        Vector2 newPosition = new Vector2(myPositionOnScreen.x / scaleFactor, myPositionOnScreen.y / scaleFactor) - offsetScreen;

        mark.anchoredPosition = newPosition + new Vector2(0,mark.sizeDelta.y/2);

        if (newPosition.y > 0 && newPosition.x < 0)
        {
            newPosition += new Vector2(content.sizeDelta.x / 2, -content.sizeDelta.y / 2) * 1.5f;
        }
        else if (newPosition.y >= 0 && newPosition.x >= 0)
        {
            newPosition += new Vector2(-content.sizeDelta.x / 2, -content.sizeDelta.y / 2) * 1.5f;
        }
        else if (newPosition.y < 0 && newPosition.x < 0)
        {
            newPosition += content.sizeDelta / 2 * 1.5f;
        }
        else if (newPosition.y <= 0 && newPosition.x > 0)
        {
            newPosition += new Vector2(-content.sizeDelta.x / 2, content.sizeDelta.y / 2) * 1.5f;
        }
        else
        {
            newPosition += new Vector2(content.sizeDelta.x / 2, -content.sizeDelta.y / 2) * 1.5f;
        }

        content.anchoredPosition = newPosition;
    }

    public void LateUpdate()
    {
        if (character != null && character.IsAlive())
        {
            SetContentPosition(character.GetPosition());
        }
        else if (!character.IsAlive())
        {
            ClosePopup();
        }
    }
}
