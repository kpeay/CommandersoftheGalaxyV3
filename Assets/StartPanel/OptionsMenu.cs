using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    // Player Hero Types
    public Toggle isPlayerHeroAttack;
    public Toggle isPlayerHeroDefense;
    public Toggle isPlayerHeroRange;

    // Enemy Hero Types
    public Toggle isEnemyHeroAttack;
    public Toggle isEnemyHeroDefense;
    public Toggle isEnemyHeroRange;

    // Player Hero Attack Type Changed
    public void PlayerHeroTypeAttackChanged()
    {      
        HeroTypes.isPlayerHeroAttackType = isPlayerHeroAttack.isOn;
        Debug.Log("Player Hero Attack Type Changed To " + isPlayerHeroAttack.isOn);
    }

    // Player Hero Defense Type Changed
    public void PlayerHeroTypeDefenseChanged()
    {
        HeroTypes.isPlayerHeroDefenseType = isPlayerHeroDefense.isOn;
        Debug.Log("Player Hero Defense Type Changed To " + isPlayerHeroDefense.isOn);
    }

    // Player Hero Range Type Changed
    public void PlayerHeroTypeRangeChanged()
    {
        HeroTypes.isPlayerHeroRangeType = isPlayerHeroRange.isOn;
        Debug.Log("Player Hero Range Type Changed To " + isPlayerHeroRange.isOn);
    }

    // Enemy Hero Attack Type Changed
    public void EnemyHeroTypeAttackChanged()
    {
        HeroTypes.isEnemyHeroAttackType = isEnemyHeroAttack.isOn;
        Debug.Log("Enemy Hero Attack Type Changed To " + isEnemyHeroAttack.isOn);
    }

    // Enemy Hero Defense Type Changed
    public void EnemyHeroTypeDefenseChanged()
    {
        HeroTypes.isEnemyHeroDefenseType = isEnemyHeroDefense.isOn;
        Debug.Log("Enemy Hero Defense Type Changed To " + isEnemyHeroDefense.isOn);
    }

    // Enemy Hero Range Type Changed
    public void EnemyHeroTypeRangeChanged()
    {
        HeroTypes.isEnemyHeroRangeType = isEnemyHeroRange.isOn;
        Debug.Log("Enemy Hero Range Type Changed To " + isEnemyHeroRange.isOn);
    }

}
