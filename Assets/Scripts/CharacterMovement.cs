using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Ensure this namespace is included

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask wallLayerMask; // Assign this in the Unity Editor to match your walls layer
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool moveUp = false;
    private bool moveDown = false;

    public GameObject winPanel; // Assign this in the Inspector with your TextMeshPro UI GameObject

    void FixedUpdate()
    {
        if (moveLeft) TryMove(Vector3.left);
        if (moveRight) TryMove(Vector3.right);
        if (moveUp) TryMove(Vector3.up);
        if (moveDown) TryMove(Vector3.down);
    }

    private void TryMove(Vector3 direction)
    {
        if (!IsBlocked(direction))
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private bool IsBlocked(Vector3 direction)
    {
        float distanceToCheck = moveSpeed * Time.deltaTime;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distanceToCheck, wallLayerMask);
        Debug.DrawRay(transform.position, direction * distanceToCheck, Color.red, 1f);

        foreach (var hit in hits)
        {
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                // If we hit a non-trigger collider, return true indicating movement is blocked.
                return true;
            }
        }

        // If we get here, no non-trigger colliders were hit, so movement isn't blocked.
        return false;
    }


    public void StartMovingLeft() => moveLeft = true;
    public void StopMovingLeft() => moveLeft = false;
    public void StartMovingRight() => moveRight = true;
    public void StopMovingRight() => moveRight = false;
    public void StartMovingUp() => moveUp = true;
    public void StopMovingUp() => moveUp = false;
    public void StartMovingDown() => moveDown = true;
    public void StopMovingDown() => moveDown = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EndTile"))
        {
            winPanel.SetActive(true); // Activate the TextMeshPro UI element
            StartCoroutine(RestartGame());
        }
    }

    IEnumerator RestartGame()
    {
        // Freeze the game for 2 seconds
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2);

        // Unfreeze and restart the game
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
