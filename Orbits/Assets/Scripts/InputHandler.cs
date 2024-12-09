using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
   [SerializeField] private GameObject Orbitor;
   [SerializeField] private SolarSystem solarSystem;
   private GameObject Sun;
   private float G;
   
   
   private float randomX;
   private float randomY;
   private void Start()
   {
      
      Sun = GameObject.Find("Sun");
      
   }

   private void Update()
   {
      G = solarSystem.G;
      if (Input.GetKeyDown(KeyCode.I))
      {
         ObjectLaunch();
      }
   }

   private void ObjectLaunch()
   {
      randomX = Random.Range(500.0f, 2000.0f);
      randomY = Random.Range(500.0f, 2000.0f);
      GameObject OrbitorInstance = Instantiate(Orbitor, transform.position, Quaternion.identity);
      OrbitorInstance.transform.position = new Vector3(randomX, randomY, Orbitor.transform.position.z);
      OrbitorInstance.GetComponent<Rigidbody>().mass = Random.Range(1000, 10000);
      solarSystem.celestialsList.Add(OrbitorInstance);
      OrbitorInstance.transform.localScale = new Vector3(50, 50, 50);
      
      float Ran = Random.Range(-1, 1) >= 0 ? 1 : -1;
      float m2 = Sun.GetComponent<Rigidbody>().mass;
      float r = Vector3.Distance(OrbitorInstance.transform.position, Sun.transform.position);
      OrbitorInstance.transform.LookAt(Sun.transform);
      OrbitorInstance.transform.eulerAngles += new Vector3(Random.Range(-90,90), Random.Range(-90,90), Random.Range(-90,90));
      OrbitorInstance.GetComponent<Rigidbody>().velocity += OrbitorInstance.transform.right * Ran * Mathf.Sqrt((G * m2) / r);
      
   }
}
