using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    [Header("Generator Setup")]
    public GameObject roadSegmentPrefab; 
    public float segmentLength = 50f;     
    public int segmentsToKeep = 5;        
    public Transform playerTransform;       

    private float spawnZ = 0.0f;           
    private List<GameObject> activeSegments = new List<GameObject>();

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            enabled = false;
            return;
        }

        // Generăm segmentele de start
        for (int i = 0; i < segmentsToKeep; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        // VERIFICARE 1: Generăm în față
        // Generăm un segment nou când jucătorul a depășit jumătatea drumului existent
        if (playerTransform.position.z > (spawnZ - (segmentsToKeep * segmentLength)))
        {
            SpawnSegment();
        }

        // VERIFICARE 2: Ștergem în spate (DOAR dacă e departe)
        // Ștergem segmentul doar dacă acesta este la o distanță mai mare de 'segmentLength' în spatele jucătorului
        if (activeSegments.Count > 0 && playerTransform.position.z > (activeSegments[0].transform.position.z + segmentLength * 2))
        {
            DeleteOldestSegment();
        }
    }

    void SpawnSegment()
    {
        Vector3 spawnPosition = new Vector3(0, 0, spawnZ);
        GameObject newSegment = Instantiate(roadSegmentPrefab, spawnPosition, Quaternion.identity);

        SegmentController segment = newSegment.GetComponent<SegmentController>();
        if (segment != null)
        {
            segment.SpawnObstacles();
        }

        activeSegments.Add(newSegment);
        spawnZ += segmentLength;
    }

    void DeleteOldestSegment()
    {
        GameObject oldestSegment = activeSegments[0];
        activeSegments.RemoveAt(0);
        Destroy(oldestSegment);
    }
}
/*
using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    [Header("Generator Setup")]
    public GameObject roadSegmentPrefab; // Conectați Prefab-ul de Segment aici
    public float segmentLength = 50f;     // Lungimea unui segment (Z Scale * 10)
    public int segmentsToKeep = 5;        // Câte segmente să păstreze active
    public Transform playerTransform;       // Referința la transform-ul Motocicletei

    private float spawnZ = 0.0f;           // Poziția Z unde trebuie generat următorul segment
    private List<GameObject> activeSegments = new List<GameObject>();

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned!");
            enabled = false;
            return;
        }

        // Generează primele N segmente pentru a umple spațiul de start
        for (int i = 0; i < segmentsToKeep; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        // Logica de spawn: Generează un segment nou când jucătorul se apropie de capătul vizibil
        // (spawnZ - 1.5 * segmentLength) asigură că noul segment este generat înainte ca jucătorul să ajungă la ultimul segment.
        if (playerTransform.position.z > (spawnZ - segmentsToKeep * segmentLength))
        {
            SpawnSegment();
            DeleteOldestSegment();
        }
    }

    void SpawnSegment()
    {
        // 1. Instanțiază un nou segment
        Vector3 spawnPosition = new Vector3(0, 0, spawnZ);
        GameObject newSegment = Instantiate(roadSegmentPrefab, spawnPosition, Quaternion.identity);

        // 2. Generează obstacolele pe segmentul nou!
        SegmentController segment = newSegment.GetComponent<SegmentController>();
        if (segment != null)
        {
            segment.SpawnObstacles();
        }

        // 3. Adaugă-l la lista și avansează punctul de spawn
        activeSegments.Add(newSegment);
        spawnZ += segmentLength;
    }

    void DeleteOldestSegment()
    {
        if (activeSegments.Count > segmentsToKeep)
        {
             // Ia cel mai vechi segment (primul din listă)
            GameObject oldestSegment = activeSegments[0];
            
            // Elimină-l din listă
            activeSegments.RemoveAt(0);
            
            // Distruge obiectul din scenă
            Destroy(oldestSegment);
        }
    }
}
*/