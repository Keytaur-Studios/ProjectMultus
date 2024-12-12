using UnityEngine;

public class DynamicRopeIK : MonoBehaviour
{
    public Transform[] ropeSegments; // Array of rope segment transforms
    public Transform target;         // Target for the end effector (e.g., hook/load)
    public float segmentLength = 0.3f; // Length of each segment
    public int iterations = 10;       // Number of FABRIK iterations

    void Update()
    {
        SolveFABRIK();
    }

    void SolveFABRIK()
    {
        // Forward pass: Move toward the target
        ropeSegments[ropeSegments.Length - 1].position = target.position;
        for (int i = ropeSegments.Length - 2; i >= 0; i--)
        {
            Vector3 direction = (ropeSegments[i].position - ropeSegments[i + 1].position).normalized;
            ropeSegments[i].position = ropeSegments[i + 1].position + direction * segmentLength;
        }

        // Backward pass: Move toward the root
        ropeSegments[0].position = transform.position; // Anchor point of the rope
        for (int i = 1; i < ropeSegments.Length; i++)
        {
            Vector3 direction = (ropeSegments[i].position - ropeSegments[i - 1].position).normalized;
            ropeSegments[i].position = ropeSegments[i - 1].position + direction * segmentLength;
        }
    }
}