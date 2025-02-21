using System.Collections;
using UnityEngine;

public class BirdsSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] birdsSounds;
    [SerializeField] private GameObject birdPrefab;
    private AudioSource CurrentAudioSource;
    private int SoundIndex;
    private Vector3 RandomPosition;
    private int previousIndex = 0;
    private Vector3 CurrentPos;


    void Start()
    {
        CurrentPos = transform.position;
    }

    void FixedUpdate()
    {

        if (Random.Range(0, 150) < 2)
        {
            RandomPosition = new Vector3(CurrentPos.x, CurrentPos.y, CurrentPos.z + Random.Range(-5000, 5000));
            SoundIndex = Random.Range(0, birdsSounds.Length);
            if (SoundIndex == previousIndex)
            {
                SoundIndex = Random.Range(0, birdsSounds.Length);
            }

            previousIndex = SoundIndex;
            GameObject Bird = Instantiate(birdPrefab, RandomPosition, Quaternion.identity);
            CurrentAudioSource = Bird.GetComponent<AudioSource>();
            float Volume = Random.Range(0.4f, 1f);
            CurrentAudioSource.volume = Mathf.Lerp(0, Volume, Time.fixedDeltaTime * 3);
            CurrentAudioSource.clip = birdsSounds[SoundIndex];
            CurrentAudioSource.Play();
            StartCoroutine(BirdDeath(Bird));
        }
    }

    private IEnumerator BirdDeath(GameObject bird)
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(bird);
        
    }
    
}
