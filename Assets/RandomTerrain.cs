using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public Transform barrel;
    GameObject[] spawnPoints;
    GameObject currentPoint;
    int index;

    void Start()
    {

        //Instantiate(barrel, new Vector3(2, 0, 0), Quaternion.identity);
        
        for (int i = 0; i < 10; i++)
        {
            spawnPoints = GameObject.FindGameObjectsWithTag("Tile");
            index = Random.Range(0, spawnPoints.Length);
            currentPoint = spawnPoints[index];

            Instantiate(barrel, currentPoint.transform.position, Quaternion.identity);
        }
        
    }

}