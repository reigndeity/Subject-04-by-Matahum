using UnityEngine;

public class MonsterTeleporter : MonoBehaviour
{
    private MonsterMovement m_movement;
    public Transform monster_teleportPoint;
    public int nextIndex;

    private void Start()
    {
        m_movement = FindFirstObjectByType<MonsterMovement>();
    }
    public void TeleportAndMove()
    {
        if (m_movement == null)
        {
            Debug.Log("Cant see monster movement");
            return;
        }
        m_movement.transform.position = monster_teleportPoint.position;

        m_movement.pointIndex = nextIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Space Bar");
            TeleportAndMove();
        }
        Debug.Log("The fuck");
    }
}
