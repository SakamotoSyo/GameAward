﻿using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyController : MonoBehaviour, IAddDamage
{

    [SerializeField, Tooltip("ダメージテキストのクラス")]
    private DamageTextController _damegeController;

    [SerializeField, Tooltip("ダメージテキストを生成する座標")]
    private Transform _damagePos;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private GameObject _canvasObj;

    private Animator _animator;

    private EnemyAttack _enemyAttack = new();

    private EnemyAnimation _enemyAnimation = new();

    private EnemyStatus _enemyStatus;

    public EnemyStatus EnemyStatus => _enemyStatus;

    private EnemyColor _enemyColor;
    private bool _isClear = false;

    public EnemyColor EnemyColor => _enemyColor;

    private void Start()
    {
        _enemyAttack.Init(_enemyStatus.EquipWeapon);
        _animator = GetComponent<Animator>();
        Debug.Log($"武器タイプは{_enemyStatus.EquipWeapon.WeaponType}");
    }

    private void Update()
    {
        if (_isClear) return;
        _animator.SetBool(_enemyStatus.EquipWeapon.WeaponType.ToString(), true);
    }

    /// <summary>
    /// 攻撃のメソッドを呼ぶ
    /// </summary>
    public async UniTask Attack()
    {
        if (!_enemyStatus.IsStan)
        {
            await _enemyAttack.SelectAttack();
            await UniTask.Delay(1);
        }
    }

    public async UniTask AddDamage(float damage, float criticalRate)
    {
        float x = Random.Range(0, 4);
        float y = Random.Range(-3, 2);
        int critical = Random.Range(0, 100);
        Vector3 pos = _damagePos.position;
        pos.x += x;
        pos.y += y;
        var damageController = Instantiate(_damegeController,
            pos,
            Quaternion.identity);

        if (critical >= criticalRate)
        {
            damage *= 1.3f;
            damageController.TextInit((int)damage, true);
        }
        else
        {
            damageController.TextInit((int)damage, false);
        }

        _enemyStatus.EquipWeapon.AddDamage((int)damage);
        _animator.Play("DamageHit");
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Damage");

        if (_enemyStatus.EquipWeapon.IsWeaponBreak())
        {
            SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Crash");
            if (await _enemyStatus.IsWeaponsAllBrek())
            {
                Debug.Log("全部壊れた");
                return;
            }
            _enemyStatus.EquipChangeWeapon();
        }
    }

    /// <summary>
    /// EnemyStatusを参照する
    /// </summary>
    public void SetEnemyStatus(EnemyStatus enemyStatus)
    {
        _enemyStatus = enemyStatus;
    }

    public void StatusActive()
    {
        Debug.Log("Canvas");
        _canvasObj.SetActive(true);
    }

    /// <summary>
    /// EnemyDateから参照してくる
    /// </summary>
    public void SetEnemyData(EnemyData enemyData)
    {
        _enemyStatus.SetWeaponDates(enemyData);
        //_animator = enemyData.EnemyAnim;
        //_renderer.sprite = enemyData.EnemySprite;
        _enemyColor = enemyData.EnemyColor;
    }

    public void Lose() 
    {
        Debug.Log("負け");
        _isClear = true;
        _animator.SetBool(_enemyStatus.EquipWeapon.WeaponType.ToString(), false);
        _animator.SetBool("Lose", true);
    }
}
