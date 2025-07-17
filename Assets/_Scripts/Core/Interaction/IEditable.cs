using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEditable
{
    public string EditorTitle { get; }
    public List<EditorParameter> GetEditorParameters();
}
public struct EditorParameter
{
    public string Name;
    public int Value;
    public Action<int> OnChanged;
}

