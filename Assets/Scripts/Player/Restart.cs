using UnityEngine;

public class Restart : MonoBehaviour
{
    private Transform player;
    public Transform currentRespawnPoint;

    private PlayerHealth playerHealth;
    private IsPaused pauseManager;
    private KETweapons ketWeapons;
    private StormMovement stormMovement;

    [SerializeField] private GameObject storm;
    [SerializeField] private GameObject stormPrefab;
    [SerializeField] private Transform stormStartPosition;

    public GameObject waypoint;
    private Quaternion stormStartRotation;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        pauseManager = GetComponent<IsPaused>();
        ketWeapons = FindObjectOfType<KETweapons>();
        stormMovement = storm.GetComponent<StormMovement>();
        stormStartRotation = stormStartPosition.rotation;
    }

    private void Update()
    {
        //Move the players position to help get unstuck
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            player.position += new Vector3(0, 3, 0);
            player.rotation = Quaternion.Euler(Vector3.forward);
        }
    }

    public void Respawn()
    {
        //Reset the players values and unpause the game.
        playerHealth.dead = false;
        playerHealth.currentHealth = playerHealth.maxHealth;
        ketWeapons.currentEnergy = ketWeapons.maxEnergy;
        playerHealth.HealthBar();
        pauseManager.paused = false;
        playerHealth.deathScreen.SetActive(false);

        //Set the players rigidbody to Kinematic and move the player to the respawn point.
        Rigidbody rb;
        rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        player.transform.SetPositionAndRotation(currentRespawnPoint.position, currentRespawnPoint.rotation);
        rb.isKinematic = false;
        playerHealth.deadPs = false;

        //Reset the dust storm
        Destroy(storm);
        storm = Instantiate(stormPrefab, stormStartPosition.position, stormStartRotation);
        stormMovement = storm.GetComponent<StormMovement>();
        stormMovement.stormWaypoint = waypoint.GetComponent<StormWaypoint>();
        stormMovement.currentWaypoint = waypoint.transform;
        storm.transform.GetChild(0).GetComponent<StormDamage>().playerHealth = playerHealth;
    }
}
