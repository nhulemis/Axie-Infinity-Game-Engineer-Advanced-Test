using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Character
{
    public override Team Team => Team.Attack;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


    }
    protected override void Update()
    {
        base.Update();
    }
}
