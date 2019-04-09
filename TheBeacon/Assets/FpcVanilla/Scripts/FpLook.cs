using UnityEngine;

[RequireComponent(typeof(FpInput))]
public class FpLook : MonoBehaviour
{
    #region User Defined Properties
    [SerializeField] private float xSensitivity = 2f;
    [SerializeField] private float ySensitivity = 2f;
    [SerializeField] private float minimumX = -90F;
    [SerializeField] private float maximumX = 90F;

    [SerializeField] private float smoothSpeed = 20f;
    #endregion

    #region Data
    private new Camera camera;
    private FpInput input;
    private Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;

    //public Camera Camera => camera;
    public Camera Camera { get { if (camera == null) { camera = GetComponentInChildren<Camera>(); } return camera; } }

    #endregion

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        RotateView();
    }

    public void Init()
    {
        camera = GetComponentInChildren<Camera>();
        input = GetComponent<FpInput>();
        characterTargetRot = transform.localRotation;
        cameraTargetRot = camera.transform.localRotation;
    }

    public void RotateView() 
    {
        characterTargetRot *= Quaternion.Euler(0f, input.MouseX * xSensitivity, 0f);
        cameraTargetRot *= Quaternion.Euler(-input.MouseY * ySensitivity, 0f, 0f);

        //Clamp
        cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

        //Smooth
        transform.localRotation = Quaternion.Slerp(transform.localRotation, characterTargetRot, smoothSpeed * Time.deltaTime);
        camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, cameraTargetRot, smoothSpeed * Time.deltaTime);
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q) //Dark magic
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, minimumX, maximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}


