using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class PythonProgram
{
    Process MainProcess;

    Process PythonProcess;

    TcpClient Client;

    IPEndPoint RemoteEP;

    TcpListener Listener;

    string ProgramName;

    string UsingIPAddress;

    int UsingPort;

    string ProgramPass;

    bool IsConnection;

    Thread TCPThread;

    public event Action<string> ResponceEvents;

    public PythonProgram(
        string ProgramName,
        string UsingIPAddress,
        int UsingPort,
        string ProgramPass
    )
    {
        this.ProgramName = ProgramName;
        this.UsingIPAddress = UsingIPAddress;
        this.UsingPort = UsingPort;
        this.ProgramPass = ProgramPass;
        RemoteEP = new IPEndPoint(IPAddress.Any, UsingPort);
    }

    public void StartPythonProgram()
    {
        if (MainProcess == null)
        {
            MainProcess = new Process();
            UnityEngine.Debug.Log (ProgramPass);
            MainProcess.StartInfo.FileName = ProgramPass;
            MainProcess.StartInfo.UseShellExecute = true;

            //MainProcess.StartInfo.Arguments = ;
            MainProcess.EnableRaisingEvents = true;
            MainProcess.Exited += MainProcess_Exited;

            MainProcess.Start();

            TCPThread = new Thread(new ThreadStart(Update));
            TCPThread.Start();
        }
    }

    public void EndProcess()
    {
        if (MainProcess != null && MainProcess.HasExited == false)
        {
            IsConnection = false;
            Client.Close();
            MainProcess.CloseMainWindow();
            MainProcess.Dispose();
            UnityEngine.Debug.Log(ProgramName + ": End");
        }
    }

    void MainProcess_Exited(object sender, System.EventArgs e)
    {
        IsConnection = false;
        Client.Close();
        MainProcess.CloseMainWindow();
        MainProcess.Dispose();
        UnityEngine.Debug.Log(ProgramName + ": Exited");
    }

    public void Update()
    {
        try
        {
            Listener = new TcpListener(RemoteEP);
            Listener.Start();
            Client = Listener.AcceptTcpClient();
            IsConnection = true;
            NetworkStream Stream = Client.GetStream();

            GetPID (Stream);
            while (IsConnection)
            {
                Byte[] data = new Byte[20000];
                String RawResponseData = String.Empty;
                Int32 bytes = Stream.Read(data, 0, data.Length);
                RawResponseData =
                    System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                ResponceEvents (RawResponseData);
            }
        }
        catch (System.IO.IOException e)
        {
            UnityEngine.Debug.Log("SocketException happened\n" + e.Message);
        }
        UnityEngine.Debug.Log("IsConnection: false");
    }

    void GetPID(NetworkStream Stream)
    {
        Byte[] Data = new Byte[256];
        String ResponseData = String.Empty;
        Int32 Bytes = Stream.Read(Data, 0, Data.Length);
        ResponseData = System.Text.Encoding.ASCII.GetString(Data, 0, Bytes);
        UnityEngine.Debug.Log("PID: " + ResponseData);

        PythonProcess = Process.GetProcessById(Int32.Parse(ResponseData));
        Byte[] Buffer =
            System.Text.Encoding.ASCII.GetBytes("responce: " + ResponseData);
        Stream.Write(Buffer, 0, Buffer.Length);
    }
}

[System.Serializable]
public class Hands
{
    string Index;

    public HandsPoint Point;

    public void Show()
    {
        Point.Show();
    }
}

[System.Serializable]
public class HandsPoint
{
    public float x;

    public float y;

    public float z;

    public void Show()
    {
        UnityEngine.Debug.Log("x:" + x + "\ny:" + y + "\nz:" + z);
    }
}
