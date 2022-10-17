using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glue : MonoBehaviour
{
    public float enableWindow = 0.5f;
    private void OnEnable()
    {
        LeanTween.scale(gameObject, Vector3.one * 5, 0.3f).setEaseOutQuad();
        Invoke(nameof(EnableGlue), enableWindow);
    }

    void EnableGlue()
    {
        float lifetime = PlayerController.instance.stuckDuration;

        lifetime = Mathf.Clamp(lifetime, 0.25f, 2.5f);
        LeanTween.scale(gameObject, Vector3.zero, 0.3f).setDelay(lifetime - 0.3f).setEaseOutQuad();
        Destroy(this.gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.isStuck = true;
            PlayerController.instance._CanBeStuckAgain = false;
        }
    }
    private void OnDisable()
    {
        PlayerController.instance.isStuck = false;
    }
}
