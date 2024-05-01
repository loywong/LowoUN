// 控制主相机的前后位移 以及 左右旋转
using UnityEngine;

public class CameraInput : MonoBehaviour {
    [Header("主相机前进后退速度")][SerializeField] private float moveSpeed = 10f;
    [Header("主相机左右旋转速度")][SerializeField] private float rotaionSpeed = 100;
    [Header("主相机左右旋转最大范围限制")][SerializeField] private float yRotaionLimit = 120;

    private Transform tarCam;
    void Start () {
        // if在PC平台 若处理鼠标
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;

        tarCam = Camera.main.transform;

        newPos = tarCam.position;
        newRotation = tarCam.localRotation.eulerAngles;
    }

    float horizontal;
    float vertival;
    Vector3 newPos;
    Vector3 newRotation;
    void Update () {
        // 获取输入
        // 水平方向控制左右旋转 Y Rotation
        horizontal = Input.GetAxis ("Horizontal");
        // 垂直方向控制前后移动 Z Positoin
        vertival = Input.GetAxis ("Vertical");
    }

    void FixedUpdate () {
        // Debug.Log ($"vertival {vertival}, horizontal {horizontal}");
        // 处理前后位移
        if (vertival != 0) {
            newPos.z += vertival * moveSpeed * Time.fixedDeltaTime;
            tarCam.position = newPos;
        }

        // 处理左右旋转
        if (horizontal != 0) {
            newRotation.y -= horizontal * rotaionSpeed * Time.deltaTime;
            newRotation.y = Mathf.Clamp (newRotation.y, -yRotaionLimit / 2, yRotaionLimit / 2);
            tarCam.localRotation = Quaternion.Euler (newRotation);
        }
    }
}