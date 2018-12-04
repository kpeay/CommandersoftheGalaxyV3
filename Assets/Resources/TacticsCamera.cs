using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticsCamera : MonoBehaviour
{

    public Text playerHealth;
    public Text npcHealth;

    // Use this for initialization
    void Start()
    {
        DisplayPlayerHealth("Player");
        DisplayNPCHealth("NPC");
    }

    // Update is called once per frame
    void Update()
    {
        DisplayPlayerHealth("Player");
        DisplayNPCHealth("NPC");
    }

    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, -90, Space.Self);
    }

    public void DisplayPlayerHealth(string unitTag)
    {
        List<TacticsMove> teamList = TurnManager.GetTeamList(unitTag);

        playerHealth.text = unitTag + " Team Members Healths\n";
        foreach (TacticsMove unit in teamList)
        {
            playerHealth.text += unit.name + " " + unit.GetComponent<Unit>().GetHealth() + "\n";
        }
    }

    public void DisplayNPCHealth(string unitTag)
    {
        List<TacticsMove> teamList = TurnManager.GetTeamList(unitTag);

        npcHealth.text = unitTag + " Team Members Healths\n";
        foreach (TacticsMove unit in teamList)
        {
            npcHealth.text += unit.name + " " + unit.GetComponent<Unit>().GetHealth() + "\n";
        }
    }

}
