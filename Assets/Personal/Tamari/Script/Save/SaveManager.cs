using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager
{
    static SaveManager Instance = new SaveManager();

    public static List<string> _weaponFileList = new List<string>();

#if RELEASE
    const string TAIKENFILEPATH = "Taiken.dat";
    const string SOUKENFILEPATH = "Souken.dat";
    const string HAMMERFILEPATH = "Hammer.dat";
    const string YARIFILEPATH = "Yari.dat";

    const string TAIKENSAMPLEFILEPATH = "Assets/SampleSaveData/Taiken.dat";
    const string SOUKENSAMPLEFILEPATH = "Assets/SampleSaveData/Souken.dat";
    const string HAMMERSAMPLEFILEPATH = "Assets/SampleSaveData/Hammer.dat";
    const string YARISAMPLEFILEPATH = "Assets/SampleSaveData/Yari.dat";
#else
    public const string GREATSWORDFILEPATH = "SaveData/Taiken.json";
    public const string DUALBLADESFILEPATH = "SaveData/Souken.json";
    public const string HAMMERFILEPATH = "SaveData/Hammer.json";
    public const string SPEARFILEPATH = "SaveData/Yari.json";

    public const string GREATSWORDSAMPLEFILEPATH = "SampleSaveData/SampleTaiken.json";
    public const string DUALBLADESSAMPLEFILEPATH = "SampleSaveData/SampleSouken.json";
    public const string HAMMERSAMPLEFILEPATH = "SampleSaveData/SampleHammer.json";
    public const string SPEARSAMPLEFILEPATH = "SampleSaveData/SampleYari.json";
#endif
    private SaveData Data;

    public static void Initialize()
    {
        _weaponFileList = new List<string>() { GREATSWORDFILEPATH, DUALBLADESFILEPATH, HAMMERFILEPATH, SPEARFILEPATH };
    }
    static public SaveData Load(string filePath)
    {
        Instance.Data = LocalData.Load<SaveData>(filePath);

        if (Instance.Data == null)
        {
            Instance.Data = new SaveData();
        }
        return Instance.Data;
    }

    static public SaveData GetData(string filePath)
    {
        if (Instance.Data == null)
        {
            Load(filePath);
        }
        return Instance.Data;
    }

    static public void Save(string filePath, SaveData data)
    {
        Instance.Data = data;
        LocalData.Save<SaveData>(filePath, Instance.Data);
    }

    static public void ResetSaveData(string filePath)
    {
        LocalData.ResetSaveData(filePath);
    }
}