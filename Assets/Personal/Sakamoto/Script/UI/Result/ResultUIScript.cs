using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class ResultUIScript : MonoBehaviour
{
    [SerializeField] private OreUIScript[] _oreUiCs = new OreUIScript[3];
    [SerializeField] private OreUIScript _selectWeaponOre;
    [SerializeField] private Button[] _oreButton = new Button[3];
    [SerializeField] private Button[] _weaponButton = new Button[4];
    [SerializeField] private Button[] _skillSelectButton = new Button[2];
    [SerializeField] private Button[] _blackSmithSelectButton = new Button[2];
    [SerializeField] private Text[] _oreText = new Text[3];
    [SerializeField] private GameObject _oreSelectObj;
    [SerializeField] private GameObject _enhanceSelectObj;
    [SerializeField] private GameObject _skillSelectPanel;
    [SerializeField] private GameObject _blacksmithSelectPanel;
    [SerializeField] private SkillSelectButtonScript[] _skillSelectButtonCs = new SkillSelectButtonScript[2];
    [SerializeField] private ActorGenerator _actorGenerator;
    [SerializeField] private SkillDataManagement _skillDataManagement;
    [SerializeField] private Sprite[] _oreImage = new Sprite[3];
    [SerializeField] private ResultWeaponButton[] _resultUICs = new ResultWeaponButton[4];
    [SerializeField] private Image[] _weaponImage = new Image[4];
    [SerializeField] private Sprite[] _weaponSampleSprite = new Sprite[4];
    [SerializeField] private Text[] _weaponNameText = new Text[4];
    private WeaponData[] _saveWeaponData;
    private OreData _selectOreData;
    private int _currentSelectWeapon;
    private GameObject[] _saveObject = new GameObject[4];

    [SceneName]
    [SerializeField] private string _blacksmithSceneName;
    [Tooltip("リザルトシーンのテスト用")]
    [SerializeField] private bool _resultSceneTest = false;
    [SceneName]
    [SerializeField] private string _homeScene;
    private bool _isBlacksmith = false;


    void Start()
    {
        if (_resultSceneTest)
        {
            StartResultLottery();
        }
    }

    public void StartResultLottery()
    {
        _oreSelectObj.SetActive(true);
        RewardLottery();
        if (_actorGenerator.EnemyController.EnemyStatus.IsBoss)
        {
            GameManager.BossClear();
            FindWeaponTypeDontHave();
        }
    }

    /// <summary>
    /// 報酬の抽選をしてデータを渡す
    /// </summary>
    private void RewardLottery()
    {
        _actorGenerator.PlayerController.PlayerStatus.AddRankPoint();
        Debug.Log("抽選");
        for (int i = 0; i < _oreUiCs.Length; i++)
        {
            var oreInfo = RarityLottery(i);
            OreData Ore = new OreData(SetEnhanceData(oreInfo.rearity), oreInfo.rearity, oreInfo.randomSkill, oreInfo.oreImage);
            _oreUiCs[i].SetData(Ore);
            _oreButton[i].onClick.AddListener(() => EnhanceWeaponEvent(Ore));
            _oreButton[i].onClick.AddListener(() => SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Select_Ingame"));
        }
    }

    /// <summary>
    /// 強化するStatusを返してくれる
    /// </summary>
    /// <returns></returns>
    private EnhanceData[] SetEnhanceData(OreRarity oreRarity)
    {
        EnhanceData[] enhanceData = new EnhanceData[DataBaseScript.DescriptionEnhanceData.Count];
        for (int i = 0; i < DataBaseScript.DescriptionEnhanceData.Count; i++)
        {
            //そのステータスを強化するかどうか
            int enhanceBool = Random.Range(0, 2);
            if (enhanceBool == 0)
            {
                //強化する数字
                int enhanceNum = Random.Range(1, 3);
                enhanceData[i].EnhanceNum = enhanceNum;
                enhanceData[i].EnhanceDescription = DataBaseScript.DescriptionEnhanceData[i].EnhanceDescription;
            }
            else
            {
                if (oreRarity == OreRarity.Normal)
                {
                    int downNum = Random.Range(-1, -3);
                    enhanceData[i].EnhanceNum = downNum;
                    enhanceData[i].EnhanceDescription = DataBaseScript.DescriptionEnhanceData[i].EnhanceDescription;
                }
            }
        }
        return enhanceData;
    }

    private (Sprite oreImage, OreRarity rearity, SkillBase randomSkill) RarityLottery(int uiNum)
    {
        SkillBase randomSkill;
        OreRarity rearity = (OreRarity)Random.Range(0, 2);
        WeaponData[] weaponDatas = _actorGenerator.PlayerController.PlayerStatus.WeaponDatas;
        WeaponData weaponData = weaponDatas[Random.Range(0, weaponDatas.Length)];
        var randomNum = Random.Range(0, 100);

        if (uiNum == 0 && _actorGenerator.EnemyController.EnemyStatus.IsBoss) 
        {
            randomSkill = _skillDataManagement.OnSkillCall(WeaponType.Any, SkillType.Epic);
            rearity = (OreRarity)2;
        }
        else if (20 < randomNum)
        {
            randomSkill = _skillDataManagement.OnSkillCall(weaponData.WeaponType, SkillType.Skill);
        }
        else
        {
            randomSkill = _skillDataManagement.OnSkillCall(weaponData.WeaponType, SkillType.Special);
        }

        Sprite oreImage = _oreImage[(int)rearity];
        _oreText[uiNum].text = rearity.ToString();

        return (oreImage, rearity, randomSkill);
    }

    /// <summary>
    ///鉱石を選んだ際にどの武器に鉱石を混ぜるか選択できるEventに移る
    /// </summary>
    public void EnhanceWeaponEvent(OreData selectOreData)
    {
        _oreSelectObj.SetActive(false);
        _enhanceSelectObj.SetActive(true);
        _selectWeaponOre.SetOreData(selectOreData);
        WeaponData[] weaponDatas;

        if (_resultSceneTest)
        {
            weaponDatas = new WeaponData[4];
            for (int i = 0; i < weaponDatas.Length; i++)
            {
                weaponDatas[i] = new(50, 50, 50, 50, WeaponData.AttributeType.None, WeaponType.GreatSword);
            }
        }
        else
        {
            weaponDatas = _actorGenerator.PlayerController.PlayerStatus.WeaponDatas;
        }

        for (int i = 0; i < _weaponButton.Length; i++)
        {
            if (i < weaponDatas.Length)
            {
                _weaponButton[i].enabled = true;
                _resultUICs[i].SetUpInfo(weaponDatas[i], selectOreData);
                var num = i;
                _weaponButton[i].onClick.AddListener(() => SetSaveEnhanceData(weaponDatas, selectOreData, num));
                _weaponButton[i].onClick.AddListener(() => SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Select_Ingame"));
            }
            else
            {
                _weaponButton[i].enabled = false;
            }

        }

        for (int i = 0; i < _weaponImage.Length; i++)
        {
            _weaponImage[i].color = new Color(1, 1, 1, 1);
            if (weaponDatas.Length - 1 < i)
            {
                _weaponImage[i].sprite = null;
                _weaponNameText[i].text = "";
                _weaponImage[i].color = new Color(1, 1, 1, 0);
            }
            else if (weaponDatas[i].WeaponType == WeaponType.DualBlades)
            {
                _weaponNameText[i].text = "双剣";
                _weaponImage[i].sprite = _weaponSampleSprite[0];
            }
            else if (weaponDatas[i].WeaponType == WeaponType.Hammer)
            {
                _weaponNameText[i].text = "ハンマー";
                _weaponImage[i].sprite = _weaponSampleSprite[1];
            }
            else if (weaponDatas[i].WeaponType == WeaponType.GreatSword)
            {
                _weaponNameText[i].text = "大剣";
                _weaponImage[i].sprite = _weaponSampleSprite[2];
            }
            else if (weaponDatas[i].WeaponType == WeaponType.Spear)
            {
                _weaponNameText[i].text = "槍";
                _weaponImage[i].sprite = _weaponSampleSprite[3];
            }
        }

    }

    public void SetSaveEnhanceData(WeaponData[] weaponDatas, OreData oredata, int selectButton)
    {
        _resultUICs[_currentSelectWeapon].ActiveOutLine(false);
        _saveWeaponData = weaponDatas;
        _selectOreData = oredata;
        _currentSelectWeapon = selectButton;
        _resultUICs[_currentSelectWeapon].ActiveOutLine(true);
    }


    /// <summary>
    /// 武器の強化したときのButtonEvent
    /// </summary>
    public void WeaponEnhanceEvent()
    {
        var weaponSkill = _saveWeaponData[_currentSelectWeapon].WeaponSkill;
        if (!_selectOreData.Skill || _selectOreData.Skill.Weapon != _saveWeaponData[_currentSelectWeapon].WeaponType)
        {
            Debug.Log("SkillがNull");
            _saveWeaponData[_currentSelectWeapon].EnhanceParam(_selectOreData.EnhancedData);
            _actorGenerator.PlayerController.SavePlayerData();
            SceneLoader.LoadScene(_homeScene);
            return;
        }

        if (_selectOreData.Skill.Type == SkillType.Skill || _selectOreData.Skill.Type == SkillType.Epic)
        {
            if (weaponSkill.GetSkillData() == weaponSkill.WeaponSkillArray.Length)
            {
                Debug.Log("スキルの追加");
                //スキルが追加できなかったときPlayerに選択させる
                _enhanceSelectObj.SetActive(false);
                _skillSelectPanel.SetActive(true);
                _skillSelectButtonCs[0].SetCurrentSkill(_skillDataManagement.SearchSkill(weaponSkill.WeaponSkillArray[0]));
                _skillSelectButtonCs[1].SetCurrentSkill(_skillDataManagement.SearchSkill(weaponSkill.WeaponSkillArray[1]));
                _skillSelectButton[0].onClick.AddListener(() =>
                {
                    ChangeSkill(0, _selectOreData.Skill, _saveWeaponData[_currentSelectWeapon]);
                    _saveWeaponData[_currentSelectWeapon].EnhanceParam(_selectOreData.EnhancedData);

                });
                _skillSelectButton[1].onClick.AddListener(() =>
                {
                    ChangeSkill(1, _selectOreData.Skill, _saveWeaponData[_currentSelectWeapon]);
                    _saveWeaponData[_currentSelectWeapon].EnhanceParam(_selectOreData.EnhancedData);
                });
            }
            else
            {
                var result = weaponSkill.AddSkillJudge(_selectOreData.Skill);
                _saveWeaponData[_currentSelectWeapon].EnhanceParam(_selectOreData.EnhancedData);
                //スキルを追加出来たときの処理
                if (_isBlacksmith)
                {
                    BlacksmithJudge();
                }
                else
                {
                    _actorGenerator.PlayerController.SavePlayerData();
                    SceneLoader.LoadScene(_homeScene);
                }

                if (!result)
                {
                    Debug.Log("武器の種類が合わなかったため追加できませんでした");
                }
            }
        }
        else if (_selectOreData.Skill.Type == SkillType.Special)
        {
            if (weaponSkill.AddSpecialSkill(_selectOreData.Skill.SkillName))
            {
                _saveWeaponData[_currentSelectWeapon].EnhanceParam(_selectOreData.EnhancedData);
                //スキルを追加出来たときの処理
                if (_isBlacksmith)
                {
                    BlacksmithJudge();
                }
                else
                {
                    _actorGenerator.PlayerController.SavePlayerData();
                    SceneLoader.LoadScene(_homeScene);
                }
            }
            else
            {
                ChangeSpecialSkill(_selectOreData.Skill, _saveWeaponData[_currentSelectWeapon]);
                _saveWeaponData[_currentSelectWeapon].EnhanceParam(_selectOreData.EnhancedData);
            }
        }
        // SceneLoader.LoadScene(_homeScene);
    }

    public void EnterSound()
    {
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Enter");
    }
    public void CancelSelectSkill()
    {
        _enhanceSelectObj.SetActive(true);
        _skillSelectPanel.SetActive(false);
    }

    /// <summary>
    /// 倒した敵がボスだったら武器をゲットする
    /// </summary>
    public void BlacksmithJudge()
    {
        _enhanceSelectObj.SetActive(false);
        _skillSelectPanel.SetActive(false);
        _blacksmithSelectPanel.SetActive(true);
        _actorGenerator.PlayerController.SavePlayerData();
        _blackSmithSelectButton[0].onClick.AddListener(() => SceneLoader.LoadScene(_blacksmithSceneName));
        _blackSmithSelectButton[1].onClick.AddListener(() => SceneLoader.LoadScene(_homeScene));
    }

    /// <summary>
    /// 持っていない武器を探して次のシーンへ移る
    /// </summary>
    public void FindWeaponTypeDontHave()
    {
        var playerStatus = _actorGenerator.PlayerController.PlayerStatus;

        for (int i = 0; i < Enum.GetValues(typeof(WeaponType)).Length; i++)
        {
            var judge = false;
            WeaponType weaponType = (WeaponType)Enum.GetValues(typeof(WeaponType)).GetValue(i);
            for (int j = 0; j < playerStatus.WeaponDatas.Length; j++)
            {
                if (playerStatus.WeaponDatas[j].WeaponType == weaponType)
                {
                    judge = true;
                }
            }

            if (!judge)
            {
                AddWeapon(weaponType);
                _isBlacksmith = true;
                GameManager.SetBlacksmithType((WeaponType)Enum.GetValues(typeof(WeaponType)).GetValue(i));
                break;
                //SceneLoader.LoadScene(_blacksmithSceneName);
            }
        }
    }

    public void ChangeSkill(int num, SkillBase skill, WeaponData weaponData)
    {
        weaponData.WeaponSkill.WeaponSkillArray[num] = skill.SkillName;
        Debug.Log($"{skill}に変更しました");

        if (_isBlacksmith)
        {
            BlacksmithJudge();
        }
        else
        {
            _actorGenerator.PlayerController.SavePlayerData();
            SceneLoader.LoadScene(_homeScene);
        }
    }

    public void ChangeSpecialSkill(SkillBase skill, WeaponData weaponData)
    {
        weaponData.WeaponSkill.ChangeSpecialSkill(skill.SkillName);
        if (_isBlacksmith)
        {
            BlacksmithJudge();
        }
        else
        {
            _actorGenerator.PlayerController.SavePlayerData();
            SceneLoader.LoadScene(_homeScene);
        }

    }

    public void SelectAgain()
    {
        _oreSelectObj.SetActive(true);
        _enhanceSelectObj.SetActive(false);
    }

    private void AddWeapon(WeaponType weaponType)
    {
        var playerStatus = _actorGenerator.PlayerController.PlayerStatus;
        if (_actorGenerator.PlayerController.PlayerStatus.WeaponDatas.Length > 3) 
        {
            return;
        } 
        WeaponData[] weapons = new WeaponData[playerStatus.WeaponDatas.Length + 1];
        for (int i = 0; i < playerStatus.WeaponDatas.Length; i++)
        {
            weapons[i] = playerStatus.WeaponDatas[i];
        }

        weapons[playerStatus.WeaponDatas.Length] = new(1000, 1000, 50, 1000, WeaponData.AttributeType.None, weaponType);
        playerStatus.ChangeWeponArray(weapons); 

    }
}
