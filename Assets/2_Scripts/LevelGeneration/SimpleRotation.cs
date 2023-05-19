using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    void Update()
    {
        // rotate based on input from the editor
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + x, transform.eulerAngles.y + y, transform.eulerAngles.z + z);
    }
}
