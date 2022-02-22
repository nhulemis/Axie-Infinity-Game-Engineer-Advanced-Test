using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using UnityEngine.Assertions;

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
    [SpineAnimation]
    [SerializeField] string[] animations;

    [Header("UI View")]
    [SerializeField] Slider hpBar;


    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState animationState;

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

    private int rankdomNumber;
    public int RandomNumber
    {
        get => rankdomNumber;
        set => rankdomNumber = value;
    }

    private Character target;
    public Character Target
    {
        get => target;
        set
        {
            if (target == value)
                return;

            target = value;
            CalcDamage();
        }
    }

    public int Damage { get; set; }


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
        RandomNumber = Random.Range(0, 3);
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
    }

    int animIndex = 0;
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animIndex++;
            animationState.SetAnimation(0, animations[animIndex], true);
        }
    }

    public void CalcDamage()
    {
        Assert.IsNotNull(Target, "Target is null");

        int offset = (3 + this.RandomNumber - Target.RandomNumber) % 3;

        switch (offset)
        {
            case 0:
                this.Damage = 4;
                break;
            case 1:
                this.Damage = 5;
                break;
            default:
                this.Damage = 3;
                break;
        }
    }
}