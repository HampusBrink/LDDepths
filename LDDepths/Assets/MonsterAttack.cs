using System.Diagnostics;
using UnityEngine;
using UnityEngine.Video;

public class MonsterAttack : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector or via script

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the PlayerMovement script
        if (other.GetComponent<FootstepController>() != null)
        {
            if (videoPlayer != null)
            {
                videoPlayer.Play();
            }
        }
    }
}