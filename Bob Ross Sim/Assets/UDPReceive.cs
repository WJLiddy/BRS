using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    // X Y Z P R Y
    double[,] pos = new double[4,6];

    // R G B
    int[,] col = new int[4, 3];


    public int port; // define > init

    // start from unity3d
    public void Start()
    {

        init();
    }

    // OnGUI
    void OnGUI()
    {
        Rect rectObj = new Rect(40, 10, 200, 400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj, "On port: " + port + " #\n"
                + "ID 0 X Y Z: " + pos[0,0] + " " + pos[0,1] + " " + pos[0,2] + " \n");
    }

    // init
    private void init()
    {
        // define port
        port = 13370;

        // status
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (true)
        {

            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] endian_data = client.Receive(ref anyIP);
                byte[] data = new byte[endian_data.Length];

                // Reverse int at the start
                for (int i = 0; i != 4; i++)
                {
                    data[i] = endian_data[3 - i];
                }

                // fetch 6 double
                for (int dbl = 0; dbl != 6; dbl++)
                {
                    for (int b = 0; b != 8; b++)
                    {
                        data[4 + (dbl*8) + b] = endian_data[4 + (dbl * 8) + (7-b)];
                    }
                }

                int start = (4 + (6 * 8));
                // fetch 3 int
                for (int col = 0; col != 3; col++)
                {
                    for (int b = 0; b != 4; b++)
                    {
                        data[start + (col * 4) + b] = endian_data[start + (col * 4) + (3 - b)];
                    }
                }

                // ID
                int posindex = 4;
                int id = BitConverter.ToInt32(data, 0);

                for(int i = 0; i != 6; i++)
                {
                    //update position vector
                    pos[id,i] = BitConverter.ToDouble(data, posindex + (i*8));
                }

                int colindex = (4 + (8*6));

                for (int i = 0; i != 3; i++)
                {
                    col[id, i] = BitConverter.ToInt32(data, colindex + i*4);
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
}