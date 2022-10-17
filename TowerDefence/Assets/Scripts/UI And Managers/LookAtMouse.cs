using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class LookAtMouse : MonoBehaviour
{
    [SerializeField] Vector2 followRange;
    [SerializeField] Transform player;
    [SerializeField] Camera maincam;
    [SerializeField] CinemachineVirtualCamera VCam;
    public static bool DoFollow = false;

    private void Start()
    {
        maincam = Camera.main;
    }

    private void Update()
    {
        if (DoFollow)
        {
            if (VCam.Follow != this.transform)
            {
                VCam.Follow = this.transform;
            }

            Vector2 mousepos = maincam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetPos = ((Vector2)player.position + mousepos) / 2f;

            targetPos.x = Mathf.Clamp(targetPos.x, -followRange.x + player.position.x, followRange.x + player.position.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -followRange.y + player.position.y, followRange.y + player.position.y);

            this.transform.position = targetPos;
        }
        else
        {
            if (VCam.Follow != player)
            {
                VCam.Follow = player;
            }
        }
    }

   
}