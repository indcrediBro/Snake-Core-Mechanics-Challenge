using UnityEngine;

public class VirtualButton : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputHandler;
    [SerializeField] private Vector2 direction;

    public void TriggerMovement()
    {
        if (inputHandler != null && direction != Vector2.zero)
        {
            inputHandler.OnVirtualButtonPressed(direction);
        }
        else
        {
            MusicManager.Instance.PlayNextInGameTrack();
        }
        AudioManager.Instance.PlaySound("MouseOver");
    }
}
