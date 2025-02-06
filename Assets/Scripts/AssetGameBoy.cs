using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class AssetGameBoy : ScriptableObject
{
    [SerializeField]
    private string _assetPath;
    public string assetPath {
        get { return _assetPath; } 
        set {
            if (value != null)    
                _assetPath = value; 
        }
     }
    [SerializeField]
    private byte[] data =null;
    

    public int readDataInMemory()
    {
        if (assetPath == null)
        {
            Debug.Log("Asset path is not defined");
            return 0;
        }
        if (data == null || data.Length == 0)
        {
            data = File.ReadAllBytes(assetPath);
        }
        return GetSize();
    }
    public byte[] GetData() { return data; }
    public int GetSize()
    {
        return data == null ? 0 : data.Length;  
    }

    public void setAssetPathAndData(string assetPath, byte[] data)
    {
        this.assetPath = assetPath;
        this.data = data;   
    }
}
