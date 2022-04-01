<h1 align="center" style="border-bottom: none;">Selection zone📦 </h1>
<h3 align="center">Selection zone for unity 3D</h3>
<p align="center">
  <a href="https://github.com/semantic-release/semantic-release/actions?query=workflow%3ATest+branch%3Amaster">
    <img alt="Build states" src="https://github.com/semantic-release/semantic-release/workflows/Test/badge.svg">
  </a>
  <a href="https://github.com/semantic-release/semantic-release/actions?query=workflow%3ATest+branch%3Amaster">
    <img alt="semantic-release: angular" src="https://img.shields.io/badge/semantic--release-angular-e10079?logo=semantic-release">
  </a>
  <a href="LICENSE">
    <img alt="License" src="https://img.shields.io/badge/License-MIT-blue.svg">
  </a>
</p>
<p align="center">
  <a href="package.json">
    <img alt="Version" src="https://img.shields.io/github/package-json/v/OpenSourceUnityPackage/SelectionZone">
  </a>
  <a href="#LastActivity">
    <img alt="LastActivity" src="https://img.shields.io/github/last-commit/OpenSourceUnityPackage/SelectionZone">
  </a>
</p>

## What is it ?
Selection zone is a package that allow you to select multiple entity in unity 3D.

![image](https://user-images.githubusercontent.com/55276408/159162563-6f98255d-0657-44c6-be0b-9f760b45a95c.png)

Its architecture is input based and online friendly.
That mean that user need to supply input to the function to obtain an output.
It allows user to define which input is used to select (mouse, joystick, advance input system...) and define which entities can be selected (sorted in team ? Only one type of entity ?...) depending on your project.

This package is supply with a demo in sample that can be imported independently thanks to the unity package manager.

## How to use ?
To use it, create a base class for you entities that inheriting from interface ISelectable and define it's behaviour on entity is selected.
```C#
using UnitSelectionPackage;
using UnityEngine;

public enum ETeam : int
{
    Team1 = 0,
    Team2 = 1,
    [InspectorName(null)] TeamCount = 2
}

public class Unit : MonoBehaviour, ISelectable
{
    private bool m_isSelected = false;
    private Material m_material;
    private ETeam m_team;

    private void OnEnable()
    {
        GameManager.Instance.RegisterUnit(m_team, this);
    }

    private void OnDisable()
    {
        if(gameObject.scene.isLoaded)
            GameManager.Instance.UnregisterUnit(m_team, this);
    }

    protected void Awake()
    {
        m_material = GetComponent<Renderer>().material;
    }

    public void SetSelected(bool selected)
    {
        m_isSelected = selected;
        m_material.color = m_isSelected ? Color.red : Color.white;
    }
    
    public bool IsSelected()
    {
        return m_isSelected;
    }
}
```

Finally, you need to integrate it into your game logic. You need to call into your update and draw function and supply input to use (list of unit, cursor, camera to use)

```C#
using System.Collections.Generic;
using UnitSelectionPackage;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera camera;
    private UnitSelection<Unit> m_unitSelection = new UnitSelection<Unit>();
    private List<Unit>[] m_teamsUnits = new List<Unit>[(int) ETeam.TeamCount];
    private bool m_isSelecting;

    private static GameManager m_Instance = null;

    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<GameManager>();
                if (m_Instance == null)
                {
                    GameObject newObj = new GameObject("GameManager");
                    m_Instance = Instantiate(newObj).AddComponent<GameManager>();
                }
            }

            return m_Instance;
        }
    }

    private void Awake()
    {
        for (var index = 0; index < m_teamsUnits.Length; index++)
            m_teamsUnits[index] = new List<Unit>();
    }

    private void OnEnable()
    {
        m_unitSelection.SetObserver(m_teamsUnits[(int) ETeam.Team1]);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_isSelecting)
            {
                m_unitSelection.OnSelectionBegin(Input.mousePosition);
                m_isSelecting = true;
            }
        }

        if (m_isSelecting)
            m_unitSelection.OnSelectionProcess(camera, Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            m_unitSelection.OnSelectionEnd();
            m_isSelecting = false;
        }
    }

    private void OnGUI()
    {
        if (m_isSelecting)
            m_unitSelection.DrawGUI(Input.mousePosition);
    }

    public void RegisterUnit(ETeam team, Unit unit)
    {
        m_teamsUnits[(int) team].Add(unit);
    }

    public void UnregisterUnit(ETeam team, Unit unit)
    {
        m_teamsUnits[(int) team].Remove(unit);
    }
}
```

You can now select your entities.
You can change the style of selection zone thanks to the style variable in "UnitSelection"
