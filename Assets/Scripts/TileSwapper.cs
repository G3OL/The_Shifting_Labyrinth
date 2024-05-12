using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileSwapper : MonoBehaviour
{
    private GameObject outsideTile; // Reference to the outside tile.
    public TileController tileController;

    public AudioClip swapSound; // Assign this in the Inspector
    private AudioSource audioSource;
    public TextMeshProUGUI soundEffectsButtonText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(InitializeOutsideTile());

        // If not assigned through the Inspector, find it
        if (tileController == null)
            tileController = FindObjectOfType<TileController>();

        // Start the coroutine to initialize the outside tile.
        StartCoroutine(InitializeOutsideTile());
        soundEffectsButtonText.text = audioSource.mute ? "Enable Sound Effects" : "Disable Sound Effects";
    }

    IEnumerator InitializeOutsideTile()
    {
        // Wait for a short duration to ensure GameBoard's Start method has run.
        yield return new WaitForSeconds(0.1f);

        // Find the outside tile by its unique tag.
        outsideTile = GameObject.FindGameObjectWithTag("OutsideTile");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse click.
        {
            // Create a ray from the camera through the mouse position.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast for 2D physics.
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                GameObject clickedTile = hit.collider.gameObject;

                // Ensure we don't swap the outside tile with itself or the Start or End tiles.
                if (clickedTile.tag != "OutsideTile" && clickedTile.tag != "StartTile" && clickedTile.tag != "EndTile" && clickedTile.tag != "Player" && clickedTile.tag != "Board")
                {
                    tileController.MoveCharacterToInitialPosition();
                    SwapTiles(clickedTile, outsideTile);
                }
            }
        }
    }


    void SwapTiles(GameObject clickedTile, GameObject currentOutsideTile)
    {
        if (currentOutsideTile == null)
        {
            Debug.LogError("Current outside tile is null.");
            return;
        }

        // Swap positions of the clicked tile and the outside tile.
        Vector3 tempPosition = clickedTile.transform.position;
        clickedTile.transform.position = currentOutsideTile.transform.position;
        currentOutsideTile.transform.position = tempPosition;

        // Update the tags.
        currentOutsideTile.tag = "Untagged"; // or any other default tag you use for regular tiles
        clickedTile.tag = "OutsideTile";

        // Update the outside tile reference.
        outsideTile = clickedTile;

        // Play the swap sound effect
        audioSource.PlayOneShot(swapSound);
    }

    // Toggle sound effects
    public void ToggleSoundEffects()
    {
        audioSource.mute = !audioSource.mute;
        soundEffectsButtonText.text = audioSource.mute ? "Enable Sound Effects" : "Disable Sound Effects";
    }

}
