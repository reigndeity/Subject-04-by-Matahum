using UnityEngine;

public class MonsterTeleporter : MonoBehaviour
{
    public static MonsterTeleporter instance;

    private MonsterMovement m_movement;
    public TeleportArrays[] teleportArrays;
    public AudioSource auSource;

    int currentIndex;

    private void Awake()
    {
        instance = this;
    }
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
        if (currentIndex == index)
        {
            return;
        }

        m_movement.transform.position = teleportArrays[index].teleportPoint.position;

        m_movement.pointIndex = teleportArrays[index].continueIndex;

        AudioManager.instance.PlayMonsterSFX();
    }
}
[System.Serializable]
public class TeleportArrays
{
    public Transform teleportPoint;
    public int continueIndex;
}
