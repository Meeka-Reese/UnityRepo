using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PigeonWave2 : MonoBehaviour
{
    [SerializeField] private GameObject _pigeonPrefab;
    private Vector3 _startPosition;
    private Vector3 _startPosition2;

    [SerializeField] private float PigeonXNum = 10f;

    [SerializeField] public float yHieght = 20f;
    public static float EqulibirumDist =  10f;

    public static Dictionary<GameObject, GameObject> _pigeons = new Dictionary<GameObject, GameObject>();
    public static Dictionary<GameObject, GameObject> _pigeons2 = new Dictionary<GameObject, GameObject>();
    public static Dictionary<GameObject, int> Goals = new Dictionary<GameObject, int>();
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject goal in GameObject.FindGameObjectsWithTag("Goal"))
        {
            Goals.Add(goal,0);
        }

        foreach (KeyValuePair<GameObject, int> pair in Goals)
        {
            Debug.Log(pair.Key.name);
        }
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
                _pigeon.GetComponent<PigeonBehavior2>().PigeonGroupNum = 1;
                _pigeon.transform.SetParent(transform);
                _pigeons.Add(_pigeon, null);
            }
        }
        for (int i = 0; i < PigeonXNum; i++)
        {
            for (int j = 0; j < PigeonXNum; j++)
            {
                _startPosition2 = new Vector3(i + 10, yHieght, j + 10);
                GameObject _pigeon = Instantiate(_pigeonPrefab, _startPosition2, Quaternion.identity);
                _pigeon.GetComponent<PigeonBehavior2>().PigeonGroupNum = 2;
                _pigeon.transform.SetParent(transform);
                _pigeons2.Add(_pigeon, null);
            }
        }

        StartCoroutine(ClosestPigeons());
    }

    

    private IEnumerator ClosestPigeons()
    {
        Dictionary<GameObject, GameObject> updates = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, GameObject> updates2 = new Dictionary<GameObject, GameObject>();
        while (true)
        {

            foreach (var birdEntry in _pigeons)
            {
                GameObject bird = birdEntry.Key;
                float closestDistance = float.MaxValue;
                GameObject closestNeighbor = null;

                GameObject RanBird = null;
                foreach (var otherBirdEntry in _pigeons)
                {
                    GameObject otherBird = otherBirdEntry.Key;

                    float Ran2 = Random.Range(0f, 1f);
                    if (Ran2 < .1f)
                    {
                        RanBird = otherBirdEntry.Value;
                    }


                    // Skip comparing with self
                    if (bird == otherBird) continue;

                    float distance = Vector3.Distance(bird.transform.position, otherBird.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        // float Ran = Random.Range(0f, 1f);
                        // if (Ran > 0.7f || RanBird == null)
                        // {
                        //     closestNeighbor = otherBird;
                        // }
                        // else
                        // {
                        //     closestNeighbor = RanBird;
                        // }
                        closestNeighbor = otherBird;
                    }
                }

                updates[bird] = closestNeighbor;
            }

            foreach (var birdEntry in _pigeons2)
            {
                GameObject bird = birdEntry.Key;
                float closestDistance = float.MaxValue;
                GameObject closestNeighbor = null;
                GameObject RanBird = null;
                foreach (var otherBirdEntry in _pigeons2)
                {
                    GameObject otherBird = otherBirdEntry.Key;

                    float Ran2 = Random.Range(0f, 1f);
                    if (Ran2 < .1f)
                    {
                        RanBird = otherBirdEntry.Value;
                    }


                    // Skip comparing with self
                    if (bird == otherBird) continue;

                    float distance = Vector3.Distance(bird.transform.position, otherBird.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        // float Ran = Random.Range(0f, 1f);
                        // if (Ran > 0.7f || RanBird == null)
                        // {
                        //     closestNeighbor = otherBird;
                        // }
                        // else
                        // {
                        //     closestNeighbor = RanBird;
                        // }
                        closestNeighbor = otherBird;
                    }
                }

                updates2[bird] = closestNeighbor;
            }

            foreach (var update in updates)
            {
                _pigeons[update.Key] = update.Value;
            }

            foreach (var update in updates2)
            {
                _pigeons2[update.Key] = update.Value;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
}
