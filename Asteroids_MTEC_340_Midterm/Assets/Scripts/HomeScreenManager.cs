using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenManager : MonoBehaviour
{
    [SerializeField] private HomeScreenAudioManager _homeScreenAudioManager;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            SceneManager.LoadScene("Play");
            _homeScreenAudioManager.PlayStartSound();
        }
    }
}