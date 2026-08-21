using UnityEngine;

public class PlayerChoicePreview : MonoBehaviour
{
    [SerializeField] private Transform playerPreviewContent;

    public Transform GetPlayerPreviewContentTransform()
    {
        return playerPreviewContent;
    }
}

