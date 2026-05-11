using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HoleObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Dead");
        }
    }
}
