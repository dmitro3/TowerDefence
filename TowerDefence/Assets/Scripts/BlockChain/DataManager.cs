using MoralisUnity.Platform.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataManager
{

}



[System.Serializable]
public class MetaJungleNFTLocal
{
    public int itemid;
    public string name;
    public string description;
    public string imageurl;
    public int cost;
    public Texture imageTexture;
}

[System.Serializable]
public class MetaFunNFTLocal
{
    public int itemid;
    public string name;
    public string description;
    public string imageurl;
    public int cost;
    public Texture imageTexture;
}



[System.Serializable]
public class MetadataNFT
{
    public int itemid;
    public string name;
    public string description;
    public string image;
    public string jsonData;
    //public properties properties =  new properties();
}

[System.Serializable]
public class MyMetadataNFT
{
    public int itemid;
    public string name;
    public string description;
    public string image;
    public string tokenId;
    //public properties properties =  new properties();
}

[System.Serializable]
public class properties
{
    public string videoClip = null;
}


public class Userdata : MoralisObject
{
    public string userid { get; set; }
    public string userdata { get; set; }
    public string gamedata { get; set; }

    public Userdata() : base("Userdata") { }
}

public class NFTData : MoralisObject
{
    public string data { get; set; }
    public NFTData() : base("NFTData") { }
}


