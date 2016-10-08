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
                + "ID 0 X Y Z: " + pos[0,0], " " + pos[0,1] + pos[0,2] + " \n");
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
                byte[] data = client.Receive(ref anyIP);

                // ID
                int posindex = 4;
                int id = BitConverter.ToInt32(data, 0);
                for(int i = 0; i != 8; i++)
                {
                    //update position vector
                    pos[id,i] = BitConverter.ToDouble(data, posindex + (i*8));
                }

                int colindex = (4 + 64);

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

    // getLatestUDPPacket
    // cleans up the rest
    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }
}