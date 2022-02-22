using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attacker : Character
{
    public override Team Team => Team.Attack;

    [Header("attacker anims")]
    [SpineAnimation]
    [SerializeField] string move;


    Vector3 moveTo;

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
        var circlesAround = Owned.GetNeighbor(1);
        var neighbors = circlesAround.Select(c => c.Owner).Where(c=>c != null && Team == Team.Defense).ToList();

        if (neighbors.Count > 0) // attack
        {
            var pickOneEnemy = Random.Range(0, neighbors.Count);
            Attack(neighbors[pickOneEnemy]);
            return;
        }

        //var enemy = GetNearestEnemy();
        //float minDistance = 999999f;
        //bool isFoundWay = false;

        //var canMoves = circlesAround.Where(c => c.Owner == null);

        //foreach (var c in canMoves)
        //{
        //    var distance = DistanceWith(c.GetPosition());
        //    if (distance < minDistance)
        //    {
        //        minDistance = distance;
        //        moveTo = c.GetPosition();
        //        isFoundWay = true;
        //    }
        //}
        //if (isFoundWay)
        //{
        //    // action Move
        //    AnimationState.SetAnimation(0, move, true);
        //}
        //else
        //{
        //    Idle();
        //}
    }

    Character GetNearestEnemy()
    {
        var enemies = GM.BattleArea.GetEnemies(Team.Defense);

        float minDistance = 999999f;
        Character target = null;
        foreach (var e in enemies)
        {
            var distance = DistanceWith(e);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = e;
            }
        }

        return target;
    }
}
