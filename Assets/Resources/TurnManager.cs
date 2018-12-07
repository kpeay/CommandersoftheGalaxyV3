using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	
        if (turnTeam.Count == 0)
        {
            Debug.Log("All TEAM members COMPLETED their turn.");
            InitTeamTurnQueue();
        }
	}

    static void InitTeamTurnQueue()
    {
        List<TacticsMove> teamList = units[turnKey.Peek()];
        Debug.Log("New Team members QUEUED for turn...");
        List<TacticsMove> tempList = new List<TacticsMove>(teamList);
        foreach (TacticsMove unit in tempList)
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    public static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
            Debug.Log("Start of TURN given to unit " + turnTeam.Peek().name);
        }
    }

    public static void EndTurn()
    {
        
        TacticsMove unit = turnTeam.Dequeue();
        Debug.Log("End of TURN declared by unit " + unit.name);
        unit.EndTurn();

        Debug.Log("Number of team units waiting for TURN " + turnTeam.Count);
        if (turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    // Add unit to Turn Manager data structures
    public static void AddUnit(TacticsMove unit)
    {
        List<TacticsMove> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<TacticsMove>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
        Debug.Log("Unit " + unit.name + " added to team " + unit.tag);
    }

    public static void RemoveUnit(GameObject unit)
    {
        string teamTag = unit.tag;
        Debug.Log("***RemoveUnit entered for Team " + teamTag + " Unit " + unit.name);

        List<TacticsMove> teamList = units[teamTag];
        /*
        Debug.Log("***Team " + teamTag + " Members BEFORE removing unit ");
        foreach (TacticsMove mem in teamList)
        {
            Debug.Log("***Team " + teamTag + " Unit " + mem.name);
        }
        */

        TacticsMove abc = unit.GetComponentInParent<TacticsMove>();
        teamList.Remove(abc);
        units[teamTag] = teamList;
        /*
        Debug.Log("***Team " + teamTag + " Members AFTER removing unit ");
        foreach (TacticsMove mem in teamList)
        {
            Debug.Log("***Team " + teamTag + " Unit " + mem.name);
        }
        */

        if (teamList.Count < 1)
        {
            if (teamTag == "Player")
            {   // Player lost the game
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {   // Player Win the Game
                SceneManager.LoadScene("WinScene");
            }
        }
    }

    public static List<TacticsMove> GetTeamList(string teamName)
    {
        return units[teamName];
    }
}
