using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Scroll_Wheel : MonoBehaviour
{
    [SerializeField] GameObject currentSelected;
    [SerializeField] GameObject previouslySelected;
    [SerializeField] RectTransform currentSelectedTranform;

    [SerializeField] RectTransform contentPanel;
    [SerializeField] ScrollRect scrollRect;

    private void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;

        if(currentSelected != null)
        {
            if(currentSelected != previouslySelected)
            {
                previouslySelected = currentSelected;
                currentSelectedTranform = currentSelected.GetComponent<RectTransform>();
                SnapTo(currentSelectedTranform);
            }
        }
    }

    private void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 newPosition  = 
            (Vector2)scrollRect.transform.InverseTransformDirection(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformDirection(target.position);

        //
        newPosition.x = 0;
        contentPanel.anchoredPosition = newPosition;
    }
}
