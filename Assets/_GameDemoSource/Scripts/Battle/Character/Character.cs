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

public enum ActionStage
{
    Idle,
    Move,
    Attack
}

public abstract class Character : MonoBehaviour
{
    public abstract Team Team { get; }
    protected abstract void DecisionAction();

    [Header("Character Defination")]
    [SerializeField] protected int MaxHp;
    [SpineAnimation]
    [SerializeField] protected string idle;
    [SpineAnimation]
    [SerializeField] protected string attack;
    [SpineAnimation]
    [SerializeField] protected string victory;

    [Header("UI View")]
    [SerializeField] Slider hpBar;
    [SerializeField] protected Vector3 offsetPosition;

    protected ActionStage oldStage;
    private ActionStage actionStage;
    public ActionStage ActionStage
    {
        get => actionStage;
        set
        {
            oldStage = actionStage;
            actionStage = value;
            Action();
        }
    }

    private SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState AnimationState { get; private set; }

    private int hp;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            OnHpChanged((float)HP / MaxHp);
            CheckAndActionDie();
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
    public HexCircle CircleOwned
    {
        get => owned;
        set
        {
            if (owned == value)
                return;
            owned = value;
        }
    }

    private float deltaTime;
    private bool isVisible;
    // public bool IsReadyToAction { get; set; }
    Coroutine coroutine;
    public void OnHpChanged(float value)
    {
        Color[] hpColor = new Color[] { Color.red, Color.yellow, Color.green };
        int hpPercent = (int)(10f * value);

        hpBar.DOValue(value, 0.5f);

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

    public void CheckAndActionDie()
    {
        if (HP <= 0 && gameObject.activeSelf)
        {
            if (CircleOwned != null)
            {
                CircleOwned.Owner = null;
                // CircleOwned = null;
            }
            StartCoroutine(DieBehavior());
        }
    }

    IEnumerator DieBehavior()
    {
        yield return null;
        skeletonAnimation.skeleton.A = 0;

        for (int i = 0; i < 5; i++)
        {
            skeletonAnimation.skeleton.A = i % 2;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
        GM.BattleArea.CheckGameOver();
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
        CircleOwned = hex;
        CircleOwned.Owner = this;
        transform.position = hex.GetPosition() + offsetPosition;
        actionStage = ActionStage.Idle;

        coroutine = StartCoroutine(BehaviouEverySecond());
    }

    IEnumerator BehaviouEverySecond()
    {
        yield return new WaitForSeconds(2);

        while (Application.isPlaying && !GM.IsEndGame)
        {
            DecisionAction();
            yield return new WaitForSeconds(1);
        }
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
        //deltaTime += Time.fixedDeltaTime;
        //if (deltaTime >= 1)
        //{
        //    deltaTime -= 1;
        //    //IsReadyToAction = true;
        //    DecisionAction();
        //}
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
        ActionStage = ActionStage.Attack;
    }

    public virtual void Action()
    {
        if (oldStage != ActionStage && isVisible) // set change anim if changed stage
        {
            if (ActionStage == ActionStage.Attack)
            {
                AnimationState.SetAnimation(0, attack, true);
            }
            else
            {
                AnimationState.SetAnimation(0, idle, true);
            }
        }

        if (ActionStage == ActionStage.Attack)
        {
            Target.TakeDamaged(this.Damage);
        }
    }

    private void TakeDamaged(int damage)
    {
       // Debug.Log(damage);
        HP -= damage;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void VictoryPose()
    {
        AnimationState.SetAnimation(0, victory, true);
    }
}
