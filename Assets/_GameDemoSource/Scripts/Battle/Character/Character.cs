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
    protected abstract void DecisionAction();

    [Header("Character Defination")]
    [SerializeField] protected int MaxHp;
    [SpineAnimation]
    [SerializeField] string idle;
    [SpineAnimation]
    [SerializeField] string attack;

    [Header("UI View")]
    [SerializeField] Slider hpBar;
    [SerializeField] Vector3 offsetPosition;


    private SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState AnimationState { get; private set; }

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

    private HexCircle owned;
    public HexCircle Owned
    {
        get => owned;
        set
        {
            if (owned == value)
                return;
            if (value != null)
            {
                owned = value;
                owned.Owner = this;
            }
            else
            {
                owned.Owner = null;
                owned = null;
            }
        }
    }

    private float deltaTime;
    // public bool IsReadyToAction { get; set; }

    public void OnHpChanged(float value)
    {
        Color[] hpColor = new Color[] { Color.red, Color.yellow, Color.green };
        int hpPercent = (int)(10f * value);

        hpBar.DOValue(value, 0.5f);
       // Debug.Log(hpPercent);
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
        
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        AnimationState = skeletonAnimation.AnimationState;
    }

    public void Init(HexCircle hex)
    {
        HP = MaxHp;
        RandomNumber = Random.Range(0, 3);
        Owned = hex;
        transform.position = hex.GetPosition() + offsetPosition;
    }

    protected virtual void Update()
    {
        
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

    protected virtual void FixedUpdate()
    {

        deltaTime += Time.fixedDeltaTime;
        if (deltaTime >= 1)
        {
            deltaTime -= 1;
            //IsReadyToAction = true;
            DecisionAction();
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float DistanceWith(Character other)
    {
        return Vector3.Distance(GetPosition(), other.GetPosition());
    }

    public float DistanceWith(Vector3 other)
    {
        return Vector3.Distance(GetPosition(), other);
    }

    public void Attack(Character target)
    {
        Target = target;
        AnimationState.SetAnimation(0, attack, false);
    }

    public void Idle()
    {
        AnimationState.SetAnimation(0, idle, true);
    }
}
