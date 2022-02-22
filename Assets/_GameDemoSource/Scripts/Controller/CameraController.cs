using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    HexCircle target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField] LayerMask targetRaycast;
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
               // target.OnPointerDown();
            }
        }

    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.forward * 50);
    }
}
