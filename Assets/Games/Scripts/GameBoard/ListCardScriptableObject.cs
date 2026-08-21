using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListCardScriptableObject", menuName = "Scriptable Objects/ListCardScriptableObject")]
public class ListCardScriptableObject : ScriptableObject
{
    public List<Sprite> sprites;
}
