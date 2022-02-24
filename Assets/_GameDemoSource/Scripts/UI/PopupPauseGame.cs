using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPauseGame : Popup
{
    public void OnResumeGame()
    {
        CallBackService.OnResume?.Invoke();
    }

    public void OnGiverUp()
    {
        CallBackService.OnResume?.Invoke();
        CallBackService.OnGiveUp?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
