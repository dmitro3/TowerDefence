using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class EnemySpawner : MonoBehaviour
{
    #region singleton
    public static EnemySpawner Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        LeanTween.alpha(WavePanel, 0, 0.01f);
    }
    #endregion

    public List<WavesList> TutorialWaves = new List<WavesList>();
    public List<WavesList> Waves = new List<WavesList>();
    public List<Transform> SpawnPoints = new List<Transform>();
    public int WaveCount;
    public int WaveID;
    public LeanGameObjectPool GreenPool;
    public LeanGameObjectPool FlyingPool;
    public LeanGameObjectPool BossPool;
    public float timer;
    public float CountDown;

    [SerializeField] GameObject WavePanel;
    [SerializeField] TMPro.TMP_Text WaveText;
    [SerializeField] TMPro.TMP_Text TimerText;
    [SerializeField] TMPro.TMP_Text TutText;
    [SerializeField] UnityEngine.UI.Image TutPic;
    [SerializeField] List<string> Tutorial = new List<string>();
    [SerializeField] List<Sprite> TutPics = new List<Sprite>();
    

    Coroutine TutCO;
    Coroutine BannerCO;

    private void OnEnable()
    {
        TutorialDone = false;
        TutCO = StartCoroutine(StartTutorialWaves());
    }
    private void OnDisable()
    {
        TutorialDone = false;
        if (TutCO != null)
        {
            StopCoroutine(TutCO);
        }
        if (BannerCO != null)
        {
            StopCoroutine(BannerCO);
        }
        GreenPool.DespawnAll();
        FlyingPool.DespawnAll();
        BossPool.DespawnAll();
        LeanTween.alpha(WavePanel, 0, 0.01f);
    }
    IEnumerator StartTutorialWaves()
    {
        for (int i = 0; i < TutorialWaves.Count; i++)
        {
            
            for (int j = 0; j < TutorialWaves[i].greenCount; j++)
            {
                int SpawnPointID = Random.Range(0, SpawnPoints.Count);
                GreenPool.Spawn(SpawnPoints[SpawnPointID]);
                
            }
            for (int j = 0; j < TutorialWaves[i].FlyingCount; j++)
            {
                int SpawnPointID = Random.Range(0, SpawnPoints.Count);
                FlyingPool.Spawn(SpawnPoints[SpawnPointID]);
                
            }
            for (int j = 0; j < TutorialWaves[i].bossCount; j++)
            {
                int SpawnPointID = Random.Range(0, SpawnPoints.Count);
                BossPool.Spawn(SpawnPoints[SpawnPointID]);
                
            }
            
            CountDown = TutorialWaves[i].NextWaveTimer;
            TutText.text = Tutorial[i];
            TutPic.sprite = TutPics[i];
            yield return new WaitForSeconds(TutorialWaves[i].NextWaveTimer);
        }
        TutText.gameObject.SetActive(false);
        TutorialDone = true;
    }

    IEnumerator ShowWaveNum(string data)
    {
        WaveText.text = data;
        WavePanel.SetActive(true);
        LeanTween.alpha(WavePanel, 1, 0.5f);
        yield return new WaitForSeconds(2.5f);
        LeanTween.alpha(WavePanel, 0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        WavePanel.SetActive(false);
    }

    public bool TutorialDone;
    private void Update()
    {
       
        TimerText.text = (CountDown).ToString("F0");
        CountDown -= Time.deltaTime;
        if (CountDown <= 3 && BannerCO == null)
        {
            string data = "Next Wave Coming!";
            BannerCO = StartCoroutine(ShowWaveNum(data));
        }
        if (!TutorialDone) return;

       
        if(CountDown <= 0)
        {
            WaveCount++;
            SelectWaveID();
            if (WaveID != -1)
            {      
                CountDown =  Waves[WaveID].NextWaveTimer;
            }
            else
            {  
                CountDown = 15;
                Debug.Log("Free round now");
            }
        }
    }

    public void SelectWaveID()
    {
        int WaveType = WaveCount % 10;
        if ((WaveType >= 0 && WaveType <= 3) || WaveType == 7)
        {
            WaveID = Random.Range(0, 12);
        }
        else if (WaveType >= 4 || WaveType <= 6 || WaveType == 8)
        {
            WaveID = Random.Range(12, 25);
        }
        else if (WaveType == 9)
        {
            WaveID = Random.Range(25, 30);
        }
        SpawnNormalWave(WaveID);
    }
    public void SpawnNormalWave(int waveID)
    {
        if (waveID == -1) return;
        for (int i = 0; i < Waves[waveID].greenCount; i++)
        {
            int SpawnPointID = Random.Range(0, SpawnPoints.Count);
            GreenPool.Spawn(SpawnPoints[SpawnPointID]);
        }
        for (int i = 0; i < Waves[waveID].FlyingCount; i++)
        {
            int SpawnPointID = Random.Range(0, SpawnPoints.Count);
            FlyingPool.Spawn(SpawnPoints[SpawnPointID]);
        }
        for (int i = 0; i < Waves[waveID].bossCount; i++)
        {
            int SpawnPointID = Random.Range(0, SpawnPoints.Count);
            BossPool.Spawn(SpawnPoints[SpawnPointID]);
        }
    }
}

[System.Serializable]
public class WavesList
{
    public int FlyingCount;
    public int greenCount;
    public int bossCount;
    public int NextWaveTimer = 20;
}
