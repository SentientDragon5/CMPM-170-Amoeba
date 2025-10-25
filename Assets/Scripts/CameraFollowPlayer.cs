using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x , target.transform.position.y, -10f);
    }
}
