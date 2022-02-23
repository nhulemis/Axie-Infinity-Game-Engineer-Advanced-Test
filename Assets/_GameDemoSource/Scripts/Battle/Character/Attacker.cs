using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Attacker : Character
{
    public override Team Team => Team.Attack;

    [Header("attacker anims")]
    [SpineAnimation]
    [SerializeField] string move;


    HexCircle moveTo;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void DecisionAction()
    {
        if (CircleOwned == null) return;


        if (Target != null)
        {
            if (!Target.IsAlive())
            {
                Target = null;
            }
        }

        if (Target == null)
        {
            PickAction();
        }

        CallBackService.OnAddActor(this);
    }

    public void PickAction()
    {
        var circlesAround = CircleOwned.GetNeighbor();
        var neighbors = circlesAround.Select(c => c.Owner).Where(c => c != null && c.Team == Team.Defense).ToList();
        if (neighbors.Count > 0) // attack
        {
            var pickOneEnemy = Random.Range(0, neighbors.Count);
            Attack(neighbors[pickOneEnemy]);
        }
        else // Move Or Idle
        {
            var enemy = GetNearestEnemy();

            if (enemy == null)
            {
                ActionStage = ActionStage.Idle;
            }
            else
            {
                float minDistance = 999999f;
                bool isFoundWay = false;

                foreach (var c in circlesAround)
                {
                    var distance = enemy.DistanceWith(c.GetPosition());
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        moveTo = c;
                        isFoundWay = true;
                    }
                }
                if (isFoundWay && moveTo.Owner == null)
                {
                    moveTo.Owner = this;
                    ActionStage = ActionStage.Move;
                }
                else
                {
                    ActionStage = ActionStage.Idle;
                }
            }
        }
    }

    Character GetNearestEnemy()
    {
        List<Character> enemies = new List<Character>() ;
        int radius = 1;
        do
        {
            radius++;
            enemies = GM.BattleArea.GetDefendersAround(CircleOwned.PointIndex, radius);
        } while (enemies.Count == 0 && radius < 11);

        Character tmp = null;
        float minDistance = 999999f;
        foreach (var e in enemies)
        {
            float distance = DistanceWith(e);
            if (distance < minDistance)
            {
                minDistance = distance;
                tmp = e;
            }
        }
        return tmp;
    }

    public override void Action()
    {
        base.Action();
        if (ActionStage == ActionStage.Move)
        {
            transform.DOMove(moveTo.GetPosition() + offsetPosition, 0.8f).onComplete = OnMoveComplete;
            SetAnimation(move);
        }
    }
    void OnMoveComplete()
    {
        CircleOwned.Owner = null;

        CircleOwned = moveTo;
        ActionStage = ActionStage.Idle;
    }
}
