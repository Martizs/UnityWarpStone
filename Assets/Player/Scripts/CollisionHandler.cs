using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    float crashDelay = 2f;

    [SerializeField]
    ParticleSystem successParticles;

    [SerializeField]
    ParticleSystem crashParticles;

    [SerializeField]
    float finishDelay = 5f;

    [SerializeField]
    AudioClip crashSound;

    [SerializeField]
    float crashVolume = 0.1f;

    [SerializeField]
    AudioClip finishSound;

    [SerializeField]
    float finishVolume = 0.5f;
    bool isTransitioning = false;

    AudioSource warpStoneAudio;

    [SerializeField]
    bool collisionsEnabled = true;

    [SerializeField]
    GameObject endMenuCanvas;

    private void Start()
    {
        warpStoneAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
            // Guess we use else if, cause if its L it cant be C?
            // i mean technically it can idk
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isTransitioning && collisionsEnabled)
        {
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("Bumped into friendly");
                    break;
                case "Finish":
                    warpStoneAudio.Stop();
                    OnNextScene();
                    break;
                default:
                    warpStoneAudio.Stop();
                    OnCrash();
                    break;
            }
        }
    }

    private void OnCrash()
    {
        isTransitioning = true;
        warpStoneAudio.PlayOneShot(crashSound, crashVolume);
        crashParticles.Play();
        Delayed("ReloadScene", crashDelay);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnNextScene()
    {
        isTransitioning = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        warpStoneAudio.PlayOneShot(finishSound, finishVolume);
        successParticles.Play();
        Delayed("LoadNextScene", finishDelay);
    }

    private void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            ShowEndMenu();
        }
    }

    void ShowEndMenu()
    {
        endMenuCanvas.SetActive(true);
    }

    private void Delayed(string methodName, float delay)
    {
        gameObject.GetComponent<Movement>().enabled = false;
        Invoke(methodName, delay);
    }
}
