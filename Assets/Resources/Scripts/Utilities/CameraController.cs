using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ScrollSpeed = 10f;
    public float DragSpeed = 2f;
    public float ScreenEdgeBorderThickness = 10f;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.position += new Vector3(horizontal, vertical, 0f) * ScrollSpeed * Time.deltaTime;

        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        if (mouseX < ScreenEdgeBorderThickness)
        {
            transform.position -= new Vector3(DragSpeed * Time.deltaTime, 0f, 0f);
        }
        else if (mouseX >= Screen.width - ScreenEdgeBorderThickness)
        {
            transform.position += new Vector3(DragSpeed * Time.deltaTime, 0f, 0f);
        }

        if (mouseY < ScreenEdgeBorderThickness)
        {
            transform.position -= new Vector3(0f, DragSpeed * Time.deltaTime, 0f);
        }
        else if (mouseY >= Screen.height - ScreenEdgeBorderThickness)
        {
            transform.position += new Vector3(0f, DragSpeed * Time.deltaTime, 0f);
        }
    }
}