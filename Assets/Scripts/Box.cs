using UnityEngine;

public class Box : MonoBehaviour
{
    public void Move(Vector3 moveStep)
    {
        transform.position += moveStep;
    }
}
