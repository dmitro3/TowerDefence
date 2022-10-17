using Cysharp.Threading.Tasks;
using Defective.JSON;
using MoralisUnity;
using MoralisUnity.Web3Api.Models;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class MoralisManager : MonoBehaviour
{
    #region Singleton
    public static MoralisManager Instance;
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

    public const string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_itemId\",\"type\":\"uint256\"}],\"name\":\"buyCoins\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_tokenId\",\"type\":\"uint256\"},{\"internalType\":\"string\",\"name\":\"_tokenUrl\",\"type\":\"string\"}],\"name\":\"buyNonBurnItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getCurrentTime\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"_result\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_recipient\",\"type\":\"address\"}],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"}]";

    // address of contract
    public const string contract = "0x06B7E570edD6b70f5671Dd269d254c14f7f47Aef";

    public const string abiRandom = "[{\"inputs\":[{\"internalType\":\"uint64\",\"name\":\"subscriptionId\",\"type\":\"uint64\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"have\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"want\",\"type\":\"address\"}],\"name\":\"OnlyCoordinatorCanFulfill\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"requestId\",\"type\":\"uint256\"},{\"internalType\":\"uint256[]\",\"name\":\"randomWords\",\"type\":\"uint256[]\"}],\"name\":\"rawFulfillRandomWords\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"SendRandomNoRequest\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"requestId\",\"type\":\"uint256\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"player\",\"type\":\"address\"}],\"name\":\"GetRandomNo\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"s_requestIdToAddress\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"name\":\"s_result\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    // address of contract
    public const string contractRandom = "0xAA44fa499DfD16b6BB5141b53903D82Fdfc7cEE3";





    float[] coinCost = { 0.025f, 0.050f, 0.075f, 0.1f, 0.050f };
    int[] coinGive = { 20, 50, 100, 200 };



    private int expirationTime;
    private string account;

    [SerializeField] TMP_Text _status;
    [SerializeField] GameObject playBTN;
    [SerializeField] GameObject loginBTN;
    [SerializeField] GameObject gameManagerOBJ;

    //public static string username;
    //public static string userethAdd;
    //public static string useruniqid;
    private bool initData = false;

    public static ChainList ContractChain = ChainList.mumbai;

    //public static decimal userBalance;
    public static float userBalance = 0;
    private void Start()
    {
        //LoginWallet();
        //Debug.Log("Amout " + UnitConversion.Convert.ToWei(0.00159, 18));
        //Debug.Log("Amout " + UnitConversion.Convert.ToWei(35));
    }


    #region MoralisArea

    public async void getUserDataonStart()
    {

        //SingletonDataManager.insta.getNFTDetailsData();
        DatabaseManager.Instance.getNFTDetailsData();

        var user = await Moralis.GetClient().GetCurrentUserAsync();
        if (user == null) return;
        SingletonDataManager.username = user.username;
        SingletonDataManager.userethAdd = user.ethAddress;
        SingletonDataManager.useruniqid = user.objectId;
        SingletonDataManager.userethAdd = user.ethAddress;
        //UIManager.insta.
        SingletonDataManager.initData = true;
        CheckUserBalance();

        SingletonDataManager.insta.CheckUserData();

        //LoadPurchasedItems();
        // CovalentManager.insta.GetNFTUserBalance();
        //GetNativeBalance();
        // fetchAllTokenIds();

        // DailyPrizeManager.insta.CheckTokenStatus();
        fetchAllTokenIds();

        if (DatabaseManager.Instance)
        {
            DatabaseManager.Instance.GetData();

           //// if (UIManager.insta) UIManager.insta.UpdateUserName(DatabaseManager.Instance.GetLocalData().name, SingletonDataManager.userethAdd);

        }

    }



    #endregion


    #region BuyCoins

    public async UniTaskVoid CoinBuyOnSendContract(int index)
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin purchase process started\nThis can up to minute", false);
        string response = await PurchaseCoinsItemFromContract(index, coinCost[index]);
        if (!string.IsNullOrEmpty(response))
        {
            //SingletonDataManager.userData.score += coinCost[index];
            //SingletonDataManager.insta.UpdateUserDatabase();
            if (DatabaseManager.Instance)
            {
                LocalData data = DatabaseManager.Instance.GetLocalData();
                data.coins += coinGive[index];
                DatabaseManager.Instance.UpdateData(data);
                // DatabaseManager.data.transactionsInformation.RemoveAt(index);
            }

           //// UIManager.insta.ShowCoinPurchaseStatus(coinGive[index]);




            if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin purchased successfully", true);


        }
        else
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
        }


        await UniTask.Delay(2000);
        //SingletonDataManager.insta.GetNativeBalance();
        CheckUserBalance();
    }
    private async Task<string> PurchaseCoinsItemFromContract(BigInteger tokenId, float _cost)
    {
        object[] parameters = {
            tokenId
        };

        // Set gas estimate
        HexBigInteger value = new HexBigInteger(UnitConversion.Convert.ToWei(_cost));
        HexBigInteger gas = new HexBigInteger(150000);
        //HexBigInteger gasPrice = new HexBigInteger(35000/UnitConversion.Convert.ToWei(1, 18));
        BigInteger _gascost = UnitConversion.Convert.ToWei(30, 9);
        HexBigInteger gasPrice = new HexBigInteger(_gascost);

        Debug.Log("DataTRansfer buyCoins " + JsonConvert.SerializeObject(parameters));


        string resp = await Moralis.ExecuteContractFunction(contract, abi, "buyCoins", parameters, value, gas, gasPrice);



        if (resp != null && resp != "")
        {
            return resp;
        }

        return null;
    }
    #endregion

    #region NonBurnNFTBuy

    public async void NonBurnNFTBuyContract(BigInteger tokenId, string metadataUrl)
    {

        object[] parameters = {
            tokenId,
            metadataUrl
        };

        // Set gas estimate
        HexBigInteger value = new HexBigInteger(0);
        HexBigInteger gas = new HexBigInteger(0);
        HexBigInteger gasPrice = new HexBigInteger(0);

        Debug.Log("DataTRansfer " + JsonConvert.SerializeObject(parameters));


        string response = await Moralis.ExecuteContractFunction(contract, abi, "buyNonBurnItem", parameters, value, gas, gasPrice);

        if (string.IsNullOrEmpty(response))
        {
            if (MessaeBox.insta)
            {
               /// if (tokenId == 1) MessaeBox.insta.ShowRetryPopup((int)tokenId);
               /// else MessaeBox.insta.showMsg("Server Error", true);
            }
        }
        else
        {
            Debug.Log(response);
            if (CovalentManager.insta)
            {
                CovalentManager.insta.GetNFTUserBalance();
            }

            if (DatabaseManager.Instance)
            {
              ///  DatabaseManager.Instance.DeductCoins(50);
            }

           /// if (StoreManager.insta)
          ///  {
              ///  StoreManager.insta.DisableLastButton();
         ///   }
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Your Transaction has been recieved\nIt will reflect to your account once it is completed!", true);
        }
        // return resp;
    }
    #endregion

    #region CheckTime

    public async UniTask<string> CheckTimeStatus()
    {
        // Function ABI input parameters
        object[] inputParams = new object[0];
        //inputParams[0] = new { internalType = "address", name = "_account", type = "address" };
        // Function ABI Output parameters
        object[] outputParams = new object[1];
        outputParams[0] = new { internalType = "uint256", name = "", type = "uint256" };
        // Function ABI
        object[] abi = new object[1];
        abi[0] = new { inputs = inputParams, name = "getCurrentTime", outputs = outputParams, stateMutability = "view", type = "function" };

        // Define request object
        RunContractDto rcd = new RunContractDto()
        {
            Abi = abi,
            Params = new { _account = SingletonDataManager.userethAdd }
        };
        string resp = await Moralis.Web3Api.Native.RunContractFunction<string>(contract, "getCurrentTime", rcd, ContractChain);

        Debug.Log("CheckTimeStatus " + resp);
        return resp;



    }
    #endregion

    #region CheckNFTBalance
    async public Task<string> CheckNFTBalance()
    {
        int first = 500;
        int skip = 0;
        try
        {
            string response = "";// await EVM.AllErc1155(chain, network, PlayerPrefs.GetString("Account"), contract, first, skip);
            // string response = await EVM.BalanceOf(chain, network, PlayerPrefs.GetString("Account"), contract, first, skip);
            Debug.Log(response);
            return response;
        }
        catch (Exception e)
        {
            Debug.Log(e, this);
            return null;
        }
    }

    public async UniTask<List<NftOwner>> fetchAllTokenIds()
    {
        // CovalentManager.loadingData = true;
        //NftCollection nfts = await Moralis.Web3Api.Token.GetAllTokenIds(contract, ContractChain);




        NftOwnerCollection polygonNFTs = await Moralis.Web3Api.Account.GetNFTsForContract(SingletonDataManager.userethAdd, contract, ContractChain);
        Debug.Log(polygonNFTs.ToJson());
        Debug.Log("===============");
        //return;
        // Debug.Log(nfts.ToJson());
        List<NftOwner> currentResult = polygonNFTs.Result;


        // polygonNFTs
        CovalentManager.insta.myTokenID.Clear();
        //myNFTData.Clear();
        for (int i = 0; i < currentResult.Count; i++)
        {


            if (currentResult[i].Metadata == null)
            {
                // Sometimes GetNFTsForContract fails to get NFT Metadata. We need to re-sync
                await Moralis.GetClient().Web3Api.Token.ReSyncMetadata(currentResult[i].TokenAddress, currentResult[i].TokenId, ContractChain);
                Debug.Log("We couldn't get NFT Metadata. Re-syncing..." + currentResult[i].TokenAddress + " | " + currentResult[i].TokenId.ToString());
                //await Task.Delay(1500);
                await UniTask.Delay(1500);
                continue;
            }
            /*
                        CovalentManager.insta.myTokenID.Add(currentResult[i].TokenId);
                        string temp = currentResult[i].Metadata.Replace(@"\", "");

                        temp = temp.Remove(temp.Length - 1, 1);
                        temp = temp.Remove(0, 1);
                        JSONObject j = new JSONObject(currentResult[i].Metadata);
                        //currentResult[i].TokenId
                        Debug.Log("Json PrePro " + currentResult[i].Metadata);
                        //Debug.Log("Json Pre" + JsonUtility.FromJson(currentResult[i].Metadata));
                        Debug.Log("Json " + j.Print(true));

                        MyMetadataNFT _nftData = new MyMetadataNFT();
                        _nftData.name = j.GetField("name").stringValue;
                        _nftData.description = j.GetField("description").stringValue;
                        _nftData.image = j.GetField("image").stringValue;
                        _nftData.tokenId = currentResult[i].TokenId;
                        _nftData.itemid = j.GetField("itemid").intValue;// (int.Parse(currentResult[i].TokenId));*/
            // myNFTData.Add(_nftData);
        }
        return currentResult;

        //CovalentManager.loadingData = false;

    }
    #endregion

    #region CheckUserBalance


    public async UniTaskVoid CheckUserBalance()
    {
        // get BSC native balance for a given address
        NativeBalance BSCbalance = await Moralis.Web3Api.Account.GetNativeBalance(SingletonDataManager.userethAdd.ToLower(), ContractChain);
        //Debug.Log(BSCbalance.ToJson());

        if (BSCbalance.Balance != null)
        {
            Debug.Log("Balance " + UnitConversion.Convert.FromWei(BigInteger.Parse(BSCbalance.Balance)));
            userBalance = (float)Math.Round((double)UnitConversion.Convert.FromWei(BigInteger.Parse(BSCbalance.Balance)), 2);
            //if (InAppManager.insta) InAppManager.insta.userBalance.text = Math.Round(userBalance, 2).ToString();
        }

    }
    #endregion

    #region Random Number Generator

    async public UniTask<int> GetRandomNumber()
    {
        object[] parameters = { };

        // Set gas estimate
        HexBigInteger value = new HexBigInteger(0);
        HexBigInteger gas = new HexBigInteger(0);
        HexBigInteger gasPrice = new HexBigInteger(0);

        Debug.Log("DataTRansfer " + JsonConvert.SerializeObject(parameters));


        string response = await Moralis.ExecuteContractFunction(contractRandom, abiRandom, "SendRandomNoRequest", parameters, value, gas, gasPrice);
        Debug.Log("GetRandomNumber " + response);

        if (!string.IsNullOrEmpty(response))
        {
            var result = await GetRandomNoFromContract();
            if (!string.IsNullOrEmpty(result)) return int.Parse(result);
        }

        return -1;
    }

    public async UniTask<string> GetRandomNoFromContract()
    {
        // Function ABI input parameters
        object[] inputParams = new object[1];
        inputParams[0] = new { internalType = "address", name = "player", type = "address" };
        // Function ABI Output parameters
        object[] outputParams = new object[1];
        outputParams[0] = new { internalType = "uint256", name = "", type = "uint256" };
        // Function ABI
        object[] abi = new object[1];
        abi[0] = new { inputs = inputParams, name = "GetRandomNo", outputs = outputParams, stateMutability = "view", type = "function" };

        // Define request object
        RunContractDto rcd = new RunContractDto()
        {
            Abi = abi,
            Params = new { player = SingletonDataManager.userethAdd }
        };
        string resp = await Moralis.Web3Api.Native.RunContractFunction<string>(contractRandom, "GetRandomNo", rcd, ContractChain);

        Debug.Log("GetRandomNoFromContract " + resp);
        return resp;



    }
    #endregion


    #region NFTUploaded


    public void purchaseItem(int _id, bool _skin)
    {
        Debug.Log("purchaseItem");

        MetadataNFT meta = new MetadataNFT();


        meta.itemid = DatabaseManager.Instance.allMetaDataServer[_id].itemid;
        meta.name = DatabaseManager.Instance.allMetaDataServer[_id].name;
        meta.description = DatabaseManager.Instance.allMetaDataServer[_id].description;
        meta.image = DatabaseManager.Instance.allMetaDataServer[_id].imageurl;

        // StartCoroutine(UploadNFTMetadata(Newtonsoft.Json.JsonConvert.SerializeObject(meta), _id, _skin));

        //string saveFile = Application.persistentDataPath + "/metadata.json";
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(meta);

        // Write JSON to file.
        //File.WriteAllText(saveFile, jsonString);

        //Debug.Log("Read" + File.ReadAllText(saveFile));

        StartCoroutine(Upload(jsonString, _id, _skin));

    }

    IEnumerator Upload(string _metadata, int _id, bool _skin)
    {

        if (MessaeBox.insta) MessaeBox.insta.showMsg("NFT purchase process started\nThis can up to minute", false);

        var form = new WWWForm();
        form.AddBinaryData("file", System.Text.Encoding.UTF8.GetBytes(_metadata), "metadata.json", "application/json");
        using (UnityWebRequest www = UnityWebRequest.Post("https://api.nft.storage/upload", form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + ConstantManager.nftStorage_key);
            www.timeout = 60;
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
                Debug.Log("UploadNFTMetadata upload error " + www.downloadHandler.text);

                if (_id == 1)
                {
                    if (MessaeBox.insta)
                    {
                      ///  MessaeBox.insta.ShowRetryPopup(_id);
                    }
                }
                else
                {
                    if (MessaeBox.insta) MessaeBox.insta.showMsg("Server error\nPlease try again", true);
                }

            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log("UploadNFTMetadata upload complete! " + www.downloadHandler.text);

                JSONObject j = new JSONObject(www.downloadHandler.text);
                if (j.HasField("ok"))
                {
                    if (j.GetField("ok").boolValue)
                    {
                        if (!string.IsNullOrEmpty(j.GetField("value").GetField("cid").stringValue))
                        {
                            SingletonDataManager.nftmetaCDI = @"ipfs://" + j.GetField("value").GetField("cid").stringValue + "/metadata.json";

                            Debug.Log("Metadata saved successfully " + SingletonDataManager.nftmetaCDI);
                            NonBurnNFTBuyContract(_id, SingletonDataManager.nftmetaCDI);
                            yield break;
                        }
                    }
                }

                if (_id == 1)
                {
                    if (MessaeBox.insta)
                    {
                       /// MessaeBox.insta.ShowRetryPopup(_id);
                    }
                }
                else
                {
                    if (MessaeBox.insta) MessaeBox.insta.showMsg("Server error\nPlease try again", true);
                }

            }
        }
    }



    public void writeFile()
    {

    }

    IEnumerator UploadNFTMetadata(string _metadata, int _id, bool _skin)
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("NFT purchase process started\nThis can up to minute", false);
        Debug.Log("Creating and saving metadata to IPFS..." + _metadata);
        WWWForm form = new WWWForm();
        form.AddField("meta", _metadata);

        using (UnityWebRequest www = UnityWebRequest.Post("https://api.nft.storage/store", form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + ConstantManager.nftStorage_key);
            www.timeout = 40;
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                Debug.Log("UploadNFTMetadata upload error " + www.downloadHandler.text);

                if (_id == 1)
                {
                    if (MessaeBox.insta)
                    {
                        /// MessaeBox.insta.ShowRetryPopup(_id);
                    }
                }
                else
                {
                    if (MessaeBox.insta) MessaeBox.insta.showMsg("Server error\nPlease try again", true);
                }
                www.Abort();
                www.Dispose();
            }
            else
            {
                Debug.Log("UploadNFTMetadata upload complete! " + www.downloadHandler.text);

                JSONObject j = new JSONObject(www.downloadHandler.text);
                if (j.HasField("value"))
                {
                    //Debug.Log("Predata " + j.GetField("value").GetField("ipnft").stringValue);
                    SingletonDataManager.nftmetaCDI = j.GetField("value").GetField("url").stringValue; //ipnft
                    //SingletonDataManager.tokenID = j.GetField("value").GetField("ipnft").stringValue; //ipnft
                    Debug.Log("Metadata saved successfully");
                    //PurchaseItem(cost, _id);
                    NonBurnNFTBuyContract(_id, j.GetField("value").GetField("url").stringValue);
                }
            }
        }
    }
    #endregion
}
