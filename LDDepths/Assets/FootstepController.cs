using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private Player.PlayerMovement playerMovement;
    [SerializeField] private float volumeLerpSpeed = 5f;
    [SerializeField] private float maxVolume = 1f;

    private void Update()
    {
        if (footstepAudio == null || playerMovement == null) return;

        bool isMoving = playerMovement.IsMoving();
        float targetVolume = isMoving ? maxVolume : 0f;

        footstepAudio.volume = Mathf.Lerp(footstepAudio.volume, targetVolume, Time.deltaTime * volumeLerpSpeed);

        if (isMoving && !footstepAudio.isPlaying)
        {
            footstepAudio.Play();
        }
        else if (!isMoving && footstepAudio.volume < 0.01f && footstepAudio.isPlaying)
        {
            footstepAudio.Stop();
        }
    }
}