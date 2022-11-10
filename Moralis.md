
## Moralis Unity SDK

* Authentication of user
* Game and user database
* All Smart contract call, run and execute interaction
* Moralis unit conversion tools

### Script :
https://github.com/MoraG22/TowerDefence/blob/main/TowerDefence/Assets/Scripts/BlockChain/MoralisManager.cs
``` C#
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
        HexBigInteger value = new HexBigInteger(BigInteger.Parse(UnitConversion.Convert.ToWei(_cost,18).ToString()));
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
        await UniTask.Delay(UnityEngine.Random.Range(5000, 10000));
        goto COMEHERE;

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
        else
        {
            if (MessaeBox.insta) MessaeBox.insta.showMsg("Transaction Has Been Failed", true);
        }

        UIManager.Instance.tokenButton.SetActive(false);


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
```
