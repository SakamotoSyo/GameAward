using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataBase : MonoBehaviour
{
   [SerializeField] private List<EnemyData> _enemyDataList = new();

    /// <summary>
    /// �����_����Enemy��Data��Ԃ�
    /// </summary>
    /// <returns></returns>
    public EnemyData GetRandomEnemyData() 
    {
        return _enemyDataList[Random.Range(0, _enemyDataList.Count)];
    }

}
