using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public int EnemiesKilled;
    public int CoinsEarned;

    private void Start()
    {
        LookAtMouse.DoFollow = true;
    }
    public void StartGame()
    {
        gameObject.SetActive(true);
        LookAtMouse.DoFollow = true;
        EnemiesKilled = 0;
        CoinsEarned = 0;

        PlayerController.instance.ResetGame();
        EnemySpawner.Instance.enabled = false;
        EnemySpawner.Instance.enabled = true;
    }

    public void TimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
