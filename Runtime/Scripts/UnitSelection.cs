using UnityEngine;

namespace EntSelect
{
    /// <summary>
    /// <see cref="https://hyunkell.com/blog/rts-style-unit-selection-in-unity-5/"/>
    /// </summary>
    public class UnitSelection<T> : MonoBehaviour
        where T : MonoBehaviour, ISelectable
    {
        [System.Serializable]
        public struct Style
        {
            public float thickness;
            public Color fillColor;
            public Color edgeColor; 
        }

        public Camera camera;
        public Style style = new Style
        {
            thickness = 2f,
            fillColor = new Color(0.8f, 0.8f, 0.95f, 0.25f),
            edgeColor = new Color(0.8f, 0.8f, 0.95f)
        };
        
        bool m_isSelecting = false;
        Vector3 m_mousePosition1;
        private T[] m_units;
        
        
        void Update()
        {
            // If we press the left mouse button, save mouse location and begin selection
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_isSelecting)
                {
                    m_units = FindObjectsOfType<T>();

                    foreach (T unit in m_units)
                    {
                        unit.SetSelected(false);
                    }
                }

                m_isSelecting = true;
                m_mousePosition1 = Input.mousePosition;
            }

            if (m_isSelecting)
            {
                foreach (T unit in m_units)
                {
                    unit.SetSelected(IsWithinSelectionBounds(unit.gameObject));
                }
            }

            // If we let go of the left mouse button, end selection
            if (Input.GetMouseButtonUp(0))
            {
                m_isSelecting = false;
            }
        }

        void OnGUI()
        {
            if (m_isSelecting)
            {
                // Create a rect from both mouse positions
                Rect rect = Utils.GetScreenRect(m_mousePosition1, Input.mousePosition);
                Utils.DrawScreenRect(rect, style.fillColor);
                Utils.DrawScreenRectBorder(rect, style.thickness, style.edgeColor);
            }
        }

        public bool IsWithinSelectionBounds(GameObject gameObject)
        {
            if (!m_isSelecting)
                return false;

            Bounds viewportBounds =
                Utils.GetViewportBounds(camera, m_mousePosition1, Input.mousePosition);

            return viewportBounds.Contains(
                camera.WorldToViewportPoint(gameObject.transform.position));
        }
    }
}