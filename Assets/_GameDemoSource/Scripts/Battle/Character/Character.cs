using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public enum Team
{
    Attack,
    Defense
}

public abstract class Character : MonoBehaviour
{
    public abstract Team Team { get; }

    [Header("Character Defination")]
    [SerializeField] protected int MaxHp;
    [SerializeField] SkeletonAnimation skeletonAnimation;
    [SerializeField] AnimationReferenceAsset[] animations;

    private int hp;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            OnHpChanged(HP / MaxHp);
        }
    }

    protected int rankdomNumber;

    [Header("UI View")]
    [SerializeField] Slider hpBar;

    public void OnHpChanged(float value)
    {
        Color[] hpColor = new Color[] { Color.red, Color.yellow, Color.green };
        int hpPercent = (int)(10f * value);

        hpBar.DOValue(value, 0.5f);
        Debug.Log(hpPercent);
        var hpFillter = hpBar.fillRect.GetComponent<Image>();
        if (hpPercent > 6)
        {
            hpFillter.color = hpColor[2];
        }
        else if (hpPercent > 3)
        {
            hpFillter.color = hpColor[1];
        }
        else
        {
            hpFillter.color = hpColor[0];
        }
    }

    protected virtual void Start()
    {
        HP = MaxHp;
        rankdomNumber = Random.Range(0, 3);

    }

    int animIndex = 0;
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animIndex++;
            skeletonAnimation.AnimationState.SetAnimation(0, animations[animIndex], true);
        }
    }
}
