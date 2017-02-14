using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;
    public float zoomlevel;
    public float zoomPCSensitivity;
    public float zoomAndroidSensitivity;
    public float CameraDistanceFromTerrain;
    float defaultCameraDistanceFromTerrain;
    public Terrain ground;
    Vector2 lasttouchposition;
    Vector3 lastcameraposition;
    public Text debugtext;
    void SetCameraPosition(Vector3 newPosition)
    {
        GetComponent<Camera>().transform.position = newPosition;
    }
#if UNITY_ANDROID
    bool FingerDown;
    bool Finger2Down;

    Vector2 inbetween; 
#else
    bool LeftMouseDown;
#endif

    Camera GetCamera()
    {
        return GetComponent<Camera>();
    }

    //float GetCameraLevel()
    //{
    //    if (GetComponent<Camera>().transform.position.y < ground.SampleHeight(GetComponent<Camera>().transform.position) + MinimumCameraHeight * zoomlevel)
    //    {
    //        return ground.SampleHeight(GetComponent<Camera>().transform.position) + MinimumCameraHeight * zoomlevel;
    //    }

    //    return GetComponent<Camera>().transform.position.y;
    //}

    void OnButtonDown()
    { 
#if UNITY_ANDROID
        lastcameraposition = GetComponent<Camera>().transform.position;
        lasttouchposition = Input.GetTouch(0).position;
        FingerDown = true;
#else
        lastcameraposition = GetComponent<Camera>().transform.position;
        lasttouchposition = Input.mousePosition;
        LeftMouseDown = true;
#endif
        debugtext.text = "IN MOUSE DOWN";
    }

    void OnButtonUp()
    {
#if UNITY_ANDROID
        FingerDown = false;
#else
        LeftMouseDown = false;
#endif
        debugtext.text = "IN LET GO";
    }

    void OnDrag()
    {
#if UNITY_ANDROID
        SetCameraPosition(new Vector3(lastcameraposition.x + (lasttouchposition.x - Input.GetTouch(0).position.x) * sensitivityX, lastcameraposition.y, lastcameraposition.z + (lasttouchposition.y - Input.mousePosition.y) * sensitivityY));
        RaycastHit hit;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        if (ground.GetComponent<Collider>().Raycast(ray, out hit, 1000.0f))
        {
            Vector3 direction = new Vector3(-GetCamera().transform.forward.x, -GetCamera().transform.forward.y, -GetCamera().transform.forward.z).normalized;
            ground.SampleHeight(ray.GetPoint(hit.distance));
            SetCameraPosition(ray.GetPoint(hit.distance) + (direction * CameraDistanceFromTerrain));
        }
#else
        SetCameraPosition(new Vector3(lastcameraposition.x + (lasttouchposition.x - Input.mousePosition.x) * sensitivityX, lastcameraposition.y, lastcameraposition.z + (lasttouchposition.y - Input.mousePosition.y) * sensitivityY));
        RaycastHit hit;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        if (ground.GetComponent<Collider>().Raycast(ray, out hit, 1000.0f))
        {
            Vector3 direction = new Vector3(-GetCamera().transform.forward.x, -GetCamera().transform.forward.y, -GetCamera().transform.forward.z).normalized;
            SetCameraPosition(ray.GetPoint(hit.distance) + (direction * CameraDistanceFromTerrain));
            float groundy = ground.SampleHeight(GetCamera().transform.position);
            if (GetCamera().transform.position.y < groundy)
            {
                GetCamera().transform.position = new Vector3(GetCamera().transform.position.x, groundy, GetCamera().transform.position.z);
            }
        }
#endif
        debugtext.text = "PANNING\nCamera Pos: [" + GetCamera().transform.position.x + "," + GetCamera().transform.position.y + "," + GetCamera().transform.position.z + "]";
    }

    void Start()
    {
#if UNITY_ANDROID
        FingerDown = false;
        Finger2Down = false;
#else
        LeftMouseDown = false;
#endif
        defaultCameraDistanceFromTerrain = CameraDistanceFromTerrain;
        //GetComponent<Camera>().transform.position = new Vector3(GetComponent<Camera>().transform.position.x, ground.SampleHeight(GetComponent<Camera>().transform.position) + CameraHeight, GetComponent<Camera>().transform.position.z);
    }

    // Update is called once per frame
	void Update ()
    {
#if UNITY_ANDROID

        if (Input.touchCount == 2)
        {
            if (!Finger2Down)
            {
                inbetween = Input.GetTouch(0).position - Input.GetTouch(1).position;
                Finger2Down = true;
            }
            else 
            {
                debugtext.text = "ZOOMING";
                Vector2 currentinBetween = Input.GetTouch(0).position - Input.GetTouch(1).position;
                debugtext.text = debugtext.text + "\n Current: " + currentinBetween.magnitude + "\n Initial: " + inbetween.magnitude;
                zoomlevel = Mathf.Clamp(zoomlevel + (inbetween.magnitude - currentinBetween.magnitude) * zoomAndroidSensitivity, 1, 10);
                CameraDistanceFromTerrain = defaultCameraDistanceFromTerrain * zoomlevel;
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
                if (ground.GetComponent<Collider>().Raycast(ray, out hit, 1000.0f))
                {
                    Vector3 direction = new Vector3(-GetCamera().transform.forward.x, -GetCamera().transform.forward.y, -GetCamera().transform.forward.z).normalized;
                    ground.SampleHeight(ray.GetPoint(hit.distance));
                    SetCameraPosition(ray.GetPoint(hit.distance) + (direction * CameraDistanceFromTerrain));
                }
                inbetween = currentinBetween;
            }
        }
        else if(Input.touchCount == 1)
        {
            if (Input.touchCount == 1 && !FingerDown)
            {
                OnButtonDown();
            }
            else if (Input.touchCount == 1 && FingerDown)
            {
                OnDrag();
            }
        }
        else if (Input.touchCount == 0)
        { 
            if(FingerDown)
            {
                OnButtonUp();
            }
            else if (Finger2Down)
            {
                Finger2Down = false;
            }
        }
#else
        if (Input.GetMouseButton(0) && !LeftMouseDown)
        {
            OnButtonDown();
        }
        else if (!Input.GetMouseButton(0) && LeftMouseDown)
        {
            OnButtonUp();
        }
        else if (Input.GetMouseButton(0) && LeftMouseDown)
        {
            OnDrag();
            debugtext.text = "PANNING\nCamera Pos: [" + GetCamera().transform.position.x + "," + GetCamera().transform.position.y + "," + GetCamera().transform.position.z + "]";
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            zoomlevel = Mathf.Clamp(zoomlevel - Input.GetAxis("Mouse ScrollWheel"), 1, 10);
            CameraDistanceFromTerrain = defaultCameraDistanceFromTerrain * zoomlevel;
            RaycastHit hit;
            Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
            if (ground.GetComponent<Collider>().Raycast(ray, out hit, 1000.0f))
            {
                Vector3 direction = new Vector3(-GetCamera().transform.forward.x, -GetCamera().transform.forward.y, -GetCamera().transform.forward.z).normalized;
                ground.SampleHeight(ray.GetPoint(hit.distance));
                SetCameraPosition(ray.GetPoint(hit.distance) + (direction * CameraDistanceFromTerrain));
            }
            debugtext.text = "ZOOMING\nZoom Out Level: " + zoomlevel;
        }
#endif
        
	}
}
