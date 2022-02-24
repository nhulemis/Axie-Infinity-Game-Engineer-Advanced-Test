using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraController : MonoBehaviour
{
    HexCircle target;
    [SerializeField] Color targetColor;
    [SerializeField] LayerMask targetRaycast;

    [Header("Free Camera")]
    [Tooltip("enable it will auto move when game is not start")] [SerializeField] bool cameraAnim;
    [SerializeField] Vector3 topLeft, topRight, bottomLeft, bottomRight;
    Sequence sequenceFreeCam;

    [Header("Camera Zoom")]
    [Range(10, 50)]
    [SerializeField] float zoomSpeed = 50;
    [SerializeField] float minZoomClamp;
    [SerializeField] float maxZoomClamp;
    float zoomAmount;


    // Start is called before the first frame update
    void Start()
    {
        if (cameraAnim)
        {
            FreeCam();
        }
    }

    private void OnEnable()
    {
        CallBackService.OnStartGame += StartGame;
    }

    public void OnDisable()
    {
        CallBackService.OnStartGame -= StartGame;
    }

    private void StartGame(Vector3 middleHexPosition)
    {
        StopFreeCam();

        var finalPos = new Vector3(middleHexPosition.x, middleHexPosition.y, transform.position.z);
        SetPosition(finalPos);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.DOMove(pos, 0.5f);
    }

    public void StopFreeCam()
    {
        sequenceFreeCam.Kill();
    }

    public void FreeCam()
    {
        sequenceFreeCam = DOTween.Sequence().Append(
                transform.DOMove(topLeft, 5f))
            .Append(
                transform.DOMove(bottomRight, 5f)
            ).Append(
                transform.DOMove(bottomLeft, 5f))
            .Append(
                transform.DOMove(topRight, 5f)
            ).SetLoops(-1);
        sequenceFreeCam.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, transform.forward, 50f, targetRaycast);
        if (hit.collider != null)
        {
            var hex = hit.collider.GetComponent<HexCircle>();
            if (target != hex)
            {
                target = hex;
                target.ClearBlur(targetColor);
            }
        }

        MouseWheeling();
    }

    void MouseWheeling()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            float fov = Camera.main.fieldOfView;
            float fovClamp = Mathf.Clamp(fov + wheel, minZoomClamp, maxZoomClamp);
            Camera.main.fieldOfView = fovClamp;
            
            //Refresh blur
            target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.forward * 50);
    }
}
