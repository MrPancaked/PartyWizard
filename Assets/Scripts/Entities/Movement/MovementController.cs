using System;
using System.Numerics;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    /*
     * class for handling the movement of players and enemies.
     * Simply applies a force to the object in the moveDirection.
     * moveDirection can be set externally (in the playerController or an FSM)
     */
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour
    {
        private Rigidbody2D rb;
        public ScriptableObjects.Player.MovementData movementData; //public so playercontroller can update the controller data classes
    
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private EventInstance walkingSounds;
    
        [HideInInspector] public Vector2 moveDirection;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            //set walking audio based on scene (different audio inside than outside)
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("MainMenu"))) 
                walkingSounds = AudioManager.Instance.CreateInstance(FMODEvents.Instance.outdoorWalkSound);
            else
                walkingSounds = AudioManager.Instance.CreateInstance(FMODEvents.Instance.indoorWalkSound );
            
            rb.linearDamping = movementData.friction;
        }
        private void FixedUpdate()
        {
            Move();
            UpdateSprite();
        }
        public void Move()
        {
            rb.AddForce(moveDirection * movementData.speed, ForceMode2D.Force);
        }
    
        private void UpdateSprite() //maybe place in separate class to be reused by different sprites
        {
            if (moveDirection.magnitude > 0f)
            {
                animator.Play("WalkAnimation");
                if (!gameObject.CompareTag("Knight"))
                {
                    PLAYBACK_STATE playbackState;
                    walkingSounds.getPlaybackState(out playbackState);
                    if (playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING)) walkingSounds.start();
                }
            }
            else
            {
                animator.Play("IdleAnimation");
                walkingSounds.stop(STOP_MODE.ALLOWFADEOUT);
            }

            if (moveDirection.x < -0.1f) spriteRenderer.flipX = true;
            else if (moveDirection.x > 0.1f) spriteRenderer.flipX = false;
        }

        private void OnDestroy()
        {
            walkingSounds.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
