using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteorHoles : MonoBehaviour
{

    public GameObject RocketHolePrefab;

    public float SpawnRate = 5;

    public AudioClip[] m_audioClipsForImpact = new AudioClip[4];


    private float m_lastSpawn;

    private RocketStatus m_currentRocketStatus;


    private List<Transform> m_possibleLeakSpawns = new List<Transform>();
    // Start is called before the first frame update


    private System.Random m_random;

    

    void Start()
    {
        m_currentRocketStatus = LevelManager.Inst.currentRocketStatus;
        m_random = new System.Random(1);

        GameObject respawnRoot = GameObject.FindGameObjectWithTag("Respawn");

        for(int i = 0; i < respawnRoot.transform.childCount ; i++)
        {
            Transform t = respawnRoot.transform.GetChild(i);
            if(t.gameObject.activeSelf)
            {
                m_possibleLeakSpawns.Add(t);
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        m_lastSpawn += Time.deltaTime;
        if (m_lastSpawn > SpawnRate)
        {
            m_lastSpawn = 0;
            Vector3 position;
            if (m_possibleLeakSpawns.Count > 0)
            {
                position = m_possibleLeakSpawns[m_random.Next(m_possibleLeakSpawns.Count)].position;
            }
            else
            {
                position = new Vector3(-2.98f, 18.26f, 88.43f);
            }

            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(m_audioClipsForImpact[Random.Range(0, m_audioClipsForImpact.Length)]);

            
            //Quaternion.AxisAngle()
            // float angle = Vector3.Angle(position, Vector3.up);
            //Quaternion correctRotation = Quaternion.AngleAxis(angle,Vector3.forward);

            //Quaternion correctRotation = new Quaternion();

            //Debug.Log("TODO: Get Rotation Quaternions done the correct way!! Pos: " + position + " - angle: " + angle.ToString("0.000") + " currentRotation: " + correctRotation);
            LevelManager.Inst.currentRocketStatus.AddLeak(position, RocketHolePrefab, transform, 1.0f);

            
        }
    }
}


