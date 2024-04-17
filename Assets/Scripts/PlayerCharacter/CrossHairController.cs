using UnityEngine;

public class CrossHairController : MonoBehaviour
{
    [SerializeField] Transform _crossHairPosition;
    RectTransform _crossHair;

    [SerializeField] private float _restingSize = 5f;
    [SerializeField] private float _maxSize = 15f;
    [SerializeField] private float _speed = 6f;
    private float _currentSize;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the default cursor
        Cursor.visible = false;

        _crossHair = GetComponent<RectTransform>();

    }

    void Update()
    {
       CrossHairPosition();
       if(isMoving)
       {
            _currentSize = Mathf.Lerp(_currentSize, _maxSize, Time.deltaTime * _speed);
       }
       else
       {
            _currentSize = Mathf.Lerp(_currentSize, _restingSize, Time.deltaTime * _speed);
       }
        _crossHair.sizeDelta = new Vector2(_currentSize, _currentSize);
    }

   

    #region private methods

    bool isMoving
    {
        get
        {
            if(Input.GetAxis("Horizontal") != 0 ||
                Input.GetAxis("Vertical") != 0 ||
                Input.GetAxis("Mouse X") != 0 ||
                Input.GetAxis("Mouse Y") != 0)
            { 
                return true;
            }
            else
            { 
                return false; 
            }
        }
    }

    private void CrossHairPosition()
    {
        // Get the position of the cursor in screen coordinates
        Vector3 cursorScreenPosition = Input.mousePosition;

        // Set a distance from the camera to place the crosshair
        float distanceFromCamera = 20f;

        // Convert the screen position to a point in the game world
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(cursorScreenPosition.x, cursorScreenPosition.y, distanceFromCamera));

        // Update the position of the crosshair to match the cursor position
        transform.position = cursorWorldPosition;
    }

    #endregion
}
