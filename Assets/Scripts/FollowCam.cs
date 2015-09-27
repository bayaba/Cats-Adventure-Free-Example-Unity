using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
	public float followSpeed = 20;
    public Transform TargetPlayer = null;
    Vector3 pos;

    void Start()
    {
        if (TargetPlayer != null) pos = transform.position - TargetPlayer.position;
    }

	void Update()
    {
        if (TargetPlayer != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPlayer.position + pos, followSpeed * Time.deltaTime);
        }

        if (transform.position.y < 2.0f)
        {
            transform.position = new Vector3(transform.position.x, 2.0f, transform.position.z);
        }
    }
}
