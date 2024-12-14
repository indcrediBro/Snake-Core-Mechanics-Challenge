using UnityEngine;

public class OpenLinkButton : MonoBehaviour
{
    [SerializeField] private string urlToOpen;

    public void OpenLink()
    {
        Application.OpenURL(urlToOpen);
    }
}
