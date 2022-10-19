using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    [Header("GameOverUI")]
    [SerializeField] GameObject GameOverUI;
    [SerializeField] TMP_Text Score;
    [SerializeField] TMP_Text HighScore_Txt;
    [SerializeField] TMP_Text CoinsAward;

    [Header("GamePlay UI")]
    [SerializeField] GameObject GamePlayUI;
    [SerializeField] TMP_Text CurrentScore;
    [SerializeField] GameObject PauseUI;

    [SerializeField] TMP_Text CoinAmount;

    [SerializeField] TMP_Text TokenAmount;


    [SerializeField] GameObject tokenUI;

    [SerializeField] GameObject Walls;

    [SerializeField] bool isInitialized = false;

    public GameObject tokenButton;

    public TMP_Text AmmoCount;
    public TMP_Text HealthCount;
    public TMP_Text CoinCount;

    public TMP_Text scoreTxt;
    public TMP_Text enemTxt;

    public TMP_Text MainBalanceTxt;
    public TMP_Text TokenBalanceTxt;

    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        tokenButton.SetActive(false);

        //UpdatePlayerUIData(DatabaseManager.Instance.GetLocalData());
    }

    private void LateUpdate()
    {
        scoreTxt.text = GameManager.Instance.CoinsEarned.ToString();
        enemTxt.text = GameManager.Instance.EnemiesKilled.ToString();
    }

    public void SetMainBalance()
    {
        MainBalanceTxt.text = "Main Balance : " + SingletonDataManager.userMainBalance;
    }

    public void SetTokenBalance()
    {
        TokenBalanceTxt.text = "Token Balance : " + SingletonDataManager.userTokenBalance;
    }



    public void UpdatePlayerUIData(LocalData data)
    {
        if (data != null)
        {
            CoinCount.text = data.coins.ToString();
            //   TokenAmount.text = data.tokens;
        }
    }



    public TMP_Text txt_information;


    public void DeductCoins(int _no)
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();
        data.coins -= 500 * (_no + 1);
        DatabaseManager.Instance.UpdateData(data);
        UpdatePlayerUIData(data);
    }

    public void ShowNoCoinsPopup()
    {

        MessaeBox.insta.showMsg("Not Enough Coins!", true);

    }
    Coroutine coroutine;
    public void ShowInfoMsg(string info)
    {
        txt_information.transform.parent.gameObject.SetActive(true);

        txt_information.text = info;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(disableTextInfo());
    }
    IEnumerator disableTextInfo()
    {
        yield return new WaitForSecondsRealtime(2f);
        txt_information.transform.parent.gameObject.SetActive(false);
    }


    public void OpenGameOverPanel()
    {
        GameOverUI.SetActive(true);
        //GamePlayUI.SetActive(false);
        Score.text = "Enemies Killed : " + GameManager.Instance.EnemiesKilled.ToString();
        CoinsAward.text = "Coins Earned : " + GameManager.Instance.CoinsEarned.ToString();


        int highscore = DatabaseManager.Instance.GetLocalData().highscore;
        LocalData data = DatabaseManager.Instance.GetLocalData();
        data.coins += GameManager.Instance.CoinsEarned;
        DatabaseManager.Instance.UpdateData(data);
        UpdatePlayerUIData(data);

        if (GameManager.Instance.EnemiesKilled >= 15)
        {
            //tokenUI.SetActive(true);
            tokenButton.SetActive(true);
        }
        else
        {
            tokenButton.SetActive(false);
        }


        /* if (GameManager.Instance.Score > highscore)
         {

             highscore = GameManager.Instance.Score;
             data.highscore = highscore;

         }
         HighScore_Txt.text = "High Score : " + highscore;*/
    }



    public void RedeemToken()
    {
        tokenUI.SetActive(false);
        MessaeBox.insta.showMsg("Token redeem process started", false);
        BlockChainManager.Instance.getDailyToken();
    }


    public void UpdateScore()
    {
        // CurrentScore.text = "Score : " + GameManager.Instance.Score.ToString();
    }
}
