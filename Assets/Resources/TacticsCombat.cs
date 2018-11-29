using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsCombat : MonoBehaviour {

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;
    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;
    int attackRange;
    int defense;
    int offense;

    // Use this for initialization
    void Start () {
        //range = Unit.range;
	}
	
	//Use to find enemies within range
    void FindEnemies()
    {
        //Mehmet will work on at home
        //A breadth first search based off of range to find tiles with "NPC" tag.
        //Store location in a list allow player to select one unit to attack in said list or end turn.

        //After finding enemies, if player attacks
        Fight();

        //If player wishes to end turn
        TurnManager.EndTurn();
    }

    //Use to complete combat phase
    void Fight()
    {

    }
}
