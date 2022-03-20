using EntSelect;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    private bool m_isSelected = false;
    private Material m_materialRef;
    private Color m_baseColor = Color.white;

    protected void Awake()
    {
        m_materialRef = GetComponent<Renderer>().material;
        m_baseColor = m_materialRef.color;
    }

    public void SetSelected(bool selected)
    {
        m_isSelected = selected;
        m_materialRef.color = m_isSelected ? Color.red : m_baseColor;
    }
    
    public bool IsSelected()
    {
        return m_isSelected;
    }
}
