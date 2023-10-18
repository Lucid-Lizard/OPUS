using System.Collections;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float moveSpeed;
    [Range(0, 1)]
    public float smoothTimeX;
    [Range(0, 1)]
    public float smoothTimeY;

    public Transform playerTransform;

    public void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTimeX);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTimeY);

        GetComponent<Transform>().position = pos;
    }
}
