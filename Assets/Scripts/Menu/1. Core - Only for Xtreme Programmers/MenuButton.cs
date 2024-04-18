using TMPro;
using UnityEngine;

public class MenuButton : MenuSelectable
{
    [Header("Text")]
    [SerializeField]
    string textString;
    [SerializeField]
    Color textColor;
    [SerializeField]
    float textSize;
    [SerializeField]
    TMP_FontAsset textFontAsset;
    [SerializeField]
    TextMeshProUGUI textMesh;

    public override void Initiate()
    {
        if (textMesh)
        {
            textMesh.text = textString;
            textMesh.color = textColor;
            textMesh.fontSize = textSize;
            textMesh.font = textFontAsset;
        }

        base.Initiate();
    }

    public override void Select()
    {
        base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    public override void Confirm(Vector3 mousePos)
    {
        var actions = GetComponents<ButtonAction>();

        foreach (var action in actions)
            action.Action();

        base.Confirm(mousePos);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
            UnityEditor.EditorApplication.update += Initiate;
    }
#endif
}
