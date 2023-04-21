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
    public EnemyData[] GetEnemyArrayData(int playerRank, int RankRange) 
    {
        var rankArray = _enemyDataList.Where(x => playerRank - RankRange <= x.RankPoint 
                                           && playerRank + RankRange >= x.RankPoint).ToArray();
        return rankArray;
    }

}
