using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float smoothing;
    public GameObject darkeningPanel;
    public GameObject pauseButton;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;
    private bool touched;
    private int pointerID;
    void Awake()
    {
        direction = Vector2.zero;
        touched = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        
        if (!touched)
        {
            darkeningPanel.SetActive(false);
            pauseButton.SetActive(false);
            Time.timeScale = 1.0f;
            //touchAnimation.Play();
            touched = true;
            pointerID = data.pointerId;
            origin = data.position;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.pointerId == pointerID)
        {
            Vector2 currentPosition = data.position;
            Vector2 directionRaw = currentPosition - origin;
            origin = currentPosition;
            if (Mathf.Abs(directionRaw.x) < 3 && Mathf.Abs(directionRaw.y) < 3)
            {
                Debug.Log(directionRaw);
                direction = Vector3.zero;
                return;
            }
            direction = directionRaw.normalized;
            //Debug.Log(directionRaw);
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
       
        if (data.pointerId == pointerID)
        {
            darkeningPanel.SetActive(true);
            pauseButton.SetActive(true);
            Time.timeScale = 0.2f;
            direction = Vector3.zero;
            touched = false;
        }
    }
    private int count = 0;
    public Vector2 GetDirection()
    {
        smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);
        Vector2 result = new Vector2(direction.x, direction.y);
        direction = Vector3.zero;
        return result;
    }
}
