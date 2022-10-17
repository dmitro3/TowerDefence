using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Camera cam;
  
    private void Start()
    {
        cam = Camera.main;
       
    }
    private void Update()
    {
        transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
