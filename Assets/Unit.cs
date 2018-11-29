using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public int attack = 4;
    public int defense = 4;
    public int move = 5;
    public int range = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
     

	}

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefense()
    {
        return defense;
    }
    public int GetMove()
    {
        return move;
    }
    public int GetRange()
    {
        return range;
    }
}
