using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{

    public void OpenPopup()
    {

    }

    public void ClosePopup()
    {
        var animator = GetComponent<Animator>();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            animator.Play("Close");

        StartCoroutine(RunPopupDestroy());
    }

    
    /// <summary>
    /// destroy popup after 0.5s
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunPopupDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
