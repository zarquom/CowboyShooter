using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioMusicSource;
    [SerializeField] private AudioSource audioSFXSource;
    [SerializeField] private AudioClip[] audioSFXClips;
    public void PlaySound(string soundName)
    {
        for (int i = 0; i < audioSFXClips.Length; i++)
        {
            if (audioSFXClips[i].name == soundName)
            {
                audioSFXSource.PlayOneShot(audioSFXClips[i]);
                break;
            }
        }
    }
    public void SetVolume(float volume)
    {
        audioMusicSource.volume = volume;
        audioSFXSource.volume = volume;
    }
}
