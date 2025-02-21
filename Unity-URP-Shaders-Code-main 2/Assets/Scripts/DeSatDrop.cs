using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class DeSatDrop : MonoBehaviour
{
    [SerializeField] private GameObject DeSatDropPrefab;
    public ParticleSystem DeSatDropParticleSystem;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private int i = 0;
    private List<GameObject> DeSatDrops = new List<GameObject>();
    [SerializeField] private int MaxDrops = 50;
    private int KillIndex = 0;
    private GameObject DeSatDropParent;
    private float TimeScale = 400;
    [SerializeField] private SoundHandler soundHandler;

    private void Start()
    {
        DeSatDropParticleSystem = GetComponent<ParticleSystem>();
        DeSatDropParent = new GameObject("DeSatDropParent");
    }

    private void Update()
    {
        if (!soundHandler.Night && DeSatDrops != null && DeSatDrops.Count > MaxDrops)
        {
            StartCoroutine(DropKill(DeSatDrops[0], 0));

        }

        if (soundHandler.Night)
        {
            for (int i = 0; i < DeSatDrops.Count; i++)
            {
                StartCoroutine(DropKill(DeSatDrops[i], i));
            }
        }
        
    }

   

    void OnParticleCollision(GameObject other)
    {
        GameObject drop;
        int numCollisionEvents = DeSatDropParticleSystem.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;
        float yoffset = 0;

        while (i < numCollisionEvents)
        {
            if (rb)
            {
                
                // if (other.CompareTag("Dirt"))
                // {
                //     yoffset = 100f;
                // }
                // else
                // {
                //     yoffset = 0;
                // }
                Vector3 pos = collisionEvents[i].intersection;
                pos.y += yoffset;
               drop = Instantiate(DeSatDropPrefab, pos, Quaternion.Euler(270, 0, 0));
               drop.transform.localScale *= Random.Range(0.5f, 2f);
               drop.transform.parent = DeSatDropParent.transform;
               DeSatDrops.Add(drop);
                
                
            }
            i++;
        }
    }

    private IEnumerator DropKill(GameObject drop, int index)
    {
        DeSatDrops.RemoveAt(index);
        Vector3 time = Vector3.one * (Time.time + TimeScale);
        while (drop.transform.localScale.x > 0)
        {
            drop.transform.localScale -= time;
            yield return null;
        }
        Destroy(drop);
    }
}