using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameCell : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image imageBack;
    [SerializeField] private Image imageFront;

    [Header("Flip Settings")]
    [SerializeField] private float flipDuration = 0.2f;

    public int GameCell_ID { get; private set; }

    private bool isSelected;
    private bool isFlipping;

    private GameBoard gameBoard;

    public Action<int> onCellSelected;

    private void Start()
    {
        gameBoard = GetComponentInParent<GameBoard>();

        if (gameBoard == null)
        {
            Debug.LogError("[GameCell] GameBoard not found in the scene.");
        }

        ShowBack();
    }

    public void InitializeCell(
        int id,
        Sprite sprite,
        Action<int> onCellSelected)
    {
        GameCell_ID = id;
        this.onCellSelected = onCellSelected;

        if (imageFront == null)
        {
            imageFront = GetComponent<Image>();
        }

        if (imageFront != null)
        {
            imageFront.sprite = sprite;
        }
        else
        {
            Debug.LogError(
                "[GameCell] Image component not found on GameCell: " + name);
        }

        isSelected = false;
        isFlipping = false;

        ShowBack();
    }

    /// <summary>
    /// Reveal the card from back to front.
    /// </summary>
    public void Reveal()
    {
        if (gameBoard != null && gameBoard.LockChoiceCells)
        {
            Debug.Log("[GameCell] Cell selection is locked. Ignoring reveal.");
            return;
        }

        if (isSelected || isFlipping)
            return;

        isSelected = true;

        FlipToFront();
    }

    /// <summary>
    /// Hide the card from front to back.
    /// </summary>
    public void Hide()
    {
        if (!isSelected || isFlipping)
            return;

        isSelected = false;

        FlipToBack();
    }

    public void ResetState()
    {
        isSelected = false;

        transform.DOKill();

        isFlipping = false;

        ShowBack();
    }

    private void FlipToFront()
    {
        isFlipping = true;

        transform.DOKill();

        transform.DOScaleX(0f, flipDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                imageBack.gameObject.SetActive(false);
                imageFront.gameObject.SetActive(true);

                transform.DOScaleX(1f, flipDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        isFlipping = false;

                        // Notify GameBoard after the card is revealed.
                        onCellSelected?.Invoke(GameCell_ID);
                    });
            });
    }

    private void FlipToBack()
    {
        isFlipping = true;

        transform.DOKill();

        transform.DOScaleX(0f, flipDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                imageFront.gameObject.SetActive(false);
                imageBack.gameObject.SetActive(true);

                transform.DOScaleX(1f, flipDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        isFlipping = false;
                    });
            });
    }

    private void ShowBack()
    {
        transform.DOKill();

        transform.localScale = Vector3.one;

        imageBack.gameObject.SetActive(true);
        imageFront.gameObject.SetActive(false);
    }
}