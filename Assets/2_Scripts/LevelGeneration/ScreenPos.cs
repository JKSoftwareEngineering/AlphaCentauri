using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScreenPos : MonoBehaviour
{
    [HideInInspector] public GameObject thisShipArrow;
    [HideInInspector] private RectTransform thisArrowRect;
    // Start is called before the first frame update
    void Start()
    {
        thisArrowRect = thisShipArrow.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // map the arrow to the viewport
        Vector3 arrowPos = Camera.main.WorldToScreenPoint(this.transform.position);
        // assume that the arrow is visible from the beginning
        bool visible = true;
        // get the frustum from the player camera
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        // get this positions on the viewport
        Vector3 point = transform.position;
        // check all 4 frustums to make sure that they are all in the screen
        foreach (Plane plane in planes)
        {
            // if one is not in th escreen then the arrow is not visable
            if (plane.GetDistanceToPoint(point) < 0)
            {
                visible = false;
            }
        }
        // set the arrows to inactive to remove mirrioring
        try
        {
            thisShipArrow.SetActive(visible);
            // move the arrows as needed to stay with the ship
            thisShipArrow.transform.position = arrowPos;
        }
        catch
        {}
    }
}
