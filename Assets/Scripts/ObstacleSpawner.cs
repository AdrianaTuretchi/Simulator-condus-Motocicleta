using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner3 : MonoBehaviour
{
    public GameObject obstaclePrefab; 

    [Range(0, 100)] 
    public int sansaGenerare = 80; 

    [Header("Siguranta: Maxim 2 pentru a lasa drum liber")]
    [Range(1, 2)] 
    public int numarMaximObstacole = 2; 
    
    public float[] lanes = { -3f, 0f, 3f }; 
    public float yOffset = 0.5f;

    [Header("Zona de siguranta (Metri)")]
    public float safeZoneZ = 10f; // Nu generam nimic sub 10 metri de la start

    void Start()
    {
        // VERIFICARE: Daca pozitia segmentului pe axa Z este mai mica decat safeZoneZ, oprim generarea
        if (transform.position.z < safeZoneZ)
        {
            Debug.Log("Segment in zona de siguranta - fara obstacole.");
            return; 
        }

        // Daca am trecut de zona de siguranta, verificam sansa de generare
        if (Random.Range(0, 101) <= sansaGenerare)
        {
            SpawnObstacles();
        }
    }

    void SpawnObstacles()
    {
        if (obstaclePrefab == null) return;

        List<int> availableIndices = new List<int> { 0, 1, 2 };
        int limitaSigura = Mathf.Clamp(numarMaximObstacole, 1, 2);
        int numarDeGenerat = Random.Range(1, limitaSigura + 1);

        for (int i = 0; i < numarDeGenerat; i++)
        {
            if (availableIndices.Count <= 1) break; 

            int listPosition = Random.Range(0, availableIndices.Count);
            int chosenLaneIndex = availableIndices[listPosition];

            float spawnX = lanes[chosenLaneIndex];

            Vector3 spawnPosition = new Vector3(spawnX, transform.position.y + yOffset, transform.position.z);
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            
            obstacle.transform.SetParent(this.transform);
            availableIndices.RemoveAt(listPosition);
        }
    }
}


/*using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner3 : MonoBehaviour
{
    public GameObject obstaclePrefab; 

    [Range(0, 100)] 
    public int sansaGenerare = 80; 

    [Header("Siguranta: Maxim 2 pentru a lasa drum liber")]
    [Range(1, 2)] 
    public int numarMaximObstacole = 2; 
    
    public float[] lanes = { -3f, 0f, 3f }; 
    public float yOffset = 0.5f;           

    void Start()
    {
        if (Random.Range(0, 101) <= sansaGenerare)
        {
            SpawnObstacles();
        }
    }

    void SpawnObstacles()
    {
        if (obstaclePrefab == null) return;

        // 1. Cream lista cu INDEXII benzilor (0, 1, 2)
        List<int> availableIndices = new List<int> { 0, 1, 2 };

        // 2. LIMITARE ABSOLUTA: 
        // Daca numarMaximObstacole este 3 sau mai mare din greseala, 
        // il fortam la 2 pentru a lasa mereu o banda libera.
        int limitaSigura = Mathf.Clamp(numarMaximObstacole, 1, 2);

        // 3. Alegem cate sa generam in acest moment (1 sau 2)
        int numarDeGenerat = Random.Range(1, limitaSigura + 1);

        for (int i = 0; i < numarDeGenerat; i++)
        {
            if (availableIndices.Count <= 1) break; // Ne oprim daca mai ramane doar o banda

            // Alegem un index la intamplare din lista de indexi RAMASI
            int listPosition = Random.Range(0, availableIndices.Count);
            int chosenLaneIndex = availableIndices[listPosition];

            // Calculam pozitia X
            float spawnX = lanes[chosenLaneIndex];

            // Generam obstacolul
            Vector3 spawnPosition = new Vector3(spawnX, transform.position.y + yOffset, transform.position.z);
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            
            // Il facem copilul segmentului
            obstacle.transform.SetParent(this.transform);

            // Stergem indexul ca sa nu mai punem pe aceeasi banda
            availableIndices.RemoveAt(listPosition);
        }
    }
}*/

/*using UnityEngine;
using System.Collections.Generic; // Avem nevoie de asta pentru a gestiona listele de benzi

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Referinte")]
    public GameObject obstaclePrefab; 

    [Header("Setari Obstacole")]
    [Range(0, 100)] 
    public int numarObstacole = 1; // Aici alegi in Inspector cate vrei (0, 1, 2 sau 3)
    
    public float[] lanes = { -3f, 0f, 3f }; 
    public float yOffset = 0.5f;           

    void Start()
    {
        SpawnObstacles();
    }

    void SpawnObstacles()
    {
        if (obstaclePrefab == null || numarObstacole <= 0) return;

        // Cream o lista cu indexul benzilor disponibile: 0, 1, 2
        List<int> availableLanes = new List<int> { 0, 1, 2 };

        // Ne asiguram ca nu cerem mai multe obstacole decat benzi avem
        int count = Mathf.Clamp(numarObstacole, 0, lanes.Length);

        for (int i = 0; i < count; i++)
        {
            // Alegem un index la intamplare din listele de benzi ramase
            int randomIndex = Random.Range(0, availableLanes.Count);
            int laneIndex = availableLanes[randomIndex];

            // Calculam pozitia
            Vector3 spawnPosition = new Vector3(lanes[laneIndex], transform.position.y + yOffset, transform.position.z);

            // Cream obstacolul
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

            // Il facem copilul segmentului de drum
            obstacle.transform.SetParent(this.transform);

            // Scoatem banda folosita din lista ca sa nu mai punem alt cub acolo
            availableLanes.RemoveAt(randomIndex);
        }
    }
}

*/
/*
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Referinte")]
    public GameObject obstaclePrefab; // Trage aici Prefab-ul cubului (cel cu tag-ul Obstacle)

    [Header("Setari Benzi")]
    public float[] lanes = { -3f, 0f, 3f }; // Coordonatele X ale benzilor
    public float yOffset = 0.5f;           // Inaltimea cubului fata de drum

    void Start()
    {
        // Aceasta functie ruleaza AUTOMAT imediat ce RoadGenerator2 
        // face Instantiate la drumul care are acest script pe el.
        SpawnObstacle();
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null) return;

        // 1. Alegem o banda la intamplare
        float randomX = lanes[Random.Range(0, lanes.Length)];

        // 2. Calculam pozitia: X-ul ales, Y-ul setat, si Z-ul centrului segmentului curent
        // Folosim transform.position.z pentru ca drumul e deja plasat de RoadGenerator2
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y + yOffset, transform.position.z);

        // 3. Cream cubul
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // 4. IMPORTANT: Facem cubul "copil" al acestui segment de drum.
        // Astfel, cand RoadGenerator2 sterge drumul in spate, cubul dispare si el.
        obstacle.transform.SetParent(this.transform);
    }
}*/