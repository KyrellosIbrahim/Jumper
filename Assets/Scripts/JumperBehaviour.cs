using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class JumperBehaviour : MonoBehaviour
{
    public float jumpForce = 5f;
    public float moveSpeed = 5f;
    public LayerMask groundLayer;
    private bool isGrounded;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public Camera cam;
    public GameObject gameOverPanel;
    private float maxScore;
    public TMP_Text scoreText;
    public TMP_Text gameOverScoreText;
    public Animator anim;
    public bool isMoving;
    public AudioClip jumpSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        maxScore = 0f;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        Keyboard k = Keyboard.current;
        checkEdge();

        if (k.wKey.isPressed && isGrounded && rb.linearVelocity.y <= 0.01f) // jump
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            audioSource.PlayOneShot(jumpSound);
        }
        if (k.aKey.isPressed) { // move left
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            anim.SetBool("isMoving", true);
            transform.localScale = new Vector3(-1, 1, 1); // face left
        }
        else if (k.dKey.isPressed) { // move right
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            anim.SetBool("isMoving", true);
            transform.localScale = new Vector3(1, 1, 1); // face right
        }
        else { // stop moving
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("isMoving", false);
        }

        TrackScore();

        if (rb.position.y < cam.transform.position.y - 8.5f) {
            Debug.Log("Player has fallen off the map!");
            gameOverPanel.SetActive(true);
            gameOverScoreText.text = "Score: " + (int) maxScore;
            Time.timeScale = 0f; // Pauses the game
        }
    }

    void TrackScore()
    { // take y level and use that to calculate score
        float score = rb.position.y * 10;
        maxScore = Mathf.Max(score, maxScore);
        scoreText.text = "Altitude: " + Mathf.FloorToInt(maxScore).ToString();
    }

    void checkEdge() {
        float leftEdge = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float rightEdge = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        float clampedX = Mathf.Clamp(rb.position.x, leftEdge, rightEdge);
        if (clampedX != rb.position.x) {
            rb.position = new Vector2(clampedX, rb.position.y);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public void increaseJumpHeight(float heightIncrease) { // use in pickup for power-up script
        jumpForce += heightIncrease;
        Debug.Log("Jump height increased! Now at " + jumpForce + ".");
    }
}
