using UnityEngine;

public class TV_Camera_LookAtPlayer : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        Player foundPlayer = FindFirstObjectByType<Player>();

        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        transform.rotation = Quaternion.LookRotation(player.position - transform.position) * Quaternion.Euler(0, 180, 0);
    }
}
