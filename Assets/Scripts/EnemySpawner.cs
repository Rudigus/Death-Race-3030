using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject player;
    public Enemies[] enemies;

    [System.Serializable]
    public class Enemies
    {
        public GameObject enemyPrefab;
        [HideInInspector] public GameObject enemyCar;
        public Vector3 spawnCoords;
    }

    private void Awake()
    {
        // Instantiate
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enemyCar = Instantiate(enemies[i].enemyPrefab);
            enemies[i].enemyCar.tag = "Enemy";
            Destroy(enemies[i].enemyCar.GetComponent<CarInputController>());
        }
    }
    void Start()
    {
        // Player
        player = GameObject.FindWithTag("Player");
        // Setting position, rotation and parent
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].enemyCar.transform.parent = transform;
            enemies[i].enemyCar.transform.position = enemies[i].spawnCoords;
            enemies[i].enemyCar.transform.rotation = Quaternion.identity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
