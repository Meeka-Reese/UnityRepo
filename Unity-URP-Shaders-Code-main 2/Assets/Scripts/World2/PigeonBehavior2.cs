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
    private bool Wait = false;
    private bool RanMove = false;
    private bool GoalMove = false;
    public int PigeonGroupNum;
    private List<GameObject> GoalKeys;
    



    void Start()
    {
        
        MaxSpeed = speed * 3;
        pigeonWave = transform.parent.GetComponent<PigeonWave2>();
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(speed, 0, 0);
        GoalKeys = new List<GameObject>(PigeonWave2.Goals.Keys);
        StartCoroutine(RandomMove());
        
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 Normal = collision.contacts[0].normal;
        if (collision.gameObject.CompareTag("GoalCollide") && (Normal == Vector3.up))
        {
            Vector3 directionToTrigger = Vector3.zero;
            if ((collision.transform.position - transform.position) != Vector3.zero)
            {
                directionToTrigger = (collision.transform.position - transform.position).normalized;
            }

            rb.AddForce(-directionToTrigger * MaxSpeed, ForceMode.Acceleration);
            PigeonWave2.Goals[collision.gameObject.transform.parent.gameObject]++;
            Wait = true;
            StartCoroutine(Waiting());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("GoalCollide"))
        {
            if (PigeonWave2.Goals[collision.gameObject.transform.parent.gameObject] > 0)
            {
                PigeonWave2.Goals[collision.gameObject.transform.parent.gameObject]--;
            }

            Wait = false;
            StopCoroutine(Waiting());
        }
    }

    void FixedUpdate() {
        rb.linearVelocity *= 0.999f; // Small constant damping
    }

    void Update()
    {
        if (float.IsNaN(rb.linearVelocity.x))
        {
            Debug.LogError("NaN detected! Resetting velocity.", this);
            rb.linearVelocity = Vector3.zero;
        }

        if (PigeonGroupNum == 1)
        {
            ClosestPigeon = PigeonWave2._pigeons[gameObject];
        }
        else if (PigeonGroupNum == 2)
        {
            ClosestPigeon = PigeonWave2._pigeons2[gameObject];
        }

        if (ClosestPigeon != null)
            if (PigeonGroupNum == 1)
            {
                ClosestPigeonsClosest = PigeonWave2._pigeons[gameObject].gameObject.GetComponent<PigeonBehavior2>()
                    .ClosestPigeon;
            }
        else if (PigeonGroupNum == 2)
        {
            ClosestPigeonsClosest = PigeonWave2._pigeons2[gameObject].gameObject.GetComponent<PigeonBehavior2>()
                .ClosestPigeon;
        }

        if (!RanMove || !GoalMove)
            {
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
            if (ClosestPigeon != null)
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, ClosestPigeon.GetComponent<Rigidbody>().linearVelocity,
                Time.deltaTime * MaxSpeed);
            if (ClosestPigeonsClosest != null)
            {
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity,
                    ClosestPigeonsClosest.GetComponent<Rigidbody>().linearVelocity, Time.deltaTime * MaxSpeed);
                if (PigeonGroupNum == 1)
                {
                    if ((PigeonWave2._pigeons[ClosestPigeonsClosest] != null) &&
                        (PigeonWave2._pigeons[ClosestPigeonsClosest] != gameObject))
                    {
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity,
                            PigeonWave2._pigeons[ClosestPigeonsClosest].GetComponent<Rigidbody>().linearVelocity,
                            Time.deltaTime * MaxSpeed);
                    }
                }
                else if (PigeonGroupNum == 2)
                {
                    if ((PigeonWave2._pigeons2[ClosestPigeonsClosest] != null) &&
                        (PigeonWave2._pigeons2[ClosestPigeonsClosest] != gameObject))
                    {
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity,
                            PigeonWave2._pigeons2[ClosestPigeonsClosest].GetComponent<Rigidbody>().linearVelocity,
                            Time.deltaTime * MaxSpeed);
                    }
                }
            }
            

            rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, -MaxSpeed, MaxSpeed),
                Mathf.Clamp(rb.linearVelocity.y, -MaxSpeed, MaxSpeed),
                Mathf.Clamp(rb.linearVelocity.z, -MaxSpeed, MaxSpeed));
            if (rb.linearVelocity.magnitude < speed / 3f)
            {
                Vector3 UpSpeed = rb.linearVelocity.normalized * speed;
              
                
                rb.AddForce(UpSpeed, ForceMode.Acceleration);

                
            }
            //Managing Speed Clamping and then making it random if it's too slow


            Vector3 AdjustedPosition = new Vector3(transform.position.x, transform.position.y - pigeonWave.yHieght,
                transform.position.z);
            if (AdjustedPosition.magnitude > MaxDistance)
            {
                Vector3 directionToCenter = Vector3.zero;
                if ((new Vector3(0, MinY + 50, 0) - transform.position) != Vector3.zero)
                {
                    directionToCenter = (new Vector3(0, MinY + 50, 0) - transform.position).normalized;
                }
                

                rb.AddForce(directionToCenter * turnForce, ForceMode.Acceleration);
            }
            //Making sure Pigeon doesn't stray too far from the world
        }

    }

    private IEnumerator RandomMove()
    {
        while (true)
        {

            RanVal = Random.Range(0f, 1f);

            if (RanVal < .1f)
            {
                Vector3 RanPos = new Vector3(Random.Range(-MaxSpeed / 2, MaxSpeed / 2),
                    Random.Range(-MaxSpeed / 2, MaxSpeed / 2), Random.Range(-MaxSpeed / 2, MaxSpeed) / 2);
                for (int i = 0; i < 10; i++)
                {

                    RanMove = true;
                    rb.AddForce(RanPos * 0.5f, ForceMode.Acceleration);
                    yield return new WaitForSeconds(.2f);
                    if (i == 9)
                    {
                        RanMove = false;
                    }
                }

            }
            //Random jult

            if (RanVal > .8f)
            {
                //GoalMove = true;
                GameObject RandomKey = GoalKeys[Random.Range(0, GoalKeys.Count)];
                if (PigeonWave2.Goals[RandomKey.gameObject] > 10)
                {
                    RandomKey = GoalKeys[Random.Range(0, GoalKeys.Count)];
                    if (PigeonWave2.Goals[RandomKey.gameObject] > 10)
                    {
                        continue;
                    }
                }

                //fix goals shit
                Vector3 GoalVector = RandomKey.transform.position;
                Vector3 directionToGoal = (GoalVector - transform.position);


                for (int i = 0; i < 10; i++)
                {
                    if (directionToGoal != Vector3.zero)
                    {
                        if (directionToGoal != Vector3.zero)
                        {
                            directionToGoal.Normalize();


                            if (Vector3.Distance(transform.position, GoalVector) > 2f)
                            {
                                rb.linearVelocity = directionToGoal * speed / 2;
                            }
                            else
                            {
                                rb.linearVelocity = rb.linearVelocity * .98f;
                                yield return new WaitForSeconds(.1f);
                            }


                            if (i == 9)
                            {
                                GoalMove = false;
                            }

                            yield return new WaitForSeconds(.2f);
                        }
                    }
                }
                //Random move towards goal


                yield return new WaitForSeconds(4f);
            }

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
