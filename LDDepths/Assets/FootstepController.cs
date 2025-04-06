using System;
using UnityEngine;
using System.Collections.Generic;

public class FootstepController : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClips;             // Your 10 footstep sounds
    [SerializeField] private Player.PlayerMovement playerMovement;
    [SerializeField] private float stepInterval = 0.5f;              // Time between footsteps
    [SerializeField] private float volume = 1f;
    [SerializeField] private float pitchVariance = 0.1f;             // Optional: makes it more natural
    [SerializeField] private int poolSize = 5;                       // The number of AudioSources to pool

    private float stepTimer;
    private Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();

    private void Awake()
    {
        // Initialize the pool of AudioSources
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tempGO = new GameObject("FootstepAudioSource_" + i);
            tempGO.transform.SetParent(transform);  // Optionally parent to the player
            AudioSource audioSource = tempGO.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // Optional: for 3D sound
            audioSourcePool.Enqueue(audioSource);
        }
    }

    private void Update()
    {
        if (footstepClips.Length == 0 || playerMovement == null) return;

        if (playerMovement.IsMoving())
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // Reset the timer when not moving
        }
    }

    private void PlayFootstep()
    {
        if (audioSourcePool.Count == 0) return;  // If the pool is exhausted, don't play a sound

        // Get an available AudioSource from the pool
        AudioSource audioSource = audioSourcePool.Dequeue();

        // Set position of the AudioSource to the player’s position
        audioSource.transform.position = transform.position;

        // Randomly choose a footstep sound
        int index = UnityEngine.Random.Range(0, footstepClips.Length);
        audioSource.clip = footstepClips[index];

        // Set random pitch variation for natural sound
        audioSource.pitch = 1f + UnityEngine.Random.Range(-pitchVariance, pitchVariance);
        audioSource.volume = volume;

        // Play the sound
        audioSource.Play();

        // Return the AudioSource to the pool after the clip finishes
        StartCoroutine(ReturnAudioSourceToPool(audioSource, audioSource.clip.length + 0.5f)); // 0.5f padding for reverb tail
    }

    private System.Collections.IEnumerator ReturnAudioSourceToPool(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        audioSourcePool.Enqueue(audioSource);  // Return the AudioSource to the pool
    }
}