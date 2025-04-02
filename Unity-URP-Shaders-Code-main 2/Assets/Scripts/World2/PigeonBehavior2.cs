using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PigeonBehavior2 : MonoBehaviour
{
    private PigeonWave2 pigeonWave;
    public GameObject ClosestPigeon;
    private GameObject ClosestPigeonsClosest;
    
    [SerializeField] private float speed = 10f;
    
    [SerializeField] private float MaxDistance = 5f;
    

    
    private Rigidbody rb;
    private float DeltaMult = .7f;
    private float RanVal = 0f;
    private float MaxSpeed;
    private float turnForce = 200f;
    private float MinY = 0f;
    private List<GameObject> Goals;
    private bool Wait = false;

    



    void Start()
    {
        Goals = new List<GameObject>();
        foreach (GameObject goal in GameObject.FindGameObjectsWithTag("Goal"))
        {
            Goals.Add(goal);
        }
        MaxSpeed = speed * 3;
        pigeonWave = transform.parent.GetComponent<PigeonWave2>();
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(speed, 0, 0);
        StartCoroutine(RandomMove());
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
            Vector3 directionToTrigger = (other.transform.position - transform.position).normalized;
            rb.AddForce(-directionToTrigger * MaxSpeed, ForceMode.Acceleration);
        
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 Normal = collision.contacts[0].normal;
        if (collision.gameObject.CompareTag("GoalCollide") && (Normal == Vector3.up))
        {
            Wait = true;
            StartCoroutine(Waiting());
        }
    }

    

    void Update()
    {
        ClosestPigeon = PigeonWave2._pigeons[gameObject];
        ClosestPigeonsClosest = PigeonWave2._pigeons[gameObject].gameObject.GetComponent<PigeonBehavior2>().ClosestPigeon;
        //Asigning Pigeon Neighbors
        
        if (Wait)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            return;
        }
        //Speed and Rotation kill if goal is reaching
        
            
        if (ClosestPigeon != null)
        {
            float distance = Vector3.Distance(transform.position, ClosestPigeon.transform.position);
            if (distance < PigeonWave2.EqulibirumDist)
            {
                

                Vector3 diff = transform.position - ClosestPigeon.transform.position;
                diff.Normalize();
                diff /= distance;
                rb.linearVelocity += diff * DeltaMult;
            }

            if (distance > PigeonWave2.EqulibirumDist)
            {
                Vector3 directionToPigeon = (ClosestPigeon.transform.position - transform.position).normalized;
                rb.AddForce(directionToPigeon * MaxSpeed, ForceMode.Acceleration);
            }
        }
        //Balancing Pigeons based on EqulibriumDist
        
        //Referencing Closest Pigeon 
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity,ClosestPigeon.GetComponent<Rigidbody>().linearVelocity, Time.deltaTime * MaxSpeed);
        if (ClosestPigeonsClosest != null)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity,
                ClosestPigeonsClosest.GetComponent<Rigidbody>().linearVelocity, Time.deltaTime * MaxSpeed);
        }
        
        rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, -MaxSpeed,MaxSpeed), 
            Mathf.Clamp(rb.linearVelocity.y, -MaxSpeed,MaxSpeed), Mathf.Clamp(rb.linearVelocity.z, -MaxSpeed,MaxSpeed));
        if (rb.linearVelocity.magnitude < speed/3f)
        {
            Vector3 RanPos = new Vector3(Random.Range(-MaxSpeed, MaxSpeed), Random.Range(-MaxSpeed, MaxSpeed), Random.Range(-MaxSpeed, MaxSpeed));
            rb.linearVelocity += RanPos;
        }   
        //Managing Speed Clamping and then making it random if it's too slow
        

        Vector3 AdjustedPosition = new Vector3(transform.position.x, transform.position.y - pigeonWave.yHieght, transform.position.z);
        if (AdjustedPosition.magnitude > MaxDistance)
        {
            Vector3 directionToCenter = (new Vector3(0,300,0) - transform.position).normalized;
            rb.AddForce(directionToCenter * turnForce, ForceMode.Acceleration);
        }
        //Making sure Pigeon doesn't stray too far from the world
        
    }

    private IEnumerator RandomMove()
    {
        while(true)
        {
            RanVal = Random.Range(0f, 1f);
            if (RanVal < .1f)
            {
                for (int i = 0; i < 10; i++)
                {
                    Vector3 RanPos = new Vector3(Random.Range(-MaxSpeed, MaxSpeed), Random.Range(-MaxSpeed, MaxSpeed), Random.Range(-MaxSpeed, MaxSpeed));
                    rb.linearVelocity += RanPos;
                    yield return new WaitForSeconds(.1f);
                }
            }
            //Random jult

            if (RanVal > .7f)
            {
                for (int i = 0; i < 10; i++)
                {
                    int RandomInt = Random.Range(0, Goals.Count - 1);
                    Vector3 MoveTowardsGoal = (Goals[RandomInt].transform.position - transform.position).normalized;
                    if (Vector3.Distance(transform.position, Goals[RandomInt].transform.position) > 2f)
                    {
                        rb.linearVelocity = (MoveTowardsGoal * (turnForce / 2));
                    }
                    else
                    {
                        rb.linearVelocity = rb.linearVelocity * .95f;
                        yield return new WaitForSeconds(.1f);
                    }

                    yield return new WaitForSeconds(.1f);
                }
            }
            //Random move towards goal
            
            
            yield return new WaitForSeconds(4f);
        }
        
        
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        Vector3 TargetVel = new Vector3(speed, 0, 0);
        for (int i = 0; i < 10; i++)
        {
            Vector3 CurrentVel = Vector3.Lerp(rb.linearVelocity, TargetVel, Time.deltaTime * MaxSpeed);
            rb.linearVelocity = CurrentVel;
            yield return new WaitForSeconds(.1f);
            i++;
        }
        
        
        Wait = false;
    }
}
