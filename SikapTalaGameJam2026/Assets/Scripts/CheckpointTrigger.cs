using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameManager.instance.Save(checkpointIndex);

        this.gameObject.SetActive(false);
    }
}