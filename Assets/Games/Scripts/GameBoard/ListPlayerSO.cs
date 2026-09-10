using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name;
    public Sprite avatar;
}

[CreateAssetMenu(fileName = "ListPlayerSO", menuName = "Scriptable Objects/ListPlayerSO")]
public class ListPlayerSO : ScriptableObject
{
    public List<PlayerData> players;
}
