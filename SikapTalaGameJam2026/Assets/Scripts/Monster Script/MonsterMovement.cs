using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [Header("Path Points")]
    public Transform[] pointToGo;

    [Header("Movement")]
    public int pointIndex;
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    public void MoveToPoint()
    {
        if (pointIndex >= pointToGo.Length)
            return;

        Transform targetPoint = pointToGo[pointIndex];

        // Move towards point
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        // Direction towards target
        Vector3 direction = (targetPoint.position - transform.position).normalized;

        // Prevent rotation error if already at position
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Reached point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            pointIndex++;

            // Optional loop
            if (pointIndex >= pointToGo.Length)
            {
                pointIndex = 0;
            }
        }
    }
}