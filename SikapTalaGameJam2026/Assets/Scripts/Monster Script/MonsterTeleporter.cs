using UnityEngine;

public class MonsterTeleporter : MonoBehaviour
{
    private MonsterMovement m_movement;
    public TeleportArrays[] teleportArrays;
    public int temporaryIndex;

    private void Start()
    {
        m_movement = FindFirstObjectByType<MonsterMovement>();
    }
    public void TeleportAndMove(int index)
    {
        if (m_movement == null)
        {
            Debug.Log("Cant see monster movement");
            return;
        }
        m_movement.transform.position = teleportArrays[index].teleportPoint.position;

        m_movement.pointIndex = teleportArrays[index].continueIndex;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Space Bar");
            TeleportAndMove(temporaryIndex);
        }
    }
}
[System.Serializable]
public class TeleportArrays
{
    public Transform teleportPoint;
    public int continueIndex;
}
