using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class DataBaseScript : MonoBehaviour
{
    public static List<EnhanceData> DescriptionEnhanceData => _enhanceData;
    public static Dictionary<WeaponType, string> WeaponDescriptionData => _weaponDescription;

    [Header("���x���A�b�v�f�[�^")]
    [SerializeField] private TextAsset _levelUpTable;
    [SerializeField] private TextAsset _descriptionData;
    [SerializeField] private TextAsset _weaponDescriptionData;
    private static Dictionary<WeaponType, string> _weaponDescription = new Dictionary<WeaponType, string>();
    private static Dictionary<int, int> _statusData = new Dictionary<int, int>();
    private static List<EnhanceData> _enhanceData = new();

    private void Awake()
    {
        //LoadingLevelData();
        if (_enhanceData.Count == 0)
        {
            SetDescription();
        }

    }

    /// <summary>
    /// �V�[���̏��߂Ƀ��x���A�b�v�̃f�[�^��ǂݍ���
    /// </summary>
    private void LoadingLevelData()
    {
        //�e�L�X�g�̓ǂݍ���
        StringReader sr = new StringReader(_levelUpTable.text);
        //�ŏ��̈�s�ڂ̓X�L�b�v
        sr.ReadLine();

        while (true)
        {
            //��s���ǂݍ���
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            string[] parts = line.Split(',');
            int table = int.Parse(parts[1]);

            int level = int.Parse(parts[0]);
            _statusData.Add(level, table);
        }
    }

    private void SetDescription()
    {
        StringReader sr = new StringReader(_descriptionData.text);
        //�ŏ��̈�s�ڂ̓X�L�b�v
        sr.ReadLine();

        while (true)
        {
            //��s���ǂݍ���
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            _enhanceData.Add(new EnhanceData(0, line));
        }
    }

    private void SetWeaponTypeDescription()
    {
        //�e�L�X�g�̓ǂݍ���
        StringReader sr = new StringReader(_weaponDescriptionData.text);
        //�ŏ��̈�s�ڂ̓X�L�b�v
        sr.ReadLine();

        while (true)
        {
            //��s���ǂݍ���
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            string[] parts = line.Split(',');
            var weaponType = Enum.Parse<WeaponType>(parts[0]);
            string description = "";

            for (int i = 1; i < parts.Length; i++)
            {
                if (i != 1)
                {
                    description += "\n";
                }

                description += parts[i];
            }
            _weaponDescription.Add(weaponType, description);
        }

    }

    /// <summary>
    /// ���x���ɑ΂��Ă̎��̃��x���A�b�v�܂ł̌o���l��Ԃ��Ă����
    /// </summary>
    /// <param name="Level"></param>
    /// <returns></returns>
    public static int GetMaxExperienceAmount(int Level)
    {
        return _statusData[Level];
    }

}
