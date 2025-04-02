using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonMovement : MonoBehaviour
{
    private List<GameObject> PigeonCollides;
    private float PigeonSpeed = 400f;
    [SerializeField] private float MaxPigeonDistance = 10000f;
    [SerializeField] private float MinY = 2000f;
    private float RanMult = 1000f;
    private float DeltaMult = .9f;
    private float Equilibrium = 200f;
    private float RandGen2 = 0;
    private float RandSpeed = 0;
   
    
    

    private Rigidbody rb;

    void Awake()
    {
        
        PigeonCollides = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Rand());
        StartCoroutine(SphereColliderSize());
    }

    void Start()
    {
       
        StartCoroutine(PigeonController());
        
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pigeon"))
        {
            PigeonCollides.Add(other.gameObject);
        }
        
    }

    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pigeon"))
        {
            PigeonCollides.Remove(other.gameObject);
        }
    }

    private IEnumerator Rand()
    {
        while (true)
        {
            RandGen2 = Random.Range(0f, 1f);
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator SphereColliderSize()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        while (true)
        {
            sphereCollider.radius = Random.Range(2, 20);
            yield return new WaitForSeconds(7f);
        }
    }

    private IEnumerator PigeonController()
    {
        bool randforce = false;
        rb.linearVelocity = new Vector3(PigeonSpeed * RandSpeed, 0, 0);

        

        while (true)
        {
            
            float randGen = Random.Range(0f, 2f);

            if (RandGen2 < 0.1f)
            {
                if (randGen < .02f)
                {
                    Vector3 Force = new Vector3(
                        Mathf.Clamp(Random.Range(-PigeonSpeed, PigeonSpeed) * RanMult, -PigeonSpeed, PigeonSpeed),
                        Mathf.Clamp(Random.Range(-PigeonSpeed, PigeonSpeed) * RanMult, -PigeonSpeed, PigeonSpeed),
                        Mathf.Clamp(Random.Range(-PigeonSpeed, PigeonSpeed) * RanMult, -PigeonSpeed, PigeonSpeed)
                    );
                    Force += rb.linearVelocity;

                    while (Vector3.Distance(Force, rb.linearVelocity) > 100)
                    {
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Force, Time.deltaTime * PigeonSpeed);
                        
                    }

                    yield return new WaitForSeconds(3f);
                }
            }
            else
            {
                float MaxDeltaMag = 0;
                Vector3 MaxVelocity = rb.linearVelocity;
                Vector3 AverageVelocity = Vector3.zero;
                
                foreach (GameObject Pigeon in PigeonCollides)
                {
                    AverageVelocity += Pigeon.GetComponent<Rigidbody>().linearVelocity;
                    if ((Pigeon.GetComponent<Rigidbody>().linearVelocity - MaxVelocity).magnitude > MaxDeltaMag)
                    {
                        
                        MaxVelocity = Pigeon.GetComponent<Rigidbody>().linearVelocity;
                        MaxDeltaMag = (Pigeon.GetComponent<Rigidbody>().linearVelocity - MaxVelocity).magnitude;
                    }
                }
             
                

                if (PigeonCollides.Count > 0)
                {
                    
                    AverageVelocity /= PigeonCollides.Count;
                    AverageVelocity = (MaxVelocity * 0.2f) + (AverageVelocity * 0.6f) + (rb.linearVelocity * 0.2f);

                    foreach (GameObject pigeon in PigeonCollides)
                    {
                        float distance = Vector3.Distance(transform.position, pigeon.transform.position);

                        if (distance < Equilibrium && distance > 0.001f)
                        {
                            Vector3 diff = transform.position - pigeon.transform.position;
                            diff.Normalize();
                            diff /= distance;
                            rb.linearVelocity += diff * DeltaMult;
                            
                            
                        }
                    }

                    if (!float.IsNaN(AverageVelocity.x) && !float.IsNaN(AverageVelocity.y) && !float.IsNaN(AverageVelocity.z))
                    {
                        yield return null;
                        rb.linearVelocity = AverageVelocity;
                    }
                }
            }

            if (Vector3.Distance(transform.position, Vector3.zero) >= MaxPigeonDistance || transform.position.y < MinY)
            {
                rb.linearVelocity = -rb.linearVelocity;
                yield return new WaitForSeconds(2f);
            }

            yield return null;
        }
    }
}