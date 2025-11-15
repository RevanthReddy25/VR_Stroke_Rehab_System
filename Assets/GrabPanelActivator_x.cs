using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPanelActivator_x : MonoBehaviour
{
    public x_rotation rotationDisplay;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (rotationDisplay != null)
            rotationDisplay.ActivatePanel(true);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (rotationDisplay != null)
            rotationDisplay.ActivatePanel(false);
    }
}
