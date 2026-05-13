using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameManager.instance.Save(checkpointIndex);

        if(checkpointIndex == 0)
        {
            GameManager.instance.OptimizeTV(0, true);
        }
        if(checkpointIndex == 3)
        {
            GameManager.instance.OptimizeTV(1, true);
            GameManager.instance.OptimizeTV(0, false);
        }
        if(checkpointIndex == 5)
        {
            GameManager.instance.OptimizeTV(1, false);
        }
        if(checkpointIndex == 6)
        {
            GameManager.instance.OptimizeTV(2, true);
        }
        if(checkpointIndex == 7)
        {
            GameManager.instance.OptimizeTV(3, true);
        }
        this.gameObject.SetActive(false);
    }
}