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


    public AudioClip[] PlayerPatchLeakClips = new AudioClip[4];
    public AudioClip[] PlayerDrillClips = new AudioClip[4];

    float CurrentRotation = 0;
    float currentPosition = 0;


    public GameObject WorldRootToRotate;

    public float TimeRequiredToPatchLeak = 0.3f;

    public float TimeRequiredToDrillALeak = 0.5f;

    public GameObject RocketLeakPrefab;

    private int m_StandardOnlyLayerMask;

    private AudioSource m_playerSounds;

    //Leaks
    private RocketLeak m_currentLeak;
    private float m_timeWorkedOnThisLeak = 0.0f;
    private List<RocketLeak> m_otherLeaksInRange = new List<RocketLeak>();


    //Drilling
    private float m_timeDrillingAHole = 0.0f;
    private Vector3 m_drillingPosition = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        if (WorldRootToRotate == null)
        {
            Debug.LogError("This Component needs to have a GameObject Attached to Rotate the Universe.");
        }

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.MinPositionZ);

        m_StandardOnlyLayerMask = LayerMask.GetMask("Default");
        m_playerSounds = GetComponent<AudioSource>();
    }

    bool CanMove(Vector3 direction)
    {
        Debug.DrawLine(transform.position,transform.position+(direction * 1f), Color.green, 1  );
        return !Physics.Raycast(transform.position, direction, 1f, m_StandardOnlyLayerMask);
       
    }

    // Update is called once per frame
    void Update()
    {
        bool isLeft = Input.GetKey(KeyCode.A);
        bool isRight = Input.GetKey(KeyCode.D);
        bool isFront = Input.GetKey(KeyCode.W);
        bool isBack = Input.GetKey(KeyCode.S);

        CurrentRotation = 0;

        //if drilling, you cannot do something else.

                //drilling
        if (Input.GetKey(KeyCode.Q))
        {
            m_timeDrillingAHole += Time.deltaTime;
            if (m_timeDrillingAHole > TimeRequiredToDrillALeak)
            {
                LevelManager.Inst.currentRocketStatus.AddLeak(m_drillingPosition, new Quaternion(), RocketLeakPrefab, this.WorldRootToRotate.transform, 1.0f);
            }
            return;
        }
        

        m_drillingPosition = Vector3.zero;
        m_timeDrillingAHole = 0.0f;

        if (isRight)
        {
            if (CanMove(Vector3.right))
            {
                CurrentRotation += RotationSpeed * Time.deltaTime;
                if (CurrentRotation > 360)
                {
                    CurrentRotation -= 360;
                }

            }
            else
            {
                //Debug.Log("Unable to move right - Blocked by Object.");
            }
            
        }
        if (isLeft)
        {
            if (CanMove(Vector3.left))
            {
                CurrentRotation -= RotationSpeed * Time.deltaTime;
                if (CurrentRotation < 0)
                {
                    CurrentRotation += 360;
                }
            }
            else
            {
                //Debug.Log("Unable to move left - Blocked by Object.");
            }
        }

        if (isFront)
        {
            if (CanMove(Vector3.forward))
            {
                currentPosition += CharacterSpeed  * Time.deltaTime;
                if (currentPosition > MaxPositionZ)
                {
                    currentPosition = MaxPositionZ;
                }
            }
            else
            {
                //Debug.Log("Unable to move forward - Blocked by Object.");
            }
        }

        if (isBack)
        {
            if (CanMove(Vector3.back))
            {
                currentPosition -= CharacterSpeed  * Time.deltaTime;
                if (currentPosition < MinPositionZ)
                {
                    currentPosition = MinPositionZ;
                }
            }
            else
            {
                //Debug.Log("Unable to move backward - Blocked by Object.");
            }
        }

        WorldRootToRotate.transform.RotateAround(Vector3.zero, Vector3.forward, CurrentRotation);

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, currentPosition);

        if (m_currentLeak != null)
        {
            if ( Input.GetKey(KeyCode.E))
            {
                if (m_timeWorkedOnThisLeak == 0)
                {
                    StartFixingLeaks();
                }
                m_timeWorkedOnThisLeak += Time.deltaTime;
                if (m_timeWorkedOnThisLeak > TimeRequiredToPatchLeak)
                {
                    EndFixingLeaks();
                    FixCurrentLeak();
                }
            }
            else
            {
                m_timeWorkedOnThisLeak = 0;
            }
        }



        //Debug.Log("Rotation: " + CurrentRotation.ToString("#.###"));
    }

    #region  Leaks


    private void StartDrillingLeakEffects()
    {
        int randomClipId = Random.Range(0, this.PlayerDrillClips.Length);
        m_playerSounds.clip = this.PlayerDrillClips[randomClipId];
        m_playerSounds.Play();  
    }

    private void EndDrillingLeakEffects()
    {
        m_playerSounds.Stop();
    }

    private void StartFixingLeaks()
    {
        int randomClipId = Random.Range(0, this.PlayerPatchLeakClips.Length);

        m_playerSounds.clip = this.PlayerPatchLeakClips[randomClipId];
        m_playerSounds.Play();        
    }

    private void EndFixingLeaks()
    {
        m_playerSounds.Stop();
    }


     //When the Primitive collides with the walls, it will reverse direction
    public void NotifyLeakInRegion(RocketLeak leak)
    {
        if (m_currentLeak==null)
        {
            m_currentLeak = leak;
            m_currentLeak.ActivateHighlight();
        }
        else
        {
            //we switch the nearest Leak if it
            if (Vector3.Distance(this.transform.position, leak.transform.position) < Vector3.Distance(this.transform.position, m_currentLeak.transform.position))
            {
                m_otherLeaksInRange.Add(m_currentLeak);
                m_currentLeak = leak;
                m_currentLeak.ActivateHighlight();
            }
            else
            {
                //Debug.Log("An additional leak git picked up.");
                m_otherLeaksInRange.Add(leak);
            }
        }

    }

    public void NotifyLeakExited(RocketLeak leak)
    {
        if (m_currentLeak == leak)
        {
            leak.DeactivateHighlight();
            m_currentLeak = null;
        }
        else
        {
            m_otherLeaksInRange.Remove(leak);
        }
    }

    private void FixCurrentLeak()
    {
        m_currentLeak.DeactivateHighlight();
        LevelManager.Inst.currentRocketStatus.rocketLeaks.Remove(m_currentLeak);
        Destroy(m_currentLeak.gameObject);
        m_currentLeak = null;
        m_timeWorkedOnThisLeak = 0.0f;


        foreach(RocketLeak leak in m_otherLeaksInRange)
        {
            if (m_currentLeak == null)
            {
                m_currentLeak = leak;
            }
            else
            {
                if (Vector3.Distance(this.transform.position, leak.transform.position) <
                    Vector3.Distance(this.transform.position, m_currentLeak.transform.position))
                    {
                        m_currentLeak = leak;
                    }
            }
        }

        if (m_currentLeak != null)
        {
            m_currentLeak.ActivateHighlight();
        }
    }

    #endregion



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
