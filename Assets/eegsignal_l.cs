using UnityEngine;
using UnityEngine.InputSystem;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class eegsignal_l : MonoBehaviour
{
    public int port = 8051;  // UDP port
    private UdpClient udpClient;
    private Thread receiveThread;
    private volatile bool gripActive = false;  // True if EEG value = 1

    public Animator handAnimator;

    void Start()
    {
        // Start background UDP listener
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        Debug.Log("EEG UDP Listener started on port: " + port);
    }

    void ReceiveData()
    {
        udpClient = new UdpClient(port);
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (true)
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.ASCII.GetString(data).Trim();

                if (int.TryParse(message, out int value))
                {
                    gripActive = (value == 1);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("UDP Receive error: " + e.Message);
        }
    }

    void Update()
    {
        // Set Grip parameter directly to 1 or 0
        handAnimator.SetFloat("Grip", gripActive ? 1f : 0f);

        if (gripActive)
            Debug.Log("Grip closed (EEG = 1)");
        else
            Debug.Log("Grip open (EEG = 0)");
    }

    // ✅ Public getter to return the Animator reference
    public Animator GetHandAnimator()
    {
        return handAnimator;
    }

    void OnApplicationQuit()
    {
        try
        {
            if (receiveThread != null && receiveThread.IsAlive)
                receiveThread.Abort();

            if (udpClient != null)
                udpClient.Close();
        }
        catch { }
    }
}
