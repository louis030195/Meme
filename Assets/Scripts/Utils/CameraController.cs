using UnityEngine;

namespace Utils
{
    public class CameraController : MonoBehaviour
    {
        [HideInInspector] public bool disable;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float rotationSpeed = 0.1f;
        private float _eulerX;
        private float _eulerY;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (disable) return;
            var t = transform;
            // Zoom / de-zoom
            t.position += Vector3.up * (-Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);

            //Get Forward face
            var dir = t.forward;
            //Reset/Ignore y axis
            dir.y = 0;
            dir.Normalize();

            // Move position with arrows around
            t.position += (t.right * Input.GetAxis("Horizontal") + dir * Input.GetAxis("Vertical")) * movementSpeed;

            // Rotation on Y axis
            if (Input.GetMouseButton(1) && !Input.GetMouseButton(2))
            {
                t.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotationSpeed, 0));
                _eulerX = t.rotation.eulerAngles.x;
                _eulerY = t.rotation.eulerAngles.y;
                t.rotation = Quaternion.Euler(_eulerX, _eulerY, 0);
            }

            // Rotate camera on X axis
            if (Input.GetMouseButton(1) && Input.GetMouseButton(2)) {
                t.Rotate(-new Vector3(Input.GetAxis("Mouse Y") * rotationSpeed, 0, 0));
                _eulerX = t.rotation.eulerAngles.x;
                _eulerY = t.rotation.eulerAngles.y;
                t.rotation = Quaternion.Euler(_eulerX, _eulerY, 0);
            }
        }
    }
}
