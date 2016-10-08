using UnityEngine;
using System.Collections;

public class Brush : MonoBehaviour {

    bool paint_start = false;
    public int brush_id;
    // Use this for initialization
    void Start()
    {

        Screen.lockCursor = true;

    }

    float old_x;
    float old_y;
    // Update is called once per frame
    void Update()
    {

        Vector3 oldpos = transform.position;
        transform.position = new Vector3(oldpos.x + (0.1F*Input.GetAxis("Mouse X")), oldpos.y + (0.1F * Input.GetAxis("Mouse Y")), (Input.GetMouseButton(0) ? -6.4F : -6.7F) );
        GameObject canvas = GameObject.Find("Canvas"+brush_id);

        CanvasOps cs = canvas.GetComponent<CanvasOps>();

        float x_send = ((transform.position.x +- canvas.transform.position.x + 1F)/2F);
        float y_send = -((transform.position.y +- canvas.transform.position.y + 1F)/2F) + 1;

        // Send X from 0..1
        // Send Y from 0.. 1
        float changeamt = Mathf.Abs(old_x - x_send) + Mathf.Abs(old_y - y_send);

        if (Input.GetMouseButton(0) && paint_start && changeamt > 0.04)
            //we need substantial movment 
            cs.applyBrush(old_x,old_y,x_send, y_send, 10, 0, 1.0F, 0.0F, 0.2F);

        paint_start = Input.GetMouseButton(0);
        old_x = x_send;
        old_y = y_send;
     }
}
