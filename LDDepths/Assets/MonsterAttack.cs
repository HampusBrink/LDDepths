using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEngine.Video;
using static System.Net.Mime.MediaTypeNames;

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MonsterAttack : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in Inspector or via script
    public UnityEngine.UI.Image deathBackground;   // Explicitly use UnityEngine.UI.Image
    public TextMeshProUGUI deathText; // Assign TextMeshProUGUI for death text in Inspector
    public float fadeDuration = 2f; // Duration of fade-in
    public float waitAfterFade = 3f; // Time to wait after fade before reloading the scene

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the FootstepController script
        if (other.GetComponent<FootstepController>() != null)
        {
            // Start the death sequence
            StartCoroutine(HandleDeathSequence());
        }
    }

    private IEnumerator HandleDeathSequence()
    {
        // Play the video first for 2 seconds
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }

        // Wait for 2 seconds to allow the video to play
        yield return new WaitForSeconds(2);

        // Fade in the death background and text
        float timeElapsed = 0f;
        Color initialBackgroundColor = deathBackground.color;
        Color initialTextColor = deathText.color;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(initialBackgroundColor.a, 1f, timeElapsed / fadeDuration);

            // Fade in both the background and text
            deathBackground.color = new Color(initialBackgroundColor.r, initialBackgroundColor.g, initialBackgroundColor.b, alpha);
            deathText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, alpha);

            yield return null;
        }

        // Ensure both are fully opaque after fade
        deathBackground.color = new Color(initialBackgroundColor.r, initialBackgroundColor.g, initialBackgroundColor.b, 1f);
        deathText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, 1f);

        // Wait for the specified time after fade
        yield return new WaitForSeconds(waitAfterFade);

        // Reload the current scene after the death sequence
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}