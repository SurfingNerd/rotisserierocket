using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceMeter : MonoBehaviour
{
    public Image SmokeTrail;

    public Transform Spaceship;

    public Transform StartPos;
    public Transform DestinationPos;

    public TextMeshProUGUI VelocityUI;
    public TextMeshProUGUI KilometersCoveredUI;
    public TextMeshProUGUI DestinationKilometersUI;
    public int DestinationKilometersSum;

    public float DistanceCovered = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        DestinationKilometersUI.text = DestinationKilometersSum.ToString() + "KM";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKilometersCoveredText();
        UpdateSpaceshipPosition();
        UpdateSmokeTrailFill();
    }

    void UpdateVelocityText()
    {
        VelocityUI.text = 30 + " KM/Sec";
    }

    void UpdateKilometersCoveredText()
    {
        int kilometersCoveredSum = Mathf.FloorToInt(DistanceCovered * DestinationKilometersSum);
        KilometersCoveredUI.text = kilometersCoveredSum.ToString() + "KM";
    }

    void UpdateSpaceshipPosition()
    {
        var newX = Mathf.Lerp(StartPos.position.x, DestinationPos.position.x, DistanceCovered);
        var shipPos = Spaceship.position;
        Spaceship.position = new Vector3(newX, shipPos.y, shipPos.z);
    }

    void UpdateSmokeTrailFill()
    {
        SmokeTrail.fillAmount = DistanceCovered;
    }
}
