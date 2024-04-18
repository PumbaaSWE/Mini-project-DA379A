using UnityEngine;
using UnityEngine.UI;

public class Outline : MonoBehaviour
{
    [SerializeField]
    RectTransform left, right, top, bottom;
    [SerializeField]
    Image leftImage, rightImage, topImage, bottomImage;

    public enum OutlineType
    {
        Inner,
        Middle,
        Outer
    }

    float _width;
    public float width
    {
        get
        {
            return  _width;
        }

        set
        {
            _width = value;
            UpdateRects();
        }
    }

    Color _color;
    public Color color
    {
        get
        {
            return _color;
        }

        set
        {
            _color = value;
            UpdateColor();
        }
    }

    OutlineType _outlineType;
    public OutlineType outlineType
    {
        get
        {
            return _outlineType;
        }

        set
        {
            _outlineType = value;
            UpdateRects();
        }
    }

    private void UpdateRects()
    {
        if (left && right && top && bottom)
        {
            switch (outlineType)
            {
                case OutlineType.Inner:
                    left.offsetMin = new Vector2(0, _width);
                    left.offsetMax = new Vector2(_width, -_width);
                    right.offsetMin = new Vector2(-_width, _width);
                    right.offsetMax = new Vector2(0, -_width);
                    top.offsetMin = new Vector2(0, -_width);
                    top.offsetMax = new Vector2(0, 0);
                    bottom.offsetMin = new Vector2(0, 0);
                    bottom.offsetMax = new Vector2(0, _width);
                    break;
                case OutlineType.Middle:

                    left.offsetMin = new Vector2(-_width / 2, _width / 2);
                    left.offsetMax = new Vector2(_width / 2, -_width / 2);
                    right.offsetMin = new Vector2(-_width / 2, _width / 2);
                    right.offsetMax = new Vector2(_width / 2, -_width / 2);
                    top.offsetMin = new Vector2(-_width / 2, -_width / 2);
                    top.offsetMax = new Vector2(_width / 2, _width / 2);
                    bottom.offsetMin = new Vector2(-_width / 2, -_width / 2);
                    bottom.offsetMax = new Vector2(_width / 2, _width / 2);
                    break;
                case OutlineType.Outer:

                    left.offsetMin = new Vector2(-_width, -_width);
                    left.offsetMax = new Vector2(0, 0);
                    right.offsetMin = new Vector2(0, 0);
                    right.offsetMax = new Vector2(_width, _width);
                    top.offsetMin = new Vector2(-_width, 0);
                    top.offsetMax = new Vector2(0, _width);
                    bottom.offsetMin = new Vector2(0, -_width);
                    bottom.offsetMax = new Vector2(_width, 0);
                    break;
            }
        }
    }

    private void UpdateColor()
    {
        if (leftImage && rightImage && topImage && bottomImage)
        {
            leftImage.color =
            rightImage.color =
            topImage.color =
            bottomImage.color =
            color;
        }
    }
}
