using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MonsterAttack : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector or via script
    public Image deathBackground;

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
    private IEnumerator DelayForGameOver()
    {
        yield return new WaitForSeconds(2);
        //while (deathBackground.color.a<1)
        {
            //deathBackground.color = new color()
        }
    }
}