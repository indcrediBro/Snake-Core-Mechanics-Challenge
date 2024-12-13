using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Dictionary<string, UIPanel> uiPanels = new Dictionary<string, UIPanel>();
    private UIPanel activePanel;

    // Initialize the UI Manager and cache the panels
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindUIPanels();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Find all UIPanel components in the scene
    private void FindUIPanels()
    {
        UIPanel[] panels = FindObjectsOfType<UIPanel>();
        foreach (UIPanel panel in panels)
        {
            if (!uiPanels.ContainsKey(panel.panelID))
            {
                uiPanels.Add(panel.panelID, panel);
                panel.ClosePanel();  // Start with all panels closed
            }
        }
    }

    // Method to open a specific panel
    public void OpenPanel(string panelID)
    {
        if (uiPanels.TryGetValue(panelID, out UIPanel panel))
        {
            if (activePanel != null)
            {
                activePanel.ClosePanel();
            }
            panel.OpenPanel();
            activePanel = panel;
        }
    }

    // Method to close the currently active panel
    public void CloseActivePanel()
    {
        if (activePanel != null)
        {
            activePanel.ClosePanel();
            activePanel = null;
        }
    }
}
