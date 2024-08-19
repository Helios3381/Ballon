using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 15f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip outOfBoundSound;

    private float upperBound = 14.5f;
    private float lowerBound = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        playerRb.AddForce(Vector3.down * Time.deltaTime, ForceMode.Impulse);
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            if (transform.position.y > upperBound) //out of bound
            {
                playerAudio.PlayOneShot(outOfBoundSound, 1.0f);
                playerRb.velocity = Vector3.zero;
                playerRb.AddForce(Vector3.down * floatForce * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                playerRb.AddForce(Vector3.up * floatForce);
            }
        }

        if (transform.position.y > upperBound) //out of bound
        {
            playerAudio.PlayOneShot(outOfBoundSound, 1.0f);
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.down * floatForce * Time.deltaTime, ForceMode.Impulse);
        }
        else if (transform.position.y < lowerBound)
        {
            playerAudio.PlayOneShot(outOfBoundSound, 1.0f);
            playerRb.AddForce(Vector3.up * 8 * floatForce);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

}
