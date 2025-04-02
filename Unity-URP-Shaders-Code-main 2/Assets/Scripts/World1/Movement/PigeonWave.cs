using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PigeonWave : MonoBehaviour
{
    [SerializeField] private GameObject pigeonPrefab;
    [SerializeField] private GameObject pigeonCsoundPrefab;
    private List<GameObject> pigeons;
    [SerializeField] private int xamount = 10;
    private int zamount;
    private float TransformOffsetMult = 150f;
    private float PigeonY = 2000f;
    [SerializeField] private float PigeonSpeed = 1000f;
    [SerializeField] private int GridCheckNum = 5;
    [SerializeField] private float MaxPigeonDistance = 6000f;
    [SerializeField] private float MinY = 2000f;
    private List<GameObject> PigeonCollides;

    private void Start()
    {
        zamount = xamount;
        Vector3 pos = new Vector3(0, 0, 0);
        int indexZ = 0;
        int indexX = 0;
        int PigeonCount = 0;
        pigeons = new List<GameObject>();
        PigeonCollides = new List<GameObject>();
        for (indexZ = 0; indexZ < zamount; indexZ++)
        {
            for (indexX = 0; indexX < xamount; indexX++)
            {
                pos = new Vector3(indexX * TransformOffsetMult * Random.Range(1,2), PigeonY, indexZ * TransformOffsetMult);
                GameObject pigeon;
                if (indexX == 0)
                {
                    pigeon = Instantiate(pigeonCsoundPrefab, pos, Quaternion.identity);
                }
                else
                {
                    pigeon = Instantiate(pigeonPrefab, pos, Quaternion.identity);
                }


                pigeon.transform.parent = transform;
                pigeons.Add(pigeon);
                PigeonCount++;
                pigeon.GetComponent<Rigidbody>().useGravity = false;
                //StartCoroutine(PigeonController(pigeon));
                //StartCoroutine(PigeonMove(pigeon, PigeonCount - 1, new Vector2(indexX, indexZ))); // Use index starting from 0
            }
        }
    }





    private IEnumerator PigeonController(GameObject pigeon)
    {
        Rigidbody rb = pigeon.GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(PigeonSpeed, Random.Range(-PigeonSpeed, PigeonSpeed), 0);
        float randGen = Random.Range(0f, 1f);
        while (true)
        {


            Vector3 MaxVelocity = rb.linearVelocity;

            if (randGen < .1f)
            {
                Vector3 Force = new Vector3(
                    Mathf.Clamp(Random.Range(-PigeonSpeed, PigeonSpeed) * 100, -PigeonSpeed, PigeonSpeed)
                    , Mathf.Clamp(Random.Range(-PigeonSpeed, PigeonSpeed) * 100, -PigeonSpeed, PigeonSpeed)
                    , Mathf.Clamp(Random.Range(-PigeonSpeed, PigeonSpeed) * 100, -PigeonSpeed, PigeonSpeed));
                rb.linearVelocity += Force;
            }

            foreach (GameObject Pigeon in PigeonCollides)
            {

                if (Pigeon.GetComponent<Rigidbody>().linearVelocity.magnitude > MaxVelocity.magnitude)
                {
                    MaxVelocity = Pigeon.GetComponent<Rigidbody>().linearVelocity;
                }
            }

            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, MaxVelocity, Time.deltaTime * 20);
            yield return null;
        }
    }


}