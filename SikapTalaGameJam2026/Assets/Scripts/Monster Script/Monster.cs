using UnityEngine;

public class Monster : MonoBehaviour
{
    public static Monster instance;

    private MonsterMovement movement;
    public bool canMove = true;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        movement = GetComponent<MonsterMovement>();
    }
    void Update()
    {
        if (canMove)
        {
            movement.MoveToPoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Game Over");
            GameManager.instance.Death();
        }
    }

    public void ToggleMovement(bool shouldMove)
    {
        canMove = shouldMove;
    }
}
