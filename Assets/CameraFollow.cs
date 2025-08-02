using UnityEngine;

public class CameraFollowGroundY : MonoBehaviour
{
    public Transform target; // Jogador
    
    public float smoothSpeed = 2f;
    public Vector3 offsetXYZ;

    void Start()
    {

        // Salva o offset inicial em X e Z
        offsetXYZ = new Vector3(
            0f,
            2f,
            -3f
        );
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            target.position.x + offsetXYZ.x,
            target.position.y + offsetXYZ.y,
            target.position.z + offsetXYZ.z
        );
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}