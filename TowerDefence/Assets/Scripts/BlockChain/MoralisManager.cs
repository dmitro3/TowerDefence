using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Web3Api.Models;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
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

    public static string abi = "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_itemId\",\"type\":\"uint256\"}],\"name\":\"BuyCoins\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_tokenId\",\"type\":\"uint256\"}],\"name\":\"buyNonBurnItem\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}],\"name\":\"ConvertTokens\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_tokenAddress\",\"type\":\"address\"}],\"name\":\"SetERC20Currency\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_recipient\",\"type\":\"address\"}],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_add\",\"type\":\"address\"}],\"name\":\"GetAllUserToken\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getCurrentTime\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"_result\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"mintingCurrency\",\"outputs\":[{\"internalType\":\"contract ERC20Spendable\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"TokenBalnace\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
    public static string abiToken = "[{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"addSpender\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}],\"name\":\"ExchangeToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"GetGameToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceSpender\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"spend\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"SpenderAdded\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"SpenderRemoved\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"GetCurrentTime\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"_result\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"GetSmartContractBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_account\",\"type\":\"address\"}],\"name\":\"GetuserBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"isSpender\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";

    public static string contract = "";
    public static string contractToken = "";







    float[] coinCost = { 0.025f, 0.050f, 0.075f, 0.1f };
    int[] coinGive = { 1000, 2000, 3500, 6000 };



    private int expirationTime;
    private string account;

    [SerializeField] TMP_Text _status;
    [SerializeField] GameObject playBTN;
    [SerializeField] GameObject loginBTN;
    [SerializeField] GameObject gameManagerOBJ;
    [SerializeField] GameObject authenticationObj;
    [SerializeField] GameObject chainSelectionObj;

    //public static string username;
    //public static string userethAdd;
    //public static string useruniqid;
    private bool initData = false;

    public static ChainList ContractChain = ChainList.mumbai;

    //public static decimal userBalance;
    public static float userBalance = 0;
    private void Start()
    {
        authenticationObj.SetActive(false);
        chainSelectionObj.SetActive(true);
    }


    public void SelectChain(int _no)
    {
        switch (_no)
        {
            case 0: //Fantom Mainnet
                ContractChain = ChainList.fantom;
                contract = "0x648aa926936E7b91F3138024A944DFd3E9954618";
                contractToken = "";
                PlayerPrefs.SetString("DappUrl", "https://6pnblmsbhkkx.grandmoralis.com:2053/server");
                PlayerPrefs.SetString("DappId", "fXyYL0zbsxExxwtBMclXBZe7opeboSRLSTyD89cm");
                PlayerPrefs.Save();
                CovalentManager.chainID = "250";
                break;
            case 1://Polygone Testnet
                ContractChain = ChainList.mumbai;
                contract = "0xB62e0C1DBf6fa72CA268Dce932362b8F49292F0c";
                contractToken = "0x00cE436B035167467B1E32339391C9fCd9E32c10";
                PlayerPrefs.SetString("DappUrl", "https://et300qpmxtn4.grandmoralis.com:2053/server");
                PlayerPrefs.SetString("DappId", "mgLPobQvpgrftWYX0KKE4Qtgkvt2Ww3WXwOma5EX");
                PlayerPrefs.Save();
                CovalentManager.chainID = "80001";
                break;
            case 2://Binance Testnet
                ContractChain = ChainList.bsc_testnet;
                contract = "0x21901889C588B35974308ae8Cf9d1fd9a8Fc6aA0";
                contractToken = "0xC1e388Fc998eDd13d3cC1eD6b26E7199cbAdFaf5";
                PlayerPrefs.SetString("DappUrl", "https://et300qpmxtn4.grandmoralis.com:2053/server");
                PlayerPrefs.SetString("DappId", "mgLPobQvpgrftWYX0KKE4Qtgkvt2Ww3WXwOma5EX");
                PlayerPrefs.Save();
                CovalentManager.chainID = "97";
                break;
        }
        chainSelectionObj.SetActive(false);
        authenticationObj.SetActive(true);
    }

    #region MoralisArea

    public async void getUserDataonStart()
    {

        var user = await Moralis.GetClient().GetCurrentUserAsync();
        if (user == null) return;
        SingletonDataManager.username = user.username;
        SingletonDataManager.userethAdd = user.ethAddress;
        SingletonDataManager.useruniqid = user.objectId;
        SingletonDataManager.userethAdd = user.ethAddress;

        SingletonDataManager.initData = true;
       

        SingletonDataManager.insta.CheckUserData();


        //fetchAllTokenIds();

        if (DatabaseManager.Instance)
        {
            DatabaseManager.Instance.GetData();
        }
        CheckUserBalance();
        GetTokenBalance();

        CovalentManager.insta.StartCoroutine(CovalentManager.insta.GetAddressBalanaces());

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
                UIManager.Instance.UpdatePlayerUIData(data);
                // DatabaseManager.data.transactionsInformation.RemoveAt(index);
            }

            UIManager.Instance.ShowInfoMsg(coinGive[index] + "Coins Purchased");
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin purchased successfully", true);

        }
        else
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
        }


       
    }
    private async Task<string> PurchaseCoinsItemFromContract(BigInteger tokenId, float _cost)
    {
        object[] parameters = {
            tokenId
        };

        // Set gas estimate
        HexBigInteger value = new HexBigInteger(UnitConversion.Convert.ToWei(_cost));
        HexBigInteger gas = new HexBigInteger(0);
        // HexBigInteger gas = new HexBigInteger(150000);

        BigInteger _gascost = UnitConversion.Convert.ToWei(30, 9);
        //HexBigInteger gasPrice = new HexBigInteger(_gascost);
        HexBigInteger gasPrice = new HexBigInteger(0);

        Debug.Log("DataTRansfer buyCoins " + JsonConvert.SerializeObject(parameters));


        string resp = await Moralis.ExecuteContractFunction(contract, abi, "BuyCoins", parameters, value, gas, gasPrice);



        if (resp != null && resp != "")
        {
            return resp;
        }

        return null;
    }
    #endregion

    #region NonBurnNFTBuy

    public async void NonBurnNFTBuyContract(BigInteger tokenId)
    {

        object[] parameters = {
            tokenId
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
                if (MessaeBox.insta) MessaeBox.insta.showMsg("Your Transaction has been failed", true);
            }
        }
        else
        {
            Debug.Log(response);
            
            /// if (StoreManager.insta)
            ///  {
            ///  StoreManager.insta.DisableLastButton();
            ///   }
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Your Transaction has been recieved\nIt will reflect to your account once it is completed!", true);

            if (UIManager.Instance) UIManager.Instance.DeductCoins((int)tokenId);

            if (StoreUI.insta)
            {
                StoreUI.insta.SetBalanceText();
            }
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
    public async Task<List<string>> GetNFTList()
    {
        List<string> nftList = new List<string>();
        nftList.Clear();
        // Function ABI input parameters
        object[] inputParams = new object[1];
        inputParams[0] = new { internalType = "address", name = "_account", type = "address" };
        // Function ABI Output parameters
        object[] outputParams = new object[1];
        outputParams[0] = new { internalType = "string", name = "", type = "string" };
        // Function ABI
        object[] abi = new object[1];
        abi[0] = new { inputs = inputParams, name = "GetAllUserToken", outputs = outputParams, stateMutability = "view", type = "function" };

        // Define request object
        RunContractDto rcd = new RunContractDto()
        {
            Abi = abi,
            Params = new { _account = SingletonDataManager.userethAdd }
        };
        string response = await Moralis.Web3Api.Native.RunContractFunction<string>(contract, "GetAllUserToken", rcd, ContractChain);

        Debug.Log("GetNFTList " + response);

        string[] splitArray = response.Split(char.Parse(",")); //return one word for each string in the array
                                                               //here, splitArray[0] = Give; splitArray[1] = me etc...

        for (int i = 0; i < splitArray.Length; i++)
        {
            if (string.IsNullOrEmpty(splitArray[i])) continue;
            nftList.Add(splitArray[i]);
        }

        //if (MetaManager.insta) MetaManager.insta.UpdatePlayerWorldProperties();
        return nftList;

    }


    #endregion

    #region CheckUserBalance


    public async UniTaskVoid CheckUserBalance()
    {
        COMEHERE:
        // get BSC native balance for a given address
        NativeBalance BSCbalance = await Moralis.Web3Api.Account.GetNativeBalance(SingletonDataManager.userethAdd.ToLower(), ContractChain);
        //Debug.Log(BSCbalance.ToJson());

        if (BSCbalance.Balance != null)
        {
            Debug.Log("Balance " + UnitConversion.Convert.FromWei(BigInteger.Parse(BSCbalance.Balance)));
            userBalance = (float)Math.Round((double)UnitConversion.Convert.FromWei(BigInteger.Parse(BSCbalance.Balance)), 4);
            //if (InAppManager.insta) InAppManager.insta.userBalance.text = Math.Round(userBalance, 2).ToString();

            if (StoreUI.insta)
            {
                StoreUI.insta.SetBalanceText();
            }

            SingletonDataManager.userMainBalance = userBalance.ToString();
            if (UIManager.Instance) UIManager.Instance.SetMainBalance();
        }
        await UniTask.Delay(UnityEngine.Random.Range(5000,10000));
        goto COMEHERE;

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


        string response = await Moralis.ExecuteContractFunction("", "", "SendRandomNoRequest", parameters, value, gas, gasPrice);
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
        string resp = await Moralis.Web3Api.Native.RunContractFunction<string>("", "GetRandomNo", rcd, ContractChain);

        Debug.Log("GetRandomNoFromContract " + resp);
        return resp;



    }
    #endregion


    #region NFTUploaded


    public void purchaseItem(int _id, bool _skin)
    {
        Debug.Log("purchaseItem");
        if (MessaeBox.insta) MessaeBox.insta.showMsg("NFT purchase process started\nThis can up to minute", false);
        NonBurnNFTBuyContract(_id);
    }

    #endregion

    #region TokenFunctions

    public async void GetTokenReward()
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("Token redeem process started", false);
        object[] parameters = {
        };

        // Set gas estimate
        HexBigInteger value = new HexBigInteger(0);
        HexBigInteger gas = new HexBigInteger(0);
        // HexBigInteger gas = new HexBigInteger(150000);

        //BigInteger _gascost = UnitConversion.Convert.ToWei(30, 9);
        //HexBigInteger gasPrice = new HexBigInteger(_gascost);
        HexBigInteger gasPrice = new HexBigInteger(0);

        Debug.Log("DataTRansfer buyCoins " + JsonConvert.SerializeObject(parameters));


        string resp = await Moralis.ExecuteContractFunction(contractToken, abiToken, "GetGameToken", parameters, value, gas, gasPrice);



        if (!string.IsNullOrEmpty(resp))
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Token redeemed successfully", true);
        }
        else {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
        }

      
    }

    public async UniTaskVoid ExchangeTokenUI(int index)
    {
        if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin exchange process started", false);
        string response = await ExchangeToken(index);
        if (!string.IsNullOrEmpty(response))
        {
            //SingletonDataManager.userData.score += coinCost[index];
            //SingletonDataManager.insta.UpdateUserDatabase();
            if (DatabaseManager.Instance)
            {
                LocalData data = DatabaseManager.Instance.GetLocalData();
                data.coins += coinGive[index];
                DatabaseManager.Instance.UpdateData(data);
                UIManager.Instance.UpdatePlayerUIData(data);
                // DatabaseManager.data.transactionsInformation.RemoveAt(index);
            }

            UIManager.Instance.ShowInfoMsg(coinGive[index] + "Coins Exchanged");
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Coin exchanged successfully", true);

        }
        else
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
        }

    }

    async Task<string> ExchangeToken(int packID)
    {
        string _amount = UnitConversion.Convert.ToWei(packID + 1, 18).ToString();
        object[] parameters = {
            contractToken,
            _amount
        };

        // Set gas estimate
        HexBigInteger value = new HexBigInteger(0);
        HexBigInteger gas = new HexBigInteger(0);
        // HexBigInteger gas = new HexBigInteger(150000);

        //BigInteger _gascost = UnitConversion.Convert.ToWei(30, 9);
        //HexBigInteger gasPrice = new HexBigInteger(_gascost);
        HexBigInteger gasPrice = new HexBigInteger(0);

        Debug.Log("DataTRansfer buyCoins " + JsonConvert.SerializeObject(parameters));


        string resp = await Moralis.ExecuteContractFunction(contractToken, abiToken, "transfer", parameters, value, gas, gasPrice);



        if (resp != null && resp != "")
        {
            return resp;
        }

        return null;
    }




    public async UniTaskVoid GetTokenBalance()
    {
        COMEHERE:
        // Function ABI input parameters
        object[] inputParams = new object[1];
        inputParams[0] = new { internalType = "address", name = "account", type = "address" };
        // Function ABI Output parameters
        object[] outputParams = new object[1];
        outputParams[0] = new { internalType = "uint256", name = "", type = "uint256" };
        // Function ABI
        object[] abiThis = new object[1];
        abiThis[0] = new { inputs = inputParams, name = "balanceOf", outputs = outputParams, stateMutability = "view", type = "function" };
        // Define request object
        RunContractDto rcd = new RunContractDto()
        {
            Abi = abiThis,
            Params = new { account = SingletonDataManager.userethAdd }
        };
        string resp = await Moralis.Web3Api.Native.RunContractFunction<string>(contractToken, "balanceOf", rcd, ContractChain);
        //Debug.Log("GetTokenBalance " + resp);

        if (!string.IsNullOrEmpty(resp))
            SingletonDataManager.userTokenBalance = Math.Round((double)UnitConversion.Convert.FromWei(BigInteger.Parse(resp)), 4).ToString();

        //SingletonDataManager.userTokenBalance = UnitConversion.Convert.FromWei(long.Parse(resp)).ToString();

        if (UIManager.Instance) UIManager.Instance.SetTokenBalance();
        Debug.Log("GetTokenBalance " + SingletonDataManager.userTokenBalance);

        await UniTask.Delay(UnityEngine.Random.Range(5000, 10000));
        goto COMEHERE;

    }
    #endregion
}
