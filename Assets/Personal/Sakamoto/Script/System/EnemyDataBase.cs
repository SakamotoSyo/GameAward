using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDataBase : MonoBehaviour
{
   [SerializeField] private List<EnemyData> _enemyDataList = new();
   

    /// <summary>
    /// �����N���߂������_����Enemy��Data��Ԃ�
    /// </summary>
    /// <returns></returns>
    public EnemyData GetRandomEnemyData(int playerRank, int RankRange) 
    {
        var rankList = _enemyDataList.Where(x => playerRank - RankRange <= x.RankPoint 
                                           && playerRank + RankRange >= x.RankPoint).ToList();
        return rankList[Random.Range(0, rankList.Count)];
    }

}
