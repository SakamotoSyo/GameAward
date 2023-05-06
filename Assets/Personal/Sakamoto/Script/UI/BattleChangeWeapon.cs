using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleChangeWeapon : MonoBehaviour
{
    [SerializeField] private ActorGenerator _generator;
    [SerializeField] private GameObject _weaponUiPanel;
    //private BattleChangeWeaponButton[] _weaponUiButton = new BattleChangeWeaponButton[4];
    [SerializeField] private Button[] _weaponButton = new Button[4];
    [SerializeField] private Text[] _weaponTypeText = new Text[4];
    [SerializeField] private BattleStateController _battleStateController;
    private GameObject _buttonInsPos;
    private PlayerStatus _playerStatus;

    private void Start()
    {
        _playerStatus = _generator.PlayerController.PlayerStatus;
    }

    /// <summary>
    /// ��������ւ��邽�߂�UI��\��
    /// </summary>
    public void ChangeWeaponUiOpen()
    {
        _weaponUiPanel.SetActive(true);
        for (int i = 0; i < _weaponButton.Length; i++)
        {
            if (i < _playerStatus.WeaponDatas.Length - 1)
            {
                var num = i;
                _weaponButton[i].interactable = true;
                _weaponTypeText[i].text = _playerStatus.WeaponDatas[num].WeaponType.ToString();  
                _weaponButton[i].onClick.AddListener(() => OnClickChangeWeapon(_playerStatus.WeaponDatas[num]));
                _weaponButton[i].onClick.AddListener(() => _battleStateController.ActorStateEnd());
            }
            else
            {
                _weaponButton[i].interactable = false;
            }
        }
    }

    /// <summary>
    /// �{�^���������ꂽ�Ƃ��̓���ւ�����
    /// </summary>
    /// <param name="weaponData">����̃f�[�^</param>
    public void OnClickChangeWeapon(WeaponData weaponData)
    {
        _playerStatus.EquipWeponChange(weaponData);
        _weaponUiPanel.SetActive(false);
    }
}