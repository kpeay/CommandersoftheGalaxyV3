using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject spawnPoint;
    public Transform obstacle;

    void Start()
    {
        if (spawnPoints == null)
            spawnPoints = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject spawnPoint in spawnPoints)
        {
            Instantiate(obstacle, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
    
    

}