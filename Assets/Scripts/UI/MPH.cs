using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPH : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    private Text speedGauge;

    // Start is called before the first frame update
    void Start()
    {
        speedGauge = GetComponent<Text>();
    }

    // Convert the players velocity into mph and round it to an int. Then set the text to the speed.
    void Update()
    {
        float speed = Mathf.RoundToInt(player.velocity.magnitude * 2.23693629f);

        speedGauge.text = speed + " MPH";
    }
}
