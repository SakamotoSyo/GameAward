using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Action GameOverAction;
    public PlayerStatus PlayerStatus => _playerStatus;

    [SerializeField, Tooltip("�_���[�W�e�L�X�g�̃N���X")]
    private DamageTextController _damegeController;
    [SerializeField, Tooltip("�_���[�W�e�L�X�g�𐶐�������W")]
    private Transform _damagePos;
    private PlayerStatus _playerStatus;
    private PlayerAnimation _playerAnimation = new();
    private bool _isCounter = false;

    private void Start()
    {  
       
    }

    private void Update()
    {
       
    }

    /// <summary>
    /// �_���[�W���󂯂闬��
    /// </summary>
    public void AddDamage(float damage) 
    {
        var damageController = Instantiate(_damegeController,
          _damagePos.position,
           Quaternion.identity);
        damageController.TextInit((int)damage);

        //if (_playerSkill.SkillDataManagement.InEffectCheck) 
        //{ 
            
        //}
        if (_playerStatus.EquipWeapon.DownJudge(damage))
        {
            //�A�j���[�V�������������炱���Ń_���[�W���󂯂鏈�����Ă�
            _playerStatus.EquipWeapon.AddDamage(damage);
        }
        else 
        {
            _playerStatus.EquipWeapon.AddDamage(damage);
            //���킪��ꂽ�Ƃ��ɓ���ւ��鏈��
            if (!_playerStatus.RandomEquipWeponChange())
            {
                //GameOver�̏����͂�����
                Debug.Log("GameOver");
                GameOverAction?.Invoke();
            }
            else 
            {
                Debug.Log("����ւ��܂���");
            }
        }
        
    }

    /// <summary>
    /// �ʏ�U��
    /// </summary>
    public float Attack(PlayerAttackType attackType) 
    {
        switch (attackType) 
        {
            case PlayerAttackType.ConventionalAttack:
                return _playerStatus.ConventionalAttack();
                break;
           // case PlayerAttackType.Skill1:
             //   return
        }

        return 0;
    }

    public void SetPlayerStatus(PlayerStatus playerStatus) 
    {
        _playerStatus = playerStatus;
    }

    public void SavePlayerData() 
    {
        PlayerSaveData playerSaveData = new PlayerSaveData();
        _playerStatus.SaveStatus(playerSaveData);
        GameManager.SetPlayerData(playerSaveData);
        Debug.Log("Save����܂���");
    }

    public void LoadPlayerData(PlayerSaveData playerSaveData) 
    {
        _playerStatus.LoadStatus(playerSaveData);
    }
}

public enum PlayerAttackType 
{
    ConventionalAttack,
    CounterAttack,
}
