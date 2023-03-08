using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface ISkill
{
    /// <summary>
    /// �X�L�����X�^�[�g������
    /// </summary>
    /// <returns></returns>
    public UniTask StartSkill();
    /// <summary>
    /// �X�L�����g�������ʂ�Ԃ�
    /// </summary>
    /// <returns></returns>
    public float SkillResult();
    /// <summary>
    /// �X�L�����I�������ɌĂ΂��֐�
    /// �O����Ă΂Ȃ��Ă悢
    /// </summary>
    public void SkillEnd();
}
