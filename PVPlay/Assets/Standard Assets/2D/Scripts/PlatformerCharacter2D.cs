using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField]private float spawnX = 0;
        [SerializeField]private float spawnY = 0;
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
		[SerializeField]private Transform Bullet;
		[SerializeField]public float Damage=25;
		[SerializeField]public float hp=100;

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private int jumpcount = 0;
        private GUIStyle diedstyle = new GUIStyle();
		private GameObject bullet;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject && !colliders[i].isTrigger)
                    m_Grounded = true;
            }
           m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


		public void Move(float move, bool crouch, bool jump, bool shoot)
        {


            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce-50*m_Rigidbody2D.velocity.y));
                jumpcount = 1;
            } else if (jump && jumpcount!=2 )
            {
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce - 50 * m_Rigidbody2D.velocity.y));
                jumpcount = 2;
            }
            if (m_Grounded) jumpcount = 0;
			if (shoot) {
				bullet = Instantiate (Bullet).gameObject;
				if(gameObject.tag=="Player1")bullet.tag="Bullet1";
				else bullet.tag="Bullet2";
				if (gameObject.transform.localScale.x > 0)
					bullet.transform.position = new Vector3 (gameObject.transform.position.x + 0.3f, gameObject.transform.position.y, 1);
				else {
					bullet.transform.position = new Vector3 (gameObject.transform.position.x - 0.3f, gameObject.transform.position.y, 1);
					bullet.transform.localScale = new Vector3 (bullet.transform.localScale.x,0-bullet.transform.localScale.y,bullet.transform.localScale.z);
				}
			}
        }
			
        void OnCollisionEnter2D(Collision2D col)
        {
			string enemybullet;
			string enemytag;
			if (gameObject.tag == "Player1") {
				enemybullet = "Bullet2";
				enemytag="Player1";
			} else {
				enemybullet = "Bullet1";
				enemytag = "Player2";
			}
					if (col.gameObject.tag == enemybullet)
            {
				hp -= GameObject.FindGameObjectWithTag (enemytag).GetComponent<PlatformerCharacter2D>().Damage;
				if(hp<=0){
					Destroy (GameObject.FindGameObjectWithTag (enemytag));
                m_Anim.SetBool("death", true);
                gameObject.GetComponent<Platformer2DUserControl>().enabled = false;
				}
            }
        }

        private void staticobject()
        {
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        private void spawn()
        {
            Application.LoadLevel(Application.loadedLevel);
            m_Anim.SetBool("death", false);
            gameObject.GetComponent<Platformer2DUserControl>().enabled = true;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

    }
}
