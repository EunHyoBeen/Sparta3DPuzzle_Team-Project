using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] private BuildableElement curElement;
    private MapEditInputController controller;

    private bool canBuild;

    private void Awake()
    {
        controller = GetComponent<MapEditInputController>();
    }

    private void Start()
    {
        controller.OnLeftButtonEvent += TryBuildElement;
        CreateBuildElement();
    }


    //TODO 나중에는 UI에서 값 받아와서 생성 
    public void CreateBuildElement()
    {
        curElement = null;
        GameObject obj = Instantiate(Resources.Load<GameObject>($"Map/Space/terrain"));
        obj.AddComponent<Rigidbody>().freezeRotation = true;
        obj.GetComponent<Rigidbody>().useGravity = false;

        curElement = obj.GetComponent<BuildableElement>();
        curElement.EnterBuildMode();

        StartCoroutine(MoveCurElement());
    }

    public void DeleteBuildElement()
    {
        curElement = null;
    }

    private IEnumerator MoveCurElement()
    {
        float snapSpeed = 10f;

        while (curElement != null)
        {
            yield return null;

            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            curElement.transform.position = mouseWorldPosition;
        }
    }




    private void TryBuildElement()
    {
        if (curElement == null && !curElement.CanBuild)
            return;

        StopCoroutine(MoveCurElement());

        curElement.Built();
        DeleteBuildElement();
    }
}