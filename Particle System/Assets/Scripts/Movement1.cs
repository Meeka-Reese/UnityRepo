using System.Collections;
using UnityEngine;

public class Movement1 : MonoBehaviour
{
    public int count = 0;
    public ParticleSystem particles;
    public Vector3 target;
    private bool clickingDown = false;
    [SerializeField] private float maxMove = 5;
    private Vector3 Initialpos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = new Vector3(particles.transform.position.x + maxMove, particles.transform.position.y, particles.transform.position.z);
        Initialpos = particles.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!clickingDown)
        {
            particles.transform.position = Vector3.Slerp(particles.transform.position, target, 5f * Time.deltaTime);
            if (Vector3.Distance(particles.transform.position, target) < 0.1f)
            {
                clickingDown = true;
            }
        }

        if (clickingDown)
        {
            particles.transform.position = Vector3.Slerp(particles.transform.position, Initialpos, 5f * Time.deltaTime);
            if (Vector3.Distance(particles.transform.position, Initialpos) < 0.1f)
            {
                clickingDown = false;
            }
        }
    }

    IEnumerator Count()
    {
        while (true)
        {
            
            // yield return new WaitForSeconds(1);
            // if (count < maxMove && clickingDown == false)
            // {
            //     target = new Vector3(particles.transform.position.x + 1, particles.transform.position.y,
            //         particles.transform.position.z);
            //     count++;
            // }
            // else if (count >= maxMove || (clickingDown && particles.transform.position.x > 0))
            // {
            //     target = new Vector3(particles.transform.position.x - 1, particles.transform.position.y,
            //         particles.transform.position.z);
            //     count--;
            //     clickingDown = true;
            //     if (count == 0)
            //     {
            //         clickingDown = false;
            //     }
            // }
        }
    }
}
