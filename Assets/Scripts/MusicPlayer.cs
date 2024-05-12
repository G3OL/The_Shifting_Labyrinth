using TMPro;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] tracks; // Assign these in the Inspector
    private AudioSource audioSource;
    public TextMeshProUGUI audioButtonText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomTrack();
        audioButtonText.text = audioSource.mute ? "Enable Music" : "Disable Music";
    }

    void PlayRandomTrack()
    {
        int randomIndex = Random.Range(0, tracks.Length);
        audioSource.clip = tracks[randomIndex];
        audioSource.loop = true;
        audioSource.Play();
    }

    // Toggle background music
    public void ToggleMusic()
    {
        audioSource.mute = !audioSource.mute;
        audioButtonText.text = audioSource.mute ? "Enable Music" : "Disable Music";
    }
}
