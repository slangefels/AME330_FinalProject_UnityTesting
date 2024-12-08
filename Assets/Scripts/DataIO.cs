// A script for recieving data from the ESP-32 microcontroller. Adapted from the previous Alt-Control assignment.

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class DataIO : MonoBehaviour //Shannon's DataIO Unity script
{
    public long encoderPosition = 1; /* this variable stores the encoder position data coming in from the microcontroller; 
                                        encoder position data is stored and sent by the microcontroller as a long, so you
                                        might want to cast it to an int when using the data 
                                      */

    //Wi-Fi things
    UdpClient udpClient;
    IPEndPoint remoteEndPoint;

    void Start()
    {
        // Set up UDP to receive data from Arduino
        udpClient = new UdpClient(4211);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        Debug.Log("BeginRecieve done");

        // Set up UDP to send data to Arduino (this IP address should be the microcontroller's IP address)
        remoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.105"), 4211); // Adjust IP and port if necessary

        Debug.Log("Start() finished");
    }

    void Update()
    {
        //debug statement
        Debug.Log("End of Update() loop");
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        // Receive the UDP packet from Arduino
        byte[] receivedBytes = udpClient.EndReceive(ar, ref remoteEndPoint);
        string receivedText = Encoding.ASCII.GetString(receivedBytes);
        Debug.Log("Received: " + receivedText);

        // Parse the received data (encoder position)
        string[] data = receivedText.Split(',');
        if (data.Length == 1)
        {
            //encoder position is stored as a long in encoderPosition
            long.TryParse(data[0].Split(':')[1].Trim(), out encoderPosition);
        }

        //print debug statement to see the value coming over from the microcontroller in the console window
        Debug.Log("encoderPosition = " + encoderPosition);

        // Continue receiving data
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);

    }

    void OnApplicationQuit()
    {
        // Close the UDP client on quit
        udpClient.Close();
    }
}