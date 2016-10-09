using UnityEngine;

public class Brush : MonoBehaviour {

    bool paint_start = false;
    float orig_x;
    float orig_y;
    public int brush_id;
    public int passion = 0;
    public int evil = 0;
    // Use this for initialization
    void Start()
    {
        
        switch(brush_id)
        {
            case 0:
                orig_x = 0.5f;
                orig_y = 6f;
                break;
            case 1:
                orig_x = 2.5f;
                orig_y = 6f;
                break;
            case 2:
                orig_x = 0.5f;
                orig_y = 4f;
                break;
            case 3:
                orig_x = 2.5f;
                orig_y = 4f;
                break;

        }

    }

    float old_x;
    float old_y;
    // Update is called once per frame
    void Update()
    {

        GameObject udp = GameObject.Find("UDP");
        UDPReceive rs = udp.GetComponent<UDPReceive>();
        float x = (float)rs.pos[brush_id,0];
        float y = (float)rs.pos[brush_id,1];

        // C Y M
        float cy = rs.col[brush_id, 0] / 255f;
        float ye = rs.col[brush_id, 1] / 255f;
        float ma = rs.col[brush_id, 2] / 255f;

        int is_painting = rs.is_painting[brush_id];
        Vector3 oldpos = transform.position;
        //mouse
        //transform.position = new Vector3(oldpos.x + (0.1F*Input.GetAxis("Mouse X")), oldpos.y + (0.1F * Input.GetAxis("Mouse Y")), (Input.GetMouseButton(0) ? -6.4F : -6.7F) );
        transform.position = new Vector3(orig_x + -(1.5f * x), orig_y + (1.5f * y), (is_painting == 1) ? -6.2F : -6.5F);


        GameObject canvas = GameObject.Find("Canvas"+brush_id);

        CanvasOps cs = canvas.GetComponent<CanvasOps>();

        float x_send = ((transform.position.x +- canvas.transform.position.x + 1F)/2F);
        float y_send = -((transform.position.y +- canvas.transform.position.y + 1F)/2F) + 1;

        /**
         *     public int passion = 0;
    public int friendship = 0;
         * 
         * */

        // TRY TO DEBUG



        int c_targ = 0;
        // Ok, here , we look up drawing on other people's images
        if (transform.position.x > 1.5 && transform.position.y < 5)
        {
            c_targ = 3;

        }
        else if (transform.position.x < 1.5 && transform.position.y < 5)
        {
            c_targ = 2;
        }
        else if (transform.position.x > 1.5 && transform.position.y > 5)
        {
            c_targ = 1;
        }
        else if (transform.position.x < 1.5 && transform.position.y > 5)
        {
            c_targ = 0;
        }
        
        // Send X from 0..1
        // Send Y from 0.. 1
        float changeamt = Mathf.Abs(old_x - x_send) + Mathf.Abs(old_y - y_send);

       

        //??? CHANGEAMY DOESNT WORK HERE
        if ((c_targ != brush_id) && (is_painting == 1) && (paint_start))
        {
            Debug.Log("EXECUTE");
            // WE ARE BEING eVIL!
            evil++;

            // NEW POS UPDATE
            // UGLY but it'll go.
            Vector3 d = new Vector3(newX(c_targ) + -(1.5f * x), newY(c_targ) + (1.5f * y), (is_painting == 1) ? -6.2F : -6.5F);

            float x_send_e = ((d.x + -GameObject.Find("Canvas" + c_targ).GetComponent<CanvasOps>().transform.position.x + 1F) / 2F);
            float y_send_e = -((d.y + -GameObject.Find("Canvas" + c_targ).GetComponent<CanvasOps>().transform.position.y + 1F) / 2F) + 1;



            GameObject.Find("Canvas" + c_targ).GetComponent<CanvasOps>().applyBrush(x_send_e, y_send_e, x_send_e+0.1f, y_send_e+0.1f, 15, cy, ye, ma, 0.2F);

        } else if ((is_painting == 1) && paint_start && changeamt > 0.002)
        {
            //we are writing on our own, so add passion
            passion++;
            cs.applyBrush(old_x, old_y, x_send, y_send, 15, cy, ye, ma, 0.2F);
        }

        paint_start = (is_painting == 1);
        old_x = x_send;
        old_y = y_send;
     }

    float newX(int id)
    {
        switch (id)
        {
            case 0:
                return 0.5f;
            case 1:
                return 2.5f;
            case 2:
                return 0.5f;
                break;
            case 3:
                return 2.5f;
        }
        return -1f;
    }

    float newY(int id)
    {
        switch (id)
        {
            case 0:
                return 6f;
                break;
            case 1:
                return 6f;
                break;
            case 2:
                return 4f;
                break;
            case 3:
                return 4f;
                break;
        }
        return -1;

      }
}
