
using MoralisUnity;
using MoralisUnity.Platform.Queries;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SingletonDataManager : MonoBehaviour
{
    public static SingletonDataManager insta;


    public static bool initData;


    public static string username;
    public static string userethAdd;
    public static string useruniqid;
    public static string userTokenBalance;
    public static string userMainBalance;
    [SerializeField]
    public static List<MetaJungleNFTLocal> metanftlocalData = new List<MetaJungleNFTLocal>();
    //public List<MetaJungleNFTLocal> metanftlocalData2 = new List<MetaJungleNFTLocal>();
    public static List<MyMetadataNFT> myNFTData = new List<MyMetadataNFT>();
    
    

    public const string postfixMetaUrl = ".ipfs.nftstorage.link/metadata.json";
    public static string nftmetaCDI;
    // public static string tokenID;

    //bool initData = false;


    public string jsonData;

    private void Awake()
    {
        if (insta == null)
        {
            insta = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }

    private void OnEnable()
    {


#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
 /* Debug.unityLogger.logEnabled = false;
          Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
        Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);
    Application.stackTraceLogType = StackTraceLogType.None;*/
#endif
    }
    byte[] data;
    private void Start()
    {

        // jigar
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
           // if (QualitySettings.vSyncCount > 0)
           //     Application.targetFrameRate = 60;
            //else
            //    Application.targetFrameRate = -1;
        }

        Application.targetFrameRate = 40;


        //getUserDataonStart();

        //JsonReader jr = JSON. JsonConvert.DeserializeObject(jsonData);


        //long tokenId = MoralisTools.ConvertStringToLong(jsonData);
        // Debug.Log(tokenId);
        // Invoke("RequestMic", 2);

        

    }

   
    public async void getUserDataonStart()
    {
        getNFTDetailsData();

      

    }

    public async void submitName(string _name)
    {
       

    }

    public async void UpdateUserDatabase()
    {
       
    }

    public async void CheckUserData()
    {
        MoralisQuery<Userdata> monster = await Moralis.Query<Userdata>();
        monster = monster.WhereEqualTo("userid", useruniqid);
        IEnumerable<Userdata> result = await monster.FindAsync();

        var datathere = false;

        foreach (Userdata mon in result)
        {
            Debug.Log("My username " + mon.userid + " and  data " + mon.userdata);
            DatabaseManager.data = JsonConvert.DeserializeObject<LocalData>(mon.userdata);
            datathere = true;

            //UpdateUserDatabase(_uniqid);
            break;
        }

        if (!datathere)
        {
            Debug.Log("addNewUserData");
            addNewUserData();
        }
        else
        {
            Debug.Log("userDataAlreadyThere");
            if (initData)
            {
                initData = false;
                //UnityEngine.SceneManagement.SceneManager.LoadScene("MetaJump");
            }
        }
    }

    public async void addNewUserData()
    {

        Userdata monster = Moralis.GetClient().Create<Userdata>();
        monster.userid = useruniqid;
        //DatabaseManager.data.name = "Player" + UnityEngine.Random.Range(0, 99999).ToString();
        monster.userdata = JsonConvert.SerializeObject(DatabaseManager.data);
        monster.gamedata = "gamedataNeedToAdd";
        var result = await monster.SaveAsync();

        if (result)
        {
            Debug.Log("CheckUserData Again");
            CheckUserData();
        }
    }


    public async void getNFTDetailsData()
    {

       
        Debug.Log("DAta is " + JsonConvert.SerializeObject(metanftlocalData));
        GetAllNFTImg();
        // return false;
    }

    //public List<Texture> nftImg = new List<Texture>();
    public void GetAllNFTImg()
    {
        for (int i = 0; i < metanftlocalData.Count; i++)
        {
            StartCoroutine(GetTexture(metanftlocalData[i].imageurl, i));
        }

    }

    IEnumerator GetTexture(string _url, int _index)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            metanftlocalData[_index].imageTexture = (((DownloadHandlerTexture)www.downloadHandler).texture);
        }
    }



   

}
