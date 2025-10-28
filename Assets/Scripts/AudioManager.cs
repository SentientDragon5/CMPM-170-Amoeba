using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip eatSound;
    public AudioClip losePointSound;

    private void OnEnable()
    {
        AmoebaEater.OnPointAdd += PlayEat;
        AmoebaPoint.OnPointRemove += PlayLosePoint;
    }

    private void OnDisable()
    {
        AmoebaEater.OnPointAdd -= PlayEat;
        AmoebaPoint.OnPointRemove -= PlayLosePoint;
    }


    public void PlayEat()
    {
        source.clip = eatSound;
        source.Play();
    }

    public void PlayLosePoint()
    {
        source.clip = losePointSound;
        source.Play();  
    }
}
