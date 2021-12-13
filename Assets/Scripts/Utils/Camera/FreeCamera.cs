using UnityEngine;

[RequireComponent(typeof(Camera))]
//按住鼠标右键旋转，同时按wasd移动，按住shift加速移动，按住中键拖拽视图
public class FreeCamera : MonoBehaviour
{
    //相机旋转速度
    public float rotateSpeed = 5f;

    //相机缩放速度
    public  float scaleSpeed  = 10f;
    private float curViewSize = 10f;

    //旋转变量
    private float m_deltX = 0f;
    private float m_deltY = 0f;

    //移动变量
    private float      m_camNormalMoveSpeed = 0.2f;
    private float      m_camFastMoveSpeed   = 2f;
    private Vector3    m_mouseMoveBegin     = Vector3.zero;
    private Vector3    m_targetPos;
    private Camera     m_cam;
    private float      m_distance;
    private float      m_camHitDistance = 10;
    private Quaternion m_camBeginRotation;

    void Start()
    {
        m_cam              = GetComponent<Camera>();
        m_camBeginRotation = m_cam.transform.rotation;
    }

    void Update()
    {
        //鼠标中键点下场景缩放
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            m_distance             =  Input.GetAxis("Mouse ScrollWheel") * scaleSpeed;
            curViewSize            += m_distance;
            curViewSize            =  Mathf.Clamp(curViewSize, 5, 15);
            m_cam.orthographicSize =  curViewSize;
        }

        //鼠标拖拽视野
        if (Input.GetMouseButtonDown(2))
        {
            //跟手拖拽的关键
            Ray        ray = m_cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 vec_cam2hitPoint = hit.point - transform.position;
                this.m_camHitDistance = Vector3.Dot(vec_cam2hitPoint, transform.forward);
            }

            m_mouseMoveBegin = m_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_camHitDistance));
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3 offset_Current2Begin = Vector3.zero;
            offset_Current2Begin = m_cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_camHitDistance)) - m_mouseMoveBegin;
            //相机的移动方向相反
            m_cam.transform.position = m_cam.transform.position - offset_Current2Begin;
        }
    }

    float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;

        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }
}