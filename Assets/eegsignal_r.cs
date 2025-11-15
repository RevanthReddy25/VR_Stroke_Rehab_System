using UnityEngine;
using UnityEngine.InputSystem;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class eegsignal_r : MonoBehaviour
{
    public int port = 8052;  // UDP port
    private UdpClient udpClient;
    private Thread receiveThread;
    private volatile bool gripActive = false;  // True if EEG indicates closed grip (0)

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
                    // ✅ Reverse logic: 0 = grip closed, 1 = grip open
                    gripActive = (value == 0);
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
        // Directly set the Animator's Grip parameter (1 for closed, 0 for open)
        handAnimator.SetFloat("Grip", gripActive ? 1f : 0f);

        if (gripActive)
            Debug.Log("Grip closed (EEG = 0)");
        else
            Debug.Log("Grip open (EEG = 1)");
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
