using Cysharp.Threading.Tasks;
using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class BlockChainManager : MonoBehaviour
{
    #region Singleton
    public static BlockChainManager Instance;

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

   

    public const string abiToken = "[{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"initialSupply\",\"type\":\"uint256\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"GetGameToken\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getSmartContractBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_account\",\"type\":\"address\"}],\"name\":\"getuserBalance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseAllowance\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply_\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_another\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}],\"name\":\"withdrawErc20\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

    public const string contractToken = "0xE3b5870Eed470A79fEaBDDaB438ea14A78dbeef4";



 
    #region BuyCoins
    async public void CoinBuyOnSendContract(int _pack)
    {
        MoralisManager.Instance.CoinBuyOnSendContract(_pack);
    }
    #endregion



    async public static void CheckTransactionStatusWithTransID(string _trxID, int _type)
    {
        Debug.Log("Check CheckTransactionStatusWithTransID ");
        int _counter = 0;
        HERE:
        Debug.Log("Check Transaction " + _counter);
        _counter++;
        try
        {
            string txConfirmed = "";// await EVM.TxStatus("", "", _trxID, networkRPC);
            Debug.Log(txConfirmed); // success, fail, pending

            if (txConfirmed.Equals("success"))
            {
                Debug.Log("success sent");
                //return true;
                if (_type == 0) //coin balanace
                {
                    if (DatabaseManager.Instance)
                    {
                        DatabaseManager.Instance.ChangeTransactionStatus(_trxID, txConfirmed);
                        MessaeBox.insta.showMsg("Coin transaction confirmed and credited", true);
                    }
                }
                if (_type == 1) // token confirm
                {
                    getTokenBalance();
                    MessaeBox.insta.showMsg("Game token transaction confirmed and credited", true);
                }
            }
            else
            {
                Debug.Log("failed sent");
                if (_counter > 15)
                {
                    Debug.Log("failed sent 2");
                    //return false;
                }
                else
                {
                    Debug.Log("failed sent 3");
                    await UniTask.Delay(5000, true);
                    Debug.Log("failed sent 4");
                    goto HERE;
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        //return false;
    }



    async public void getDailyToken()
    {

        object[] inputParams = { };
        string method = "GetGameToken"; // buyBurnItem";// "buyCoins";

        // array of arguments for contract
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        // value in wei
        string value = "";// Convert.ToDecimal(wei).ToString();
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        string response = "";
        // connects to user's browser wallet (metamask) to update contract state
        try
        {

#if !UNITY_EDITOR
               // response = await Web3GL.SendContract(method, abiToken, contractToken, args, value, gasLimit, gasPrice);
                Debug.Log(response);
#else
            string data = "";// await EVM.CreateContractData(abiToken, method, args);
            response = "";// await Web3Wallet.SendTransaction(chainId, contractToken, "0", data, gasLimit, gasPrice);
            Debug.Log(response);
#endif

        }
        catch (Exception e)
        {
            Debug.Log("error" + e);
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Server Error", true);
            return;
        }

        if (!string.IsNullOrEmpty(response))
        {
            MessaeBox.insta.showMsg("Token will be credited soon", true);
            CheckTransactionStatusWithTransID(response, 1);
            //Debug.Log("In check");
            //CheckTransactionStatusWithTransID(response);


        }
        else
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Server Error", true);
            Debug.Log("In check blank");
        }

    }



    #region CheckTime
    public async Task<string> CheckTimeStatus()
    {
        // smart contract method to call
        string method = "getCurrentTime";
        // array of arguments for contract
        object[] inputParams = { };
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        try
        {
            string response = "";// await EVM.Call(chain, network, contract, abi, method, args, networkRPC);
            Debug.Log(response);
            return response;

        }
        catch (Exception e)
        {
            Debug.Log(e, this);
            return "";
        }
    }

    #endregion

    #region CheckNFTBalance
    public async Task<List<string>> GetNFTList()
    {
        return await MoralisManager.Instance.GetNFTList();
    }

    async public static void getTokenBalance()
    {
        // smart contract method to call
        string method = "getuserBalance";
        // array of arguments for contract
        object[] inputParams = { PlayerPrefs.GetString("Account") };
        string args = Newtonsoft.Json.JsonConvert.SerializeObject(inputParams);
        try
        {
            string response = "";// await EVM.Call(chain, network, contractToken, abiToken, method, args, networkRPC);
            Debug.Log(response);
            try
            {
                float wei = float.Parse(response);
                float decimals = 1000000000000000000; // 18 decimals
                float eth = wei / decimals;
                // print(Convert.ToDecimal(eth).ToString());
                var tokenBalance = Convert.ToDecimal(eth).ToString();
                Debug.Log("Token Bal : " + Convert.ToDecimal(eth).ToString() + " | " + response);
                if (DatabaseManager.Instance)
                {
                    LocalData data = DatabaseManager.Instance.GetLocalData();
                    data.tokens = tokenBalance;
                    DatabaseManager.Instance.UpdateData(data);
                    // if (UIManager.insta) UIManager.insta.UpdatePlayerUIData(true, data);
                }
            }
            catch (Exception)
            {
            }


        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }
    #endregion

    #region CheckUserBalance
    async public void CheckUserBalance()
    {

    }
    #endregion


    #region NFTUploaded

    public void purchaseItem(int _id, bool _skin)
    {
        Debug.Log("purchaseItem");

        MoralisManager.Instance.purchaseItem(_id, _skin);
    }

    #endregion







}
