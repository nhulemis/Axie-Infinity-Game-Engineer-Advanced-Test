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
    Attack,
    Victory
        
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SkeletonAnimation))]
public abstract class Character : MonoBehaviour
{
    public abstract Team Team { get; }
    protected abstract void DecisionAction();

    [Header("Character Defination")]
    [SerializeField] protected int maxHp;
    [SpineAnimation]
    [SerializeField] protected string idle;
    [SpineAnimation]
    [SerializeField] protected string attack;
    [SpineAnimation]
    [SerializeField] protected string victory;

    [Header("UI View")]
    [SerializeField] Slider hpBar;
    [SerializeField] protected Vector3 offsetPosition;

    private AudioSource audioAtk;
    protected ActionStage oldStage;
    private ActionStage actionStage;
    public ActionStage ActionStage
    {
        get => actionStage;
        set
        {
            oldStage = actionStage;
            actionStage = value;
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
            hp = Mathf.Clamp(value,0,999);
            OnHpChanged((float)HP / maxHp);
            CheckAndActionDie();
        }
    }
    public int MaxHP
    {
        get => maxHp;
        set => maxHp = value;
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
            CallBackService.OnCharacterTargetChanged?.Invoke();
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
    private bool isVisible;
    private bool isAlive;
    private float deltaTime;

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
        CallBackService.OnCharacterHPChanged?.Invoke();
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
            isAlive = false;
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
        CallBackService.OnTeamMemberChanged?.Invoke();
    }

    protected virtual void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        AnimationState = skeletonAnimation.AnimationState;
        audioAtk = GetComponent<AudioSource>();
    }

    public void Init(HexCircle hex)
    {
        HP = maxHp;
        RandomNumber = Random.Range(0, 3);
        CircleOwned = hex;
        CircleOwned.Owner = this;
        transform.position = hex.GetPosition() + offsetPosition;
        actionStage = ActionStage.Idle;
        isAlive = true;
        //StartCoroutine(BehaviouEverySecond());
    }

    //IEnumerator BehaviouEverySecond()
    //{
    //    yield return new WaitForSeconds(2);

    //    while (Application.isPlaying && !GM.IsEndGame)
    //    {
    //        yield return new WaitUntil(()=>GM.IsPauseGame == false);
    //        DecisionAction();
    //        yield return new WaitForSeconds(1);
    //    }
    //}

    protected virtual void Update()
    {

    }

    public void CalcDamage()
    {
        if (Target == null)
            return;

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
        //Debug.Log("dmg: " + Damage);
    }

    protected virtual void FixedUpdate()
    {
        if (GM.IsEndGame || GM.IsPauseGame)
        {
            return;
        }

        deltaTime += Time.fixedDeltaTime;
        if (deltaTime >= 1)
        {
            deltaTime = 0;
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
        ActionStage = ActionStage.Attack;
    }

    public virtual void Action()
    {
        if (oldStage != ActionStage && isVisible) // set change anim if changed stage
        {
            if (ActionStage == ActionStage.Attack)
            {
                SetAnimation(attack);
            }
            else
            {
                SetAnimation(idle);
            }
        }

        if (ActionStage == ActionStage.Attack)
        {
            Target.TakeDamaged(this.Damage);
            audioAtk.Play();
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

    public bool IsAlive()
    {
        return isAlive;
    }

    public void VictoryPose()
    {
        SetAnimation(victory);
    }

    protected void SetAnimation(string animName)
    {
        AnimationState.SetAnimation(0, animName, true);
    }

    public void OnPointerDown()
    {
        GM.CamCtrl.SetFov();
        var popup = GM.UI.OpenPopup<PopupInfo>();
        popup.FillData(this);
    }
}
