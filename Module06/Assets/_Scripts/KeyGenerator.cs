using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int numberOfKeys = 3;
    private bool[] check;

    void Start()
    {
        check = new bool[spawnPoints.Length];
        while (numberOfKeys > 0)
        {
            int index = Random.Range(0, spawnPoints.Length);
            if (!check[index])
            {
                check[index] = true;
                Instantiate(keyPrefab, spawnPoints[index].position, Quaternion.identity, transform);
                numberOfKeys--;
            }
        }
    }
}
