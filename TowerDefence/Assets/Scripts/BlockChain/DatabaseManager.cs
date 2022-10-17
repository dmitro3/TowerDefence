using MoralisUnity;
using MoralisUnity.Platform.Queries;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    #region Singleton
    public static DatabaseManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    #endregion



    public static LocalData data = new LocalData();

    public List<MetaFunNFTLocal> allMetaDataServer = new List<MetaFunNFTLocal>();
    public LocalData GetLocalData()
    {

        return data;
    }


    public async void updateProfile(int dataType, bool createnew = false)
    {
        if (createnew)
        {
            //data.name = "Player" + UnityEngine.Random.Range(0, 99999).ToString();
        }
        // if (UIManager.insta) UIManager.insta.UpdatePlayerUIData(true);
        MoralisQuery<Userdata> monster = await Moralis.Query<Userdata>();
        monster = monster.WhereEqualTo("userid", SingletonDataManager.useruniqid);
        IEnumerable<Userdata> result = await monster.FindAsync();
        foreach (Userdata mon in result)
        {
            mon.userdata = JsonConvert.SerializeObject(data);
            await mon.SaveAsync();
            Debug.Log("UpdateUserDatabase");
            if (UIManager.Instance)
            {
                //UIManager.Instance.UpdatePlayerUIData(true, data);
            }
            if (UIManager.Instance)
            {
                UIManager.Instance.UpdatePlayerUIData(data);
            }
            break;
        }

        /*
                JSONObject a = new JSONObject();
                JSONObject b = new JSONObject();
                JSONObject c = new JSONObject();
                //JSONObject d = new JSONObject();
                string url = ConstantManager.getProfile_api + PlayerPrefs.GetString("Account", "test").ToLower();
                switch (dataType)
                {
                    case 0:
                        if (!createnew) url += "?updateMask.fieldPaths=userdata";


                        c.AddField("stringValue", Newtonsoft.Json.JsonConvert.SerializeObject(data));
                        b.AddField("userdata", c);
                        break;
                   *//* case 3:
                        if (!createnew) url += "?updateMask.fieldPaths=gamedata";
                        c.AddField("stringValue", PlayerPrefs.GetString("data"));
                        b.AddField("gamedata", c);
                        break;*//*
                }

                WWWForm form = new WWWForm();

                Debug.Log("TEST updateProfile");

                // Serialize body as a Json string
                //string requestBodyString = "";



                a.AddField("fields", b);

                Debug.Log(a.Print(true));

                // Convert Json body string into a byte array
                byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(a.Print());

                using (UnityWebRequest www = UnityWebRequest.Put(url, requestBodyData))
                {
                    www.method = "PATCH";

                    // Set request headers i.e. conent type, authorization etc
                    //www.SetRequestHeader("Content-Type", "application/json");
                    www.SetRequestHeader("Content-length", (requestBodyData.Length.ToString()));
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        //JSONObject obj = new JSONObject(www.downloadHandler.text);
                        Debug.Log(www.downloadHandler.text);
                        //Debug.Log(obj.GetField("fields").GetField("musedata").GetField("stringValue").stringValue);
                        if (UIManager.Instance)
                        {
                            UIManager.Instance.UpdatePlayerUIData(data);
                        }
                    }
                }*/
    }
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] GameObject MainPanel;
    [SerializeField] TMPro.TMP_Text coinCount;
    public async void CheckProfile()
    {
        MoralisQuery<Userdata> monster = await Moralis.Query<Userdata>();
        monster = monster.WhereEqualTo("userid", SingletonDataManager.useruniqid);
        IEnumerable<Userdata> result = await monster.FindAsync();



        foreach (Userdata mon in result)
        {
            Debug.Log("My username " + mon.userid + " and  data " + mon.userdata);
            //userData = JsonConvert.DeserializeObject<localuserData>(mon.userdata);
            data = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalData>(mon.userdata);

            break;
        }

        if (data != null)
        {
            LoadingPanel.SetActive(false);
            MainPanel.SetActive(true);

            coinCount.text = data.coins.ToString();

        }
        if (data != null && GunDemoScript.Instance)
        {
            GunDemoScript.Instance.SelectGun(data.selectedTheme);
        }


        /* string url = ConstantManager.getProfile_api + PlayerPrefs.GetString("Account", "test").ToLower();

         using (UnityWebRequest www = UnityWebRequest.Get(url))
         {
             //www.method = "PATCH";

             // Set request headers i.e. conent type, authorization etc
             //www.SetRequestHeader("Content-Type", "application/json");
             // www.SetRequestHeader("Content-length", (requestBodyData.Length.ToString()));
             yield return www.SendWebRequest();

             if (www.result != UnityWebRequest.Result.Success)
             {
                 Debug.Log("Profile not found " + www.downloadHandler.text);
                 //Debug.Log(www.error);
                 Debug.Log(www.downloadHandler.text);

                // StartCoroutine(updateProfile(0, true));
                updateProfile(0, true);
             }
             else
             {

                 JSONObject obj = new JSONObject(www.downloadHandler.text);
                 Debug.Log(obj);
                 //Debug.Log("CheckProfile " + www.downloadHandler.text);
                 data = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalData>(obj.GetField("fields").GetField("userdata").GetField("stringValue").stringValue);
                 if(data != null)
                 {
                     LoadingPanel.SetActive(false);
                     MainPanel.SetActive(true);

                     coinCount.text = data.coins.ToString();

                 }
                 if(data != null && GunDemoScript.Instance)
                 {
                     GunDemoScript.Instance.SelectGun(data.selectedTheme);
                 }

                 if (data.transactionsInformation != null && data.transactionsInformation.Count > 0)
                 {
                     for (int i = 0; i < data.transactionsInformation.Count; i++)
                     {
                         if (data.transactionsInformation[i].transactionStatus.Equals("pending"))
                         {
                             Debug.Log("Pending Test 1");
                             BlockChainManager.CheckTransactionStatusWithTransID(data.transactionsInformation[i].transactionId,0);
                         }
                     }
                 }

             }
         }*/
    }









    public void GetData()
    {
        CheckProfile();
        //ConvertEpochToDatatime(1659504437);
    }

    public void UpdateData(LocalData localData)
    {
        data = localData;
        updateProfile(0);
    }
    async public void UpdateSpinData()
    {
        data = GetLocalData();
        updateProfile(0);
    }

    /*  public DateTime ConvertEpochToDatatime(long epochSeconds) {
          DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epochSeconds);
          DateTime dateTime = dateTimeOffset.DateTime;

          return dateTime;
      }
  */
    async public Task<long> GetCurrentTime()
    {

        string result = await BlockChainManager.Instance.CheckTimeStatus();

        long currentEpoch;
        if (!string.IsNullOrEmpty(result))
        {
            currentEpoch = long.Parse(result);
        }
        else
        {
            currentEpoch = 1659504437;
        }
        // Get Current EPOCH Time
        // DateTime currentTime= ConvertEpochToDatatime(currentEpoch);
        return currentEpoch;
    }

    public void AddTransaction(string TransId, string status, int _shopId)
    {
        TranscationInfo info = new TranscationInfo(TransId, status);
        switch (_shopId)
        {
            case 0:
                {
                    info.coinAmount = 1000;
                    break;
                }
            case 1:
                {
                    info.coinAmount = 2000;
                    break;
                }
            case 2:
                {
                    info.coinAmount = 4000;
                    break;
                }
            case 3:
                {
                    info.coinAmount = 8000;
                    break;
                }
        }

        data.transactionsInformation.Add(info);
        UpdateData(data);
    }
    public void ChangeTransactionStatus(string transID, string txConfirmed)
    {
        Debug.Log("Changing Database " + transID + " " + txConfirmed);
        TranscationInfo trans_info = data.transactionsInformation.Find(x => x.transactionId == transID);
        if (trans_info != null)
        {
            int index = data.transactionsInformation.IndexOf(trans_info);
            trans_info.transactionStatus = txConfirmed;
            data.transactionsInformation[index] = trans_info;
            if (txConfirmed.Equals("success"))
            {
                data.coins += trans_info.coinAmount;
                data.transactionsInformation.RemoveAt(index);
            }
            UIManager.Instance.ShowInfoMsg(trans_info.coinAmount + "Coins Purchased");

            UpdateData(data);

            if (UIManager.Instance)
            {
                UIManager.Instance.UpdatePlayerUIData(data);
            }
        }


    }



}
[System.Serializable]
public class LocalData
{

    public int highscore = 0;
    public int selectedTheme = 0;
    public int coins;
    public int upgraded_hp = 0;
    public float upgraded_damage = 1;
    public int upgraded_ammocount = 0;
    public string tokens = "0";
    public List<TranscationInfo> transactionsInformation = new List<TranscationInfo>();


    public LocalData()
    {
        highscore = 0;
        coins = 0;
        selectedTheme = 0;
        tokens = "0";
        upgraded_hp = 0;
        upgraded_damage = 1;
        upgraded_ammocount = 0;
        transactionsInformation = new List<TranscationInfo>();
    }

}



[System.Serializable]
public class TranscationInfo
{
    public string transactionId;
    public string transactionStatus;
    public int coinAmount;
    public TranscationInfo(string Id, string status)
    {
        transactionId = Id;
        transactionStatus = status;
    }
}