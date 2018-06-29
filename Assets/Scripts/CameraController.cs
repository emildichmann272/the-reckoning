using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float edgeWidth = 0.1f;
    public float panSpeed = 2f;

    [SerializeField] public float zoomSpeed;
    [SerializeField] public float maxZoomIn;
    [SerializeField] public float maxZoomOut;
    [SerializeField] public float scrollDampener;

    public bool freeze = false;
    private float lastPress = 0;

    private float currentZoom = 1;
    private float scrollVelocity = 0;

    private float horizontalEdge, verticalEdge;

    private Transform backPlane;
    private World worldController;

    private Vector3 camMovement, camMovementForward, camMovementRight, crosVec;

    private Vector2 lastChunkCheck;

    private int backMask;

    bool first = true;

    // Use this for initialization
    void Start () {
        backMask = LayerMask.GetMask("BackPlaneRaycast");

        backPlane = GameObject.Find("BackPlanePivot").transform;
        worldController = GameObject.Find("World").GetComponent<World>();
        setEdgeWith(edgeWidth);

        camMovementForward = this.transform.up;
        camMovementForward.y = 0;
        camMovementForward.Normalize();

        camMovementRight = this.transform.right;
        camMovementRight.y = 0;
        camMovementRight.Normalize();

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);
        lastChunkCheck = currentPosition;

    }

    void setEdgeWith(float newWidth)
    {
        edgeWidth = newWidth;
        horizontalEdge = Camera.main.pixelWidth * edgeWidth;
        verticalEdge = Camera.main.pixelHeight * edgeWidth;
        camMovement = new Vector3();
    }
	
	// Update is called once per frame
	void Update () {
        if (first)
        {
            Vector3 camRay1 = Camera.main.ScreenPointToRay(new Vector2(0, 0)).direction;
            Vector3 camRay2 = Camera.main.ScreenPointToRay(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)).direction;

            float k1 = (-transform.position.y) / camRay1.y;
            float k2 = (-transform.position.y) / camRay2.y;

            Vector3 p1 = camRay1 * k1 + transform.position;
            Vector3 p2 = camRay2 * k2 + transform.position;

            crosVec = p1 - p2;

            float width = 10f * backPlane.localScale.x;
            float height = 10f * backPlane.localScale.z;
            Vector3 bPos = backPlane.position;
            bPos.x -= width / 2f;
            bPos.z -= height / 2f;

            worldController.LoadChunks(bPos.x, bPos.z, width, height);

            first = false;
        }
        float press = Input.GetAxisRaw("Cancel");
        //Debug.Log(press.ToString() + "  " + lastPress.ToString());
        if (press != lastPress)
        {
            if (press == 1)
            {
                freeze = !freeze;
            }
        }
        lastPress = press;
        if (freeze)
        {
            return;
        }
        // let player pan camera
        Vector2 mousePosition = Input.mousePosition;
        bool moved = false;
        camMovement.Set(0, 0, 0);
        if (mousePosition.x <= horizontalEdge)
        {
            camMovement -= camMovementRight;
            moved = true;
        }
        if (mousePosition.x >= Camera.main.pixelWidth - horizontalEdge)
        {
            camMovement += camMovementRight;
            moved = true;
        }
        if (mousePosition.y <= verticalEdge)
        {
            camMovement -= camMovementForward;
            moved = true;
        }
        if (mousePosition.y >= Camera.main.pixelHeight - verticalEdge)
        {
            camMovement += camMovementForward;
            moved = true;
        }

        float mouseScroll = Input.mouseScrollDelta.y;

        if (mouseScroll == 0)
        {
            scrollVelocity *= scrollDampener;
            if (Mathf.Abs(scrollVelocity) < 0.01)
            {
                scrollVelocity = 0;
            }
        }

        if (mouseScroll != 0 || scrollVelocity != 0)
        {
            scrollVelocity += mouseScroll * Time.deltaTime;
            float deltaZoom = scrollVelocity;
            float newZoom = scrollVelocity + currentZoom;

            if (newZoom > maxZoomIn)
            {
                scrollVelocity = 0;
                deltaZoom = maxZoomIn - currentZoom;
            }
            if (newZoom < maxZoomOut)
            {
                scrollVelocity = 0;
                deltaZoom = maxZoomOut - currentZoom;
            }
            if (deltaZoom != 0)
            {
                currentZoom += deltaZoom;

                updateZoom(deltaZoom);
            }
        }

        if (moved)
        {
            float width = 10f * backPlane.localScale.x;
            float height = 10f * backPlane.localScale.z;
            Vector3 bPos = backPlane.position;
            bPos.x -= width / 2f;
            bPos.z -= height / 2f;

            camMovement.Normalize();
            transform.position += camMovement * panSpeed * Time.deltaTime;

            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);
            Vector2 deltaDistance = currentPosition - lastChunkCheck;

            if (deltaDistance.magnitude > Chunk.SIZE)
            {
                worldController.LoadChunks(bPos.x, bPos.z, width, height);
                lastChunkCheck = currentPosition;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            
            Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit rayHit;

            bool hit = Physics.Raycast(mouseRay, out rayHit, 1000, backMask);
            
            if (hit)
            {
                Debug.Log(rayHit.point);
            }
        }

    }

    private void updateZoom(float deltaZoom)
    {
        Vector3 movement = this.transform.forward * deltaZoom * zoomSpeed;
        this.transform.position += movement;
        backPlane.position -= movement;

        Vector3 camRay1 = Camera.main.ScreenPointToRay(new Vector2(0, 0)).direction;
        Vector3 camRay2 = Camera.main.ScreenPointToRay(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)).direction;

        float k1 = (-transform.position.y) / camRay1.y;
        float k2 = (-transform.position.y) / camRay2.y;

        Vector3 p1 = camRay1 * k1 + transform.position;
        Vector3 p2 = camRay2 * k2 + transform.position;

        Vector3 newCrosVec = p1 - p2;

        float deltaScale = newCrosVec.magnitude / crosVec.magnitude;

        crosVec = newCrosVec;

        backPlane.localScale *= deltaScale;
    }
}
