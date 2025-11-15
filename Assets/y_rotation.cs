using UnityEngine;
using TMPro;

public class y_rotation : MonoBehaviour
{
    [Header("Controller References")]
    public Transform leftController;
    public Transform rightController;

    [Header("UI References")]
    public TextMeshProUGUI leftRotationText;
    public TextMeshProUGUI rightRotationText;

    [Header("Panel Settings")]
    public GameObject panel; // The UI panel to enable/disable

    private bool isActive = false;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false); // Hide initially
    }

    void Update()
    {
        if (!isActive) return;

        if (leftController != null)
        {
            Vector3 leftRot = leftController.eulerAngles;
            leftRotationText.text = $"Left Rotation: Y={leftRot.y:F1}";
        }

        if (rightController != null)
        {
            Vector3 rightRot = rightController.eulerAngles;
            rightRotationText.text = $"Right Rotation: Y={rightRot.y:F1}";
        }
    }

    public void ActivatePanel(bool state)
    {
        isActive = state;
        if (panel != null)
            panel.SetActive(state);
    }
}
