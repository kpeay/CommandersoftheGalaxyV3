using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTypes : MonoBehaviour {

    public static bool isPlayerHeroAttackType {get; set;}
    public static bool isPlayerHeroDefenseType {get; set;}
    public static bool isPlayerHeroRangeType {get; set;}

    public static bool isEnemyHeroAttackType {get; set; }
    public static bool isEnemyHeroDefenseType {get; set; }
    public static bool isEnemyHeroRangeType {get; set; }

    private void Start()
    {
        HeroTypes.isPlayerHeroAttackType = true;
        HeroTypes.isPlayerHeroDefenseType = false;
        HeroTypes.isPlayerHeroRangeType = false;

        HeroTypes.isEnemyHeroAttackType = true;
        HeroTypes.isEnemyHeroDefenseType = false;
        HeroTypes.isEnemyHeroDefenseType = false;
        Debug.Log("Player and Enemy Hero Type Set To ATTACK....");
    }

}
