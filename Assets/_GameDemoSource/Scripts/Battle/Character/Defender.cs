using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Defender : Character
{
    public override Team Team => Team.Defense;


    protected override void DecisionAction()
    {
        var circlesAround = CircleOwned.GetNeighbor(1);
        var neighbors = circlesAround.Select(c => c.Owner).Where(c => c != null && c.Team == Team.Attack).ToList();

        if (neighbors.Count > 0) // attack
        {
            var pickOneEnemy = Random.Range(0, neighbors.Count);
            Attack(neighbors[pickOneEnemy]);
        }

        CallBackService.OnAddActor(this);
    }

}
