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
    double[,] pos = new double[4,2];

    // R G B
    int[,] col = new int[4, 3];

    int[] is_painting = new int[4];


    public int port; // define > init

    bool dead = false;

    // start from unity3d
    public void Start()
    {
        init();
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
        while (!dead)
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

                // reverse 2 double
                for (int dbl = 0; dbl != 2; dbl++)
                {
                    for (int b = 0; b != 8; b++)
                    {
                        data[4 + (dbl*8) + b] = endian_data[4 + (dbl * 8) + (7-b)];
                    }
                }

                int start = (4 + (8 * 2));
                // fetch 3 int
                for (int coln = 0; coln != 3; coln++)
                {
                    for (int b = 0; b != 4; b++)
                    {
                        data[start + (coln * 4) + b] = endian_data[start + (coln * 4) + (3 - b)];
                    }
                }

                // fetch print 
                int startfinal = (4 + (8 * 2) + (3 * 4));
                // fetch 3 int
                for (int b = 0; b != 4; b++)
                {
                    data[startfinal + b] = endian_data[startfinal + (3 - b)];
                }
                
 
                // ID
                int id = BitConverter.ToInt32(data, 0);

                int posindex = 4;
                for (int i = 0; i != 2; i++)
                {
                    //update position vector
                    pos[id,i] = BitConverter.ToDouble(data, posindex + (i*8));
                }

                int colindex = (4 + (2*8));

                for (int i = 0; i != 3; i++)
                {
                    col[id, i] = BitConverter.ToInt32(data, colindex + i*4);
                }


                is_painting[id] = BitConverter.ToInt32(data, colindex + (3 * 4));

               // Debug.Log("From" + id+ "X " + pos[id, 0] + " Y " + pos[id, 1]+ "C " + col[id, 0] + " M " + col[id, 1] + " Y " + col[id, 2] + "Pnt? " + is_painting[id]);

            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("CLOSING!");
        dead = true;
        Thread.Sleep(1000);
        // udpclient object
        client.Close();
        // receiving Thread
        receiveThread.Abort();

    }
}