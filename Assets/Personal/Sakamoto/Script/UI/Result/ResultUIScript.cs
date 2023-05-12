using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class ResultUIScript : MonoBehaviour
{
    [SerializeField] private OreUIScript[] _oreUiCs = new OreUIScript[3];
    [SerializeField] private Button[] _oreButton = new Button[3];
    [SerializeField] private Button[] _weaponButton = new Button[4];
    [SerializeField] private Button[] _skillSelectButton = new Button[2];
    [SerializeField] private Button[] _blackSmithSelectButton = new Button[2];
    [SerializeField] private GameObject _oreSelectObj;
    [SerializeField] private GameObject _enhanceSelectObj;
    [SerializeField] private GameObject _skillSelectPanel;
    [SerializeField] private GameObject _blacksmithSelectPanel;
    [SerializeField] private SkillSelectButtonScript[] _skillSelectButtonCs = new SkillSelectButtonScript[2];
    [SerializeField] private ActorGenerator _actorGenerator;
    [SerializeField] private SkillDataManagement _playerSkillDataManagement;
    [SerializeField] private Sprite[] _oreImage = new Sprite[3];
    [SerializeField] private ResultWeaponButton[] _resultUICs = new ResultWeaponButton[4];
    [SceneName]
    [SerializeField] private string _blacksmithSceneName;
    [Tooltip("���U���g�V�[���̃e�X�g�p")]
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

        if (_actorGenerator.EnemyController.EnemyStatus.IsBoss)
        {
            FindWeaponTypeDontHave();
        }
    }

    void Update()
    {

    }
    public void StartResultLottery()
    {
        _oreSelectObj.SetActive(true);
        RewardLottery();
    }

    /// <summary>
    /// ��V�̒��I�����ăf�[�^��n��
    /// </summary>
    private void RewardLottery()
    {
        _actorGenerator.PlayerController.PlayerStatus.AddRankPoint();
        for (int i = 0; i < _oreUiCs.Length; i++)
        {
            var oreInfo = RarityLottery();
            OreData Ore = new OreData(SetEnhanceData(oreInfo.rearity), oreInfo.rearity, oreInfo.randomSkill, oreInfo.oreImage);
            _oreUiCs[i].SetData(Ore);
            _oreButton[i].onClick.AddListener(() => EnhanceWeaponEvent(Ore));
        }
    }

    /// <summary>
    /// ��������Status��Ԃ��Ă����
    /// </summary>
    /// <returns></returns>
    private EnhanceData[] SetEnhanceData(OreRarity oreRarity)
    {
        EnhanceData[] enhanceData = new EnhanceData[DataBaseScript.DescriptionEnhanceData.Count];
        for (int i = 0; i < DataBaseScript.DescriptionEnhanceData.Count; i++)
        {
            //���̃X�e�[�^�X���������邩�ǂ���
            int enhanceBool = Random.Range(0, 2);
            if (enhanceBool == 0)
            {
                //�������鐔��
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

    private (Sprite oreImage, OreRarity rearity, SkillBase randomSkill) RarityLottery()
    {
        SkillBase randomSkill;
        OreRarity rearity = (OreRarity)Random.Range(0, 2);
        WeaponData[] weaponDatas = _actorGenerator.PlayerController.PlayerStatus.WeaponDatas;
        WeaponData weaponData = weaponDatas[Random.Range(0, weaponDatas.Length)];
        var randomNum = Random.Range(0, 100);
        if (20 < randomNum)
        {
            randomSkill = _playerSkillDataManagement.OnSkillCall(weaponData.WeaponType, SkillType.Skill);
        }
        else
        {
            randomSkill = _playerSkillDataManagement.OnSkillCall(weaponData.WeaponType, SkillType.Special);
        }

        Sprite oreImage = _oreImage[(int)rearity];

        return (oreImage, rearity, randomSkill);
    }

    /// <summary>
    ///�z�΂�I�񂾍ۂɂǂ̕���ɍz�΂������邩�I���ł���Event�Ɉڂ�
    /// </summary>
    public void EnhanceWeaponEvent(OreData selectOreData)
    {
        _oreSelectObj.SetActive(false);
        _enhanceSelectObj.SetActive(true);
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
                _weaponButton[i].onClick.AddListener(() => WeaponEnhanceEvent(weaponDatas[num], selectOreData));
            }
            else
            {
                _weaponButton[i].enabled = false;
            }

        }

    }


    /// <summary>
    /// ����̋��������Ƃ���ButtonEvent
    /// </summary>
    public void WeaponEnhanceEvent(WeaponData weaponData, OreData oreData)
    {
        var playerSkill = _actorGenerator.PlayerController.PlayerSkill;
        if (!oreData.Skill) return;
        if (oreData.Skill.Type == SkillType.Skill)
        {
            if (playerSkill.AddSkillJudge(oreData.Skill))
            {
                weaponData.EnhanceParam(oreData.EnhancedData);
                //�X�L����ǉ��o�����Ƃ��̏���
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
                //�X�L�����ǉ��ł��Ȃ������Ƃ�Player�ɑI��������
                _enhanceSelectObj.SetActive(false);
                _skillSelectPanel.SetActive(true);
                _skillSelectButtonCs[0].SetCurrentSkill(playerSkill.PlayerSkillArray[0]);
                _skillSelectButtonCs[1].SetCurrentSkill(playerSkill.PlayerSkillArray[1]);
                _skillSelectButton[0].onClick.AddListener(() =>
                {
                    ChangeSkill(0, oreData.Skill);
                    weaponData.EnhanceParam(oreData.EnhancedData);

                });
                _skillSelectButton[1].onClick.AddListener(() =>
                {
                    ChangeSkill(1, oreData.Skill);
                    weaponData.EnhanceParam(oreData.EnhancedData);
                });
            }
        }
        else if (oreData.Skill.Type == SkillType.Special)
        {
            if (playerSkill.AddSpecialSkill(oreData.Skill))
            {
                weaponData.EnhanceParam(oreData.EnhancedData);
                //�X�L����ǉ��o�����Ƃ��̏���
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
                Debug.Log("�K�E�Z");
                _enhanceSelectObj.SetActive(false);
                _skillSelectPanel.SetActive(true);
                _skillSelectButtonCs[0].SetCurrentSkill(playerSkill.SpecialAttack);
                _skillSelectButtonCs[1].SetCurrentSkill(oreData.Skill);
                _skillSelectButton[0].onClick.AddListener(() =>
                {
                    ChangeSpecialSkill(oreData.Skill);
                    weaponData.EnhanceParam(oreData.EnhancedData);

                });
                _skillSelectButton[1].onClick.AddListener(() =>
                {
                    weaponData.EnhanceParam(oreData.EnhancedData);
                    SceneLoader.LoadScene(_homeScene);
                });
            }
        }
        // SceneLoader.LoadScene(_homeScene);
    }

    public void CancelSelectSkill()
    {
        _enhanceSelectObj.SetActive(true);
        _skillSelectPanel.SetActive(false);
    }

    /// <summary>
    /// �|�����G���{�X�������畐����Q�b�g����
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
    /// �����Ă��Ȃ������T���Ď��̃V�[���ֈڂ�
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
                //SceneLoader.LoadScene(_blacksmithSceneName);
            }
        }
    }

    public void ChangeSkill(int num, SkillBase skill)
    {
        _actorGenerator.PlayerController.PlayerSkill.PlayerSkillArray[num] = skill;
        Debug.Log($"{skill}�ɕύX���܂���");

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

    public void ChangeSpecialSkill(SkillBase skill)
    {
        _actorGenerator.PlayerController.PlayerSkill.ChangeSpecialSkill(skill);
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
        for (int i = 0; i < playerStatus.WeaponDatas.Length; i++)
        {
            if (playerStatus.WeaponDatas[i] == null)
            {
                playerStatus.WeaponDatas[i] = new(1000, 1000, 50, 1000, WeaponData.AttributeType.None, weaponType);
            }
        }
    }
}
