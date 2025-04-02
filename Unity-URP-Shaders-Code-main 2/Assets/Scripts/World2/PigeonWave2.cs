using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PigeonWave2 : MonoBehaviour
{
    [SerializeField] private GameObject _pigeonPrefab;
    private Vector3 _startPosition;

    private float PigeonXNum = 10f;

    [SerializeField] public float yHieght = 20f;
    public static float EqulibirumDist =  5f;

    public static Dictionary<GameObject, GameObject> _pigeons = new Dictionary<GameObject, GameObject>();
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PigeonCreate();
    }

    void PigeonCreate()
    {
        for (int i = 0; i < PigeonXNum; i++)
        {
            for (int j = 0; j < PigeonXNum; j++)
            {
                _startPosition = new Vector3(i, yHieght, j);
                GameObject _pigeon = Instantiate(_pigeonPrefab, _startPosition, Quaternion.identity);
                _pigeon.transform.SetParent(transform);
                _pigeons.Add(_pigeon, null);
            }
        }
    }
    
    void Update()
    {
        Dictionary<GameObject, GameObject> updates = new Dictionary<GameObject, GameObject>();
        
        foreach (var birdEntry in _pigeons)
        {
            GameObject bird = birdEntry.Key;
            float closestDistance = float.MaxValue;
            GameObject closestNeighbor = null;

            foreach (var otherBirdEntry in _pigeons)
            {
                GameObject otherBird = otherBirdEntry.Key;
                
                // Skip comparing with self
                if (bird == otherBird) continue;

                float distance = Vector3.Distance(bird.transform.position, otherBird.transform.position);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNeighbor = otherBird;
                }
            }
            
            updates[bird] = closestNeighbor;
        }
    
        foreach (var update in updates)
        {
            _pigeons[update.Key] = update.Value;
        }
    }
}
