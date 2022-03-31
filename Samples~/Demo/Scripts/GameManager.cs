using System;
using System.Collections.Generic;
using UnityEngine;

public enum ETeam : int
{
    Team1 = 0,
    Team2 = 1,
    [InspectorName(null)]
    TeamCount = 2
}

public class GameManager : MonoBehaviour
{
    public Camera camera;
    private UnitSelection unitSelection = new UnitSelection();
    private List<Unit>[] m_teamsUnits = new List<Unit>[(int)ETeam.TeamCount];
    bool m_isSelecting = false;
    
    #region Singleton
    protected static GameManager m_Instance = null;

    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<GameManager>();
                if (m_Instance == null)
                {
                    // Create the Picking system
                    GameObject newObj = new GameObject("GameManager");
                    m_Instance = Instantiate(newObj).AddComponent<GameManager>();
                }
            }

            return m_Instance;
        }
    }
    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        unitSelection.camera = camera;
        for (var index = 0; index < m_teamsUnits.Length; index++)
        {
            m_teamsUnits[index] = new List<Unit>();
        }
    }

    private void OnEnable()
    {
        unitSelection.SetObserver(m_teamsUnits[(int)ETeam.Team1]);
    }
    
    void Update()
    {
        // If we press the left mouse button, save mouse location and begin selection
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_isSelecting)
            {
                unitSelection.OnSelectionBegin(Input.mousePosition);
                m_isSelecting = true;
            }

            unitSelection.OnSelectionProcess(Input.mousePosition);
        }

        if (m_isSelecting)
        {
            unitSelection.OnSelectionProcess(Input.mousePosition);
        }

        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            unitSelection.OnSelectionEnd();
            m_isSelecting = false;
        }
    }
    
    void OnGUI()
    {
        if (m_isSelecting)
        {
            unitSelection.DrawGUI(Input.mousePosition);
        }
    }
    #endregion

    /// <summary>
    /// Need to be called in OnEnable
    /// </summary>
    /// <example>
    ///private void OnEnable()
    ///{
    ///    GameManager.Instance.RegisterUnit(team, this);
    ///}
    /// </example>
    /// <param name="team"></param>
    public void RegisterUnit(ETeam team, Unit unit)
    {
        m_teamsUnits[(int)team].Add(unit);
    }

    /// <summary>
    /// Need to be called in OnDisable
    /// </summary>
    /// <example>
    ///private void OnDisable()
    ///{
    ///    if(gameObject.scene.isLoaded)
    ///        GameManager.Instance.UnregisterUnit(team, this);
    ///}
    /// </example>
    /// <param name="team"></param>
    public void UnregisterUnit(ETeam team, Unit unit)
    {
        m_teamsUnits[(int)team].Remove(unit);
    }
}