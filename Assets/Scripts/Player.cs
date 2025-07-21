using UnityEngine;

public class Player : MonoBehaviour
{
    public void Move(Vector3 moveStep)
    {
        transform.position += moveStep;
    }
}