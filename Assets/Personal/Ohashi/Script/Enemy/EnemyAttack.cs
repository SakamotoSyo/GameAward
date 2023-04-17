using UnityEngine;

public class EnemyAttack
{
    private float _offensivePower;

    public void Init(float offensivePower)
    {
        _offensivePower = offensivePower;
    }

    /// <summary>
    /// �U��
    /// </summary>
    public void NormalAttack(PlayerController playerController)
    {
        playerController.AddDamage(_offensivePower);
    }
}
