using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;

    private float topBounds = 14.5f;
    private float botBounds = 1;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 2, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and game is not over, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }

        // If player hits the top of the screen, stop them from leaving screen area and add a bit of downward force to stop them from getting stuck + play bounce sound
        if (transform.position.y > topBounds)
        {
            transform.position = new Vector3(transform.position.x, topBounds, transform.position.z);
            playerRb.AddForce(Vector3.down * floatForce, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 0.5f);
        }
        // If player hits the bottom of the screen, stop them from leaving screen area and add a bit of upward force to stop them from getting stuck + play bounce sound
        else if (transform.position.y < botBounds)
        {
            transform.position = new Vector3(transform.position.x, botBounds, transform.position.z);
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 0.5f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode, play sound effect and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 0.5f);
            gameOver = true;
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks and sound effect
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 0.5f);
            Destroy(other.gameObject);

        }

    }

}
