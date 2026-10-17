using UnityEngine;

public class HomeScreenAudioManager : MonoBehaviour
{
    private AudioSource _source;
    [Header("Start Button Sound")]
    [SerializeField] private AudioClip _startButtonSound;
    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayStartSound()
    {
        Debug.Log("PlayStartSound Initiated");
        _source.clip = _startButtonSound;
        _source.Play();
    }
}
