using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMeter : MonoBehaviour
{

    public LevelManager LevelManager;

    public float NormalizedDistanceCovered => LevelManager.NormalizedDistanceCovered;
    public float PreviousDistanceCovered;

    
    //public float Velocity => (PreviousDistanceCovered - LevelManager.DistanceCovered) * (Time.deltaTime);

    public Image SmokeTrail;

    public Transform Spaceship;

    public Transform StartPos;
    public Transform DestinationPos;

    public TextMeshProUGUI VelocityUI;
    public TextMeshProUGUI KilometersCoveredUI;
    public TextMeshProUGUI DestinationKilometersUI;
    public int DestinationKilometersSum;

    // Start is called before the first frame update
    void Start()
    {
        DestinationKilometersUI.text = DestinationKilometersSum.ToString() + "KM";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpaceshipPosition();
        UpdateKilometersCoveredText();
        UpdateVelocityText();
        UpdateSmokeTrailFill();

        PreviousDistanceCovered = NormalizedDistanceCovered;
    }

    void UpdateVelocityText()
    {
        VelocityUI.text = ""; // Velocity.ToString("0.00000") + " KM/Sec";
    }

    void UpdateKilometersCoveredText()
    {
        int kilometersCoveredSum = Mathf.FloorToInt(NormalizedDistanceCovered * DestinationKilometersSum);
        KilometersCoveredUI.text = kilometersCoveredSum.ToString() + "KM";
    }

    void UpdateSpaceshipPosition()
    {
        var newX = Mathf.Lerp(StartPos.position.x, DestinationPos.position.x, NormalizedDistanceCovered);
        var shipPos = Spaceship.position;
        Spaceship.position = new Vector3(newX, shipPos.y, shipPos.z);
    }

    void UpdateSmokeTrailFill()
    {
        SmokeTrail.fillAmount = NormalizedDistanceCovered;
    }
}
