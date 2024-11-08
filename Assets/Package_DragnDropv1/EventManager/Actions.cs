using System;
using UnityEngine;

public static class Actions
{
    public static Action<String> answercheck;
    public static Action onDrag;
    public static Action onDragHighlight;
    public static Action onDragRemoveHighlight;
    public static Action<GameObject> onDropHighlight;
    public static Action<GameObject> onDropRemoveHighlight;
    public static Action<GameObject> onItemDropped;
}
