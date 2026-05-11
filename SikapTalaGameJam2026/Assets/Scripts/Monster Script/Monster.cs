using UnityEngine;

public class Monster : MonoBehaviour
{
    private MonsterMovement movement;
    public bool canMove = true;

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
        }
    }
}
