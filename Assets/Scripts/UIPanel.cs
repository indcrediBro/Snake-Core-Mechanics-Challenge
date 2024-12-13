using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public string panelID;  // Unique ID for each panel

    // Open and close methods with simple activation control
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
