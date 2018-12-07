using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public string unitTag = "null";
    public GameObject unitObject = null;

    int attack;
    int defense;

    public List<Tile> adjacencyList = new List<Tile>();

    //Needed BFS (Breadth First Search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    //for A*
    public float f = 0;
    public float g = 0;
    public float h = 0;

    // Use this for initialization
    void Start () {
        GetUnitObject();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Get information above tile
        unitTag = GetUnit();
        if (unitTag != "null") {
            if (unitObject.GetComponent<Unit>().tileBelow != this.GetComponent<Tile>())
            {
                //debug. check if tiles are correctly stored if not, notify console and try to set it correctly
                Debug.Log("**************************************************************************************************************************");
                Debug.Log("TILE and OBJECT were NOT stored correctly! Current Unit.tileBelow on " + unitObject + " is: " + unitObject.GetComponent<Unit>().tileBelow);

                unitObject.GetComponent<Unit>().tileBelow = this.GetComponent<Tile>();

                Debug.Log("TILE and OBJECT were NOT stored correctly! Now current Unit.tileBelow on " + unitObject + " is: " + unitObject.GetComponent<Unit>().tileBelow);
                Debug.Log("This TILE is: " + this.GetComponent<Tile>());
                Debug.Log("**************************************************************************************************************************");
            }
        }

	    if (current)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (!walkable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
	}

    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;

    }

    public void FindNeighbors(float jumpHeight, Tile target)
    {
        Reset();

        Checktile(Vector3.forward, jumpHeight, target);
        Checktile(-Vector3.forward, jumpHeight, target);
        Checktile(Vector3.right, jumpHeight, target);
        Checktile(-Vector3.right, jumpHeight, target);
    }

    //Gets the tag of the object above tile
    public string GetUnit()
    {

        //Create raycast
        RaycastHit hit;
     
        //Check to see what is above tile. If object is above, return tag. 
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
        {
            if(hit.collider.tag == "NPC")
            {
                unitObject = hit.collider.gameObject;

                //Debug.Log("NPC");
                return "NPC";

            }
            else if(hit.collider.tag == "Player")
            {
                unitObject = hit.collider.gameObject;
                //Debug.Log("Player");
                return "Player";
            }
           
        }

        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
        {
            return "null";
        }




        return unitTag;

    }

    public void Checktile (Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }

    public GameObject GetUnitObject()
    {
        GetUnit();
        return unitObject;
    }

}
