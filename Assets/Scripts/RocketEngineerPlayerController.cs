using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEngineerPlayerController : MonoBehaviour
{

    public float MinPositionZ = -50;
    public float MaxPositionZ = 50;

    public float CharacterSpeed = 20; 

    //Rotation speed can be somehow calculated out of the Diameter and the Character speed. We set it by hand now.
    public float RotationSpeed = 90; 


    float CurrentRotation = 0;
    float currentPosition = 0;


    public GameObject WorldRootToRotate;

    // Start is called before the first frame update
    void Start()
    {
        if (WorldRootToRotate == null)
        {
            Debug.LogError("This Component needs to have a GameObject Attached to Rotate the Universe.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isLeft = Input.GetKey(KeyCode.A);
        bool isRight = Input.GetKey(KeyCode.D);
        bool isFront = Input.GetKey(KeyCode.W);
        bool isBack = Input.GetKey(KeyCode.S);

        CurrentRotation = 0;

        if (isRight)
        {
            CurrentRotation += RotationSpeed * Time.deltaTime;
            if (CurrentRotation > 360)
            {
                CurrentRotation -= 360;
            }
        }
        if (isLeft)
        {
            CurrentRotation -= RotationSpeed * Time.deltaTime;
            if (CurrentRotation < 0)
            {
                CurrentRotation += 360;
            }
        }

        if (isFront)
        {
            currentPosition += CharacterSpeed  * Time.deltaTime;
            if (currentPosition > MaxPositionZ)
            {
                currentPosition = MaxPositionZ;
            }
        }

        if (isBack)
        {
            currentPosition -= CharacterSpeed  * Time.deltaTime;
            if (currentPosition < MinPositionZ)
            {
                currentPosition = MinPositionZ;
            }
        }

        WorldRootToRotate.transform.RotateAround(Vector3.zero, Vector3.forward, CurrentRotation);

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, currentPosition);

        Debug.Log("Rotation: " + CurrentRotation.ToString("#.###"));
    }


    #region Character Animation
    		void UpdateAnimator(Vector3 move)
		{
			// // update the animator parameters
			// m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			// m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			// m_Animator.SetBool("Crouch", m_Crouching);
			// m_Animator.SetBool("OnGround", m_IsGrounded);
			// if (!m_IsGrounded)
			// {
			// 	m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			// }

			// // calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// // (This code is reliant on the specific run cycle offset in our animations,
			// // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			// float runCycle =
			// 	Mathf.Repeat(
			// 		m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			// float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			// if (m_IsGrounded)
			// {
			// 	m_Animator.SetFloat("JumpLeg", jumpLeg);
			// }

			// // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// // which affects the movement speed because of the root motion.
			// if (m_IsGrounded && move.magnitude > 0)
			// {
			// 	m_Animator.speed = m_AnimSpeedMultiplier;
			// }
			// else
			// {
			// 	// don't use that while airborne
			// 	m_Animator.speed = 1;
			// }
		}
    #endregion


}
