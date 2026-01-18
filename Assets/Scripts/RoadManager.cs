
using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator2 : MonoBehaviour
{
    [Header("Configurare Drum")]
    public GameObject[] roadPrefabs; 
    public Transform player;         

    [Header("Setari Generare")]
    public float tileLength = 50f;   
    public int tilesOnScreen = 6;    // Am marit putin numarul de bucati active
    public float safeDistance = 15f; // Distanta in SPATELE jucatorului pana la stergere
    
    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < tilesOnScreen; i++)
        {
            if (i == 0) SpawnTile(0); 
            else SpawnTile(Random.Range(0, roadPrefabs.Length));
        }
    }

    void Update()
    {
        // Conditie imbunatatita:
        // Daca jucatorul a inaintat mai mult decat pozitia primei bucati + lungimea ei + o marja de siguranta
        if (player.position.z > (activeTiles[0].transform.position.z + tileLength + safeDistance))
        {
            SpawnTile(Random.Range(0, roadPrefabs.Length));
            DeleteOldTile();
        }
    }

    void SpawnTile(int prefabIndex)
    {
        GameObject go = Instantiate(roadPrefabs[prefabIndex], transform.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(go);
        spawnZ += tileLength;
    }

    void DeleteOldTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
/*using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator2 : MonoBehaviour
{
    [Header("Configurare Drum")]
    public GameObject[] roadPrefabs; // Pune aici cele 3 tipuri de drum (Prefabs)
    public Transform player;         // Trage aici obiectul Jucatorului

    [Header("Setari Generare")]
    public float tileLength = 50f;   // Lungimea exacta a unui segment (ex: 50 metri)
    public int tilesOnScreen = 5;    // Cate bucati sa fie vizibile simultan
    
    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnZ = 0f;

    void Start()
    {
        // Generam primele bucati la inceputul jocului
        for (int i = 0; i < tilesOnScreen; i++)
        {
            // Prima bucata (i=0) sa fie mereu tipul "gol" pentru a nu porni cu un obstacol in fata
            if (i == 0) SpawnTile(0); 
            else SpawnTile(Random.Range(0, roadPrefabs.Length));
        }
    }

    void Update()
    {
        // Verificam daca jucatorul s-a apropiat destul de capat pentru a pune o bucata noua
        // (Verificam daca pozitia Z a jucatorului a depasit punctul de spawn minus lungimea segmentelor vizibile)
        if (player.position.z - 30 > (spawnZ - tilesOnScreen * tileLength))
        {
            SpawnTile(Random.Range(0, roadPrefabs.Length));
            DeleteOldTile();
        }
    }

    void SpawnTile(int prefabIndex)
    {
        // Instantiem bucata de drum la pozitia spawnZ pe axa Z
        GameObject go = Instantiate(roadPrefabs[prefabIndex], transform.forward * spawnZ, Quaternion.identity);
        
        // O adaugam in lista pentru a o putea sterge mai tarziu
        activeTiles.Add(go);
        
        // Incrementam pozitia de spawn pentru urmatoarea bucata
        spawnZ += tileLength;
    }

    void DeleteOldTile()
    {
        // Stergem cel mai vechi segment (cel din spatele jucatorului)
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}*/