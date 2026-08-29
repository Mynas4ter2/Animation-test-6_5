using UnityEngine;

public class Enemycontroller : MonoBehaviour
{
    float speed = 1f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 forwardMove = transform.forward * speed;
        rb.linearVelocity = new Vector3(forwardMove.x, rb.linearVelocity.y, forwardMove.z);
    }

}
