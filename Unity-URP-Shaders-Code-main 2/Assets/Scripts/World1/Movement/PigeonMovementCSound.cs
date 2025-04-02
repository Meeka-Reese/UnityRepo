using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonMovementCSound : MonoBehaviour
{
    private List<GameObject> PigeonCollides;
    private float PigeonSpeed = 400f;
    private float MaxPigeonDistance = 6000f;
    [SerializeField] private float MinY = 2000f;
    private float RanMult = 1000f;
    private float DeltaMult = .7f;
    private float Equilibrium = 300f;
    private float RandGen2 = 0;
    private float RandSpeed = 0;
    private float[] Tones = { 7.00f, 7.04f, 7.07f, 7.11f, 8.02f };
    private float[] Overtones = { 1f, 2f, 3f, 4f, 5f, 6f };
    public bool tone = false;
    public float CurrentTone = 7.00f;
    private CsoundUnity CSound;
    private int BirdNum = 0;
    private bool SecondNote = false;
    

    private Rigidbody rb;

    void Awake()
    {
        PigeonCollides = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        CSound = gameObject.GetComponent<CsoundUnity>();
        StartCoroutine(Rand());
        StartCoroutine(SphereColliderSize());
    }

    void Start()
    {
        
       
        StartCoroutine(PigeonController());
        CSound.SendScoreEvent("i1 0 5000 .1");
        foreach (GameObject pigeon in PigeonCollides)
        {
            CSound.SendScoreEvent("i2 0 5000 .05");
        }
        BirdNum = PigeonCollides.Count;
        
    }

    void Update()
    {
        if (BirdNum > PigeonCollides.Count)
        {
            CSound.SendScoreEvent("i-2 0 -1 .05");
            BirdNum = PigeonCollides.Count;
            
        }
        else if (BirdNum < PigeonCollides.Count)
        {
            CSound.SendScoreEvent("i2 0 5000 .05");
            BirdNum = PigeonCollides.Count;
        }

        if (PigeonCollides.Count > 20 && !SecondNote)
        {
            CSound.SendScoreEvent("i5 0 5000 .1");
            foreach (GameObject pigeon in PigeonCollides)
            {
                CSound.SendScoreEvent("i6 0 5000 .05");
            }
            SecondNote = true;
        }
        else if (PigeonCollides.Count < 20 && SecondNote)
        {
            CSound.SendScoreEvent("i-5 0 5000 .1");
            foreach (GameObject pigeon in PigeonCollides)
            {
                CSound.SendScoreEvent("i-6 0 5000 .05");
            }
            SecondNote = false;
        }
        
        CSound.SetChannel("Reverb", (Vector3.Distance(transform.position, Vector3.zero) / 6000));
        
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
        rb.linearVelocity = new Vector3(PigeonSpeed * RandSpeed * 500, 0, 0);

        

        while (true)
        {
            float randGen = Random.Range(0f, 2f);

            if (RandGen2 < 0.3f)
            {
                CSound.SendScoreEvent("i3 0 1");
                CSound.SendScoreEvent("i4 0 1");
                if (randGen < .05f)
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