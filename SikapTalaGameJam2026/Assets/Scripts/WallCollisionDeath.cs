using UnityEngine;

public class WallCollisionDeath : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.Death();
        }
    }
}
