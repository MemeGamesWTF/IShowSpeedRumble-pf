using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerJump : MonoBehaviour
{
    public Button jumpButton;
    public Button startButton;
    public GameObject startScreen;
    public GameObject gameOverMenu;
    public GameObject winMenu;
    public Slider timerSlider;
    public float jumpForce = 5f;
    public float timeLimit = 60f;

    public AudioSource winSound;
    public AudioSource gameOverSound;
    public AudioSource jumpSound;

    private Rigidbody2D rb;
    public Animator anim;
    private bool isGrounded;
    private float timeRemaining;
    private bool gameEnded = false;
    private bool gameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeRemaining = timeLimit;

        jumpButton.onClick.AddListener(Jump);
        startButton.onClick.AddListener(StartGame);
        gameOverMenu.SetActive(false);
        winMenu.SetActive(false);
        startScreen.SetActive(true);

        timerSlider.maxValue = timeLimit;
        timerSlider.value = 0;

        Time.timeScale = 0;
    }

    void Update()
    {
        if (gameStarted && !gameEnded)
        {
            timeRemaining -= Time.deltaTime;
            timerSlider.value = timeLimit - timeRemaining;

            if (timeRemaining <= 0)
            {
                WinGame();
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;
        startScreen.SetActive(false);
        Time.timeScale = 1;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            anim.SetBool("Jump", true);
            jumpSound.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("Jump", false);
        }
        else if (collision.gameObject.CompareTag("enemy"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            gameOverMenu.SetActive(true);
            gameOverSound.Play();

            GameManager.instance.SendFinalScore(); // Send total score on Game Over

            Time.timeScale = 0;
        }
    }

    void WinGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            winMenu.SetActive(true);
            winSound.Play();

            GameManager.instance.AddScore(60); // Add 60 points on win
            GameManager.instance.SendFinalScore(); // Send updated score

            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
