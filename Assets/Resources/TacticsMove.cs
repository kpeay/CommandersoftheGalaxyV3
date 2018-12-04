using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{
    public bool turn = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles; // Array of tiles as game objects

    Stack<Tile> path = new Stack<Tile>();
    Tile currentTile;

    public static bool moving = false;
    public static bool willAttackAfterMove = false;
    public static bool startAttack = false;
    public static bool attacking = false;
    public bool newUnitTurn = false;

    bool turnActive;
    public int move = 5;                    // Number of tiles to move
    public int range = 1;
    public float jumpHeight = 2;
    public float moveSpeed = 2;             // How fast unit will walk across tiles
    public float jumpVelocity = 4.5f;

    Vector3 velocity = new Vector3();       // How fast units moves fro tile to tile
    Vector3 heading = new Vector3();        // Direction unit is heading towards

    float halfHeight = 0;

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;

    public Tile actualTargetTile;

    protected void Init()
    {   // Get all tiles into an array
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        //Debug.Log("Number tiles " + tiles.Length);

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }

    // Gets current tile under the Player/NPC.
    // This will be the starting point for path finding.
    public void GetCurrentTile()
    {
        Debug.Log("Game Object: " + gameObject);
        currentTile = GetTargetTile(gameObject);
        Debug.Log("Current Tile:" + currentTile);

    }

    // Gets the Tile of game object named target
    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        Debug.Log("transform.position:" + target.transform.position);
        Debug.DrawRay(transform.position, -Vector3.up, Color.magenta);
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1000))
        {
            Debug.Log("Raycast Hit");

         tile = hit.collider.GetComponent<Tile>();
        }
        Debug.Log("GetTargetTile() return:" + tile);
            return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeight, Tile target)
    {
        //tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight, target);
        }
    }

    public List<Tile> FindSelectableTiles(GameObject unit)
    {
        string unitTag = unit.tag;
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
        return selectableTiles;
    }

    public void FindEnemies()
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = ?? leave as null

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < range)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        Debug.Log("Tile in MoveToTile():" + tile);
        if (tile == null)
        {
            TurnManager.EndTurn(); //tee hee
        }
        else
        {
            tile.target = true;
            moving = true;

            Tile next = tile;
            while (next != null)
            {
                path.Push(next);
                next = next.parent;
            }
        }
    }

    public void MoveToSelectableNeighborTile(Tile tile, float range, GameObject unit)
    {
        Debug.Log("Range is: " + range);
        //on the case that range is greater than 1
        if (range > 1)
        {
            //create a stack to hold all the tiles adjacent to tile up till the point that i >= range
            List<Tile> tileInRange = new List<Tile>();

            //create a queue that is used to store tiles that need to be used to find adjacent tiles
            Queue<Tile> tileQueue = new Queue<Tile>();

            List<Tile> selectableT = new List<Tile>();

            //store tile in the queue and stack. This should be the location that the targeted enemy is standing on.
            tileQueue.Enqueue(tile);

            //find tiles adjacent to origin tile. then find tiles adjacent to those tiles. continue this process until the distance from the last tile in the stack is = range
            for (int i = 0; i < range * 100; i++)
            {
                //take the next tile in the queue and set it at the tile
                tile = tileQueue.Dequeue();

                //store the tiles adjacent to tile into the queue and stack
                foreach (Tile t in tile.adjacencyList)
                {
                    tileInRange.Add(t);
                    tileQueue.Enqueue(t);
                }
                Debug.Log("Times searched through tiles: " + (i + 1));
            }


            foreach(Tile target in tileInRange)
            {
                RaycastHit hit;

                if (!Physics.Raycast(target.transform.position, Vector3.up, out hit, 1))
                {
                    //find the first selectable tile in the stack and move to it
                    if (target.selectable)
                    {
                        selectableT.Add(target);
                        if (unit.tag == "NPC")
                        {
                            Debug.Log("unit.Tag = 'NPC' Target: " + target);
                            MoveToTile(target);
                        }
                       // MoveToTile(target);
                    }

                }
            }

            Tile destination = tile; 
            

            foreach(Tile t in selectableT)
            {
                //find the tile closest to the unit's current location
                if(Vector3.Distance(t.transform.position, unit.transform.position) < Vector3.Distance(destination.transform.position, unit.transform.position))
                {
                    destination = t;
                }
            }
            Debug.Log("Destination:" + destination);
            //move to tile. should be the furthest possible tile away from the enemy while still being in range
            MoveToTile(destination); 

        }
        else{
            // Find next selectable empty neighbor tile
            foreach (Tile t in tile.adjacencyList)
            {
                RaycastHit hit;

                if (!Physics.Raycast(t.transform.position, Vector3.up, out hit, 1))
                {
                    if (t.selectable)
                    {
                        Debug.Log("t:" + t);
                        MoveToTile(t);
                        return;
                    }
                }
            }
        }
    }

    public void DontMove()
    {
        path.Clear();
        RemoveSelectableTiles();
        moving = false;

        if (willAttackAfterMove)    // Will we attack enemy
        {
            // Set switch to start attack
            startAttack = true;
            attacking = true;
            PlayerMove.playerAttacking = true;
        }
        else
        {   // No attacking. Therefore, give up my turn.
            TurnManager.EndTurn();
        }
    }

    public void Move(GameObject objectRef)
    {
        //Debug.Log("-----Move entered-----------");
        //Debug.Log("fath.Count " + path.Count);
        if(objectRef.tag == "NPC")
        {
            //objectRef.GetComponent<NPCMove>().animateMove = true;
        }

        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //calculate the unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                bool jump = transform.position.y != target.y;

                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizontalVelocity();
                }
                //Locomotion
                //PlayerMove.Anim();
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Tile center reached
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;

            //Debug.Log("willAttackAfterMove " + willAttackAfterMove);

            // Movement to target tile ended.
            if (willAttackAfterMove)    // Will we attack enemy
            {
                if (objectRef.tag == "Player")
                    objectRef.GetComponent<PlayerMove>().animateMove = false;
                if (objectRef.tag == "NPC")
                    //objectRef.GetComponent<NPCMove>().animateMove = false;
                // Set switch to start attack
                startAttack = true;
                attacking = true;
                PlayerMove.playerAttacking = true;
            }
            else
            {   // No attacking. Therefore, give up my turn.
                if(objectRef.tag == "Player") { 
                objectRef.GetComponent<PlayerMove>().animateMove = false;
                }

                if (objectRef.tag == "NPC")
                {
                    //objectRef.GetComponent<NPCMove>().animateMove = false;
                }
                TurnManager.EndTurn();
            }
        }

    }

    protected void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        if(fallingDown)
        {
            FallDownward(target);
        }
        else if(jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);

        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {
            SetHorizontalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 3.0f;
            velocity.y = 1.5f;
        }
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i=0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(jumpHeight, target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        //currentTile.parent = ??
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    //Do nothing, already processed
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    tile.f = tile.g + tile.h;

                    openList.Add(tile);

                }
            }
        }

        //todo - what do you do if there is no path to the target tile?
        Debug.Log("Path not found");

    }

    public void BeginTurn()
    {
        turn = true;
        newUnitTurn = true;
    }

    public void EndTurn()
    {

        turn = false;
        moving = false;
    }
}
