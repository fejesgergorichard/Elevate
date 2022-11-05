using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject HolePrefab;
    public int HolesCount = 300;


    void Start()
    {
        Vector3 spawnPosition = new Vector3();

        for (int i = 0; i < HolesCount; i++)
        {
            spawnPosition.y += Random.Range(0.8f, 2.0f);
            spawnPosition.x = Random.Range(-2f, 2f);

            Instantiate(HolePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
