using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MultiHits", menuName = "Card Effects/HitsData")]
[System.Serializable]
public class HitData
{
    public int startupKe; // 出招所需的刻数
    public int activeKe;  // 命中判定所需的刻数
    public int damage; // 伤害值
    public GameObject attackColliderPrefab; // 每段攻击对应的碰撞体
    public List<CardEffect> startEffects; //每段起手效果
    public List<CardEffect> hitEffects; // 每段攻击的效果
}
