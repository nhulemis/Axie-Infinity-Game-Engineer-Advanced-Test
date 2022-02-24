using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "game define", menuName = "new game define", order = 0)]

public class GameDefine : ScriptableObject
{
    [Header("special popup")]
    [SerializeField]
    private List<Popup> specialPopups;

    public T GET<T>() where T : Popup
    {
        var popup = specialPopups.Find(x => x is T);

        return popup as T;
    }


    [Header("Gameplay define")]
    public int teamSizeNormal;
    public int teamSizeExtra;

    [Range(10, 50)]
    public int actionCountInFrame = 20;

    [SerializeField] float startCooldown;

    public float StartCoolDownTime { get => startCooldown; }
}
