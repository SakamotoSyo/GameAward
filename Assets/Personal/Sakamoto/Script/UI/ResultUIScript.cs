using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIScript : MonoBehaviour
{
    [SerializeField] private OreUIScript[] _oreUiCs = new OreUIScript[3];
    [SerializeField] private Button[] _oreButton = new Button[3];
    [SerializeField] private Button[] _weaponButton = new Button[4];
    [SerializeField] private GameObject _oreSelectObj;
    [SerializeField] private GameObject _enhanceSelectObj;
    [SerializeField] private ActorGenerator _actorGenerator;
    [SerializeField] private PlayerSkillDataManagement _playerSkillDataManagement;
    [SerializeField] private Image[] _oreImage = new Image[3];
    [SerializeField] private ResultWeaponButton[] _resultUICs = new ResultWeaponButton[4];
    [Tooltip("���U���g�V�[���̃e�X�g�p")]
    [SerializeField] private bool _resultSceneTest = false;
    [SceneName]
    [SerializeField] private string _homeScene;


    void Start()
    {
        if (_resultSceneTest) 
        {
            StartResultLottery();
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
        for (int i = 0; i < _oreUiCs.Length; i++)
        {
            var oreInfo = RarityLottery();
            Debug.Log(oreInfo.randomSkill);
            OreData Ore = new OreData(SetEnhanceData(), oreInfo.rearity, oreInfo.randomSkill, oreInfo.oreImage);
            _oreUiCs[i].SetData(Ore);
            _oreButton[i].onClick.AddListener(() => EnhanceWeaponEvent(Ore));
        }
    }

    /// <summary>
    /// ��������Status��Ԃ��Ă����
    /// </summary>
    /// <returns></returns>
    private EnhanceData[] SetEnhanceData()
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
        }
        return enhanceData;
    }

    private (Image oreImage, OreRarity rearity, SkillBase randomSkill) RarityLottery() 
    {
       OreRarity rearity = (OreRarity)Random.Range(0, 3);
       //WeaponData[] weaponDatas = _actorGenerator.PlayerController.PlayerStatus.WeponDatas;
       //WeaponData weaponData = weaponDatas[Random.Range(0, weaponDatas.Length)];
       //TODO:�e�X�g�œ�������悤�ɂȂ��Ă���̂�Player�����������������������g��
       //WeaponType weaponType = (WeaponType)Random.Range(0, 4);
       SkillBase randomSkill = _playerSkillDataManagement.OnSkillCall(WeaponType.GreatSword, OreRarity.Normal);
       Image oreImage = _oreImage[(int)rearity];

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
            if (weaponDatas[i] != null)
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
        weaponData.EnhanceParam(oreData.EnhancedData);
        Debug.Log("�������܂���");
        if (oreData.Skill != null) 
        {

        }
        _actorGenerator.PlayerController.SavePlayerData();
        SceneLoader.LoadScene(_homeScene);
    }

    public void SelectAgain()
    {
        _oreSelectObj.SetActive(true);
        _enhanceSelectObj.SetActive(false);
    }
}
