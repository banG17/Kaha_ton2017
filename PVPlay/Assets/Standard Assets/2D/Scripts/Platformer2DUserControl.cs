using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
		private bool m_shoot;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
				if(gameObject.tag=="Player1")m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
				else m_Jump = CrossPlatformInputManager.GetButtonDown("Jump1");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
			if(gameObject.tag=="Player1")m_shoot = CrossPlatformInputManager.GetButtonDown("Shoot");
			else m_shoot = CrossPlatformInputManager.GetButtonDown("Shoot1");
			float h;
			if(gameObject.tag=="Player1") h = CrossPlatformInputManager.GetAxis("Horizontal");
			else h = CrossPlatformInputManager.GetAxis("Horizontal1");
            // Pass all parameters to the character control script.
			m_Character.Move(h, crouch, m_Jump,m_shoot);
            m_Jump = false;
        }
    }
}
