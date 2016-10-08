using UnityEngine;
using System.Collections;

public class Brush : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

        Screen.lockCursor = true;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 oldpos = transform.position;
        transform.position = new Vector3(oldpos.x + (0.1F*Input.GetAxis("Mouse X")), oldpos.y + (0.1F * Input.GetAxis("Mouse Y")), (Input.GetMouseButton(0) ? -7.0F : -7.5F) );
        GameObject canvas = GameObject.Find("Canvas");
        CanvasOps cs = canvas.GetComponent<CanvasOps>();

        if (Input.GetMouseButton(0))
           cs.applyBrush(transform.position.x - canvas.transform.position.x, transform.position.y -  canvas.transform.position.y);

    }
}
