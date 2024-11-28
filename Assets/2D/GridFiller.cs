//Requires GridLayout Group Attach this to a UI Panel

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridFiller : MonoBehaviour
{
    public int TargetRowAmt = 4;
    public int TargetColAmt = 4;

    public int NumberOfElements;

    RectTransform rt;

    List<GameObject> GridElements = new List<GameObject>();
    public GameObject prefab;

    public GridLayoutGroup layoutgroup;
    public bool canUpdate = false;

    void Awake()
    {
        if (!layoutgroup)
            Debug.LogError("GridLayoutGroup not found");
        rt = GetComponent<RectTransform>();
    }
    void Start()
    {
        UpdateCellSize(layoutgroup);
        Spawn();
    }
    void Update()
    {
        if (canUpdate)
        {
            UpdateCellSize(layoutgroup);
        }
    }
    void UpdateCellSize(GridLayoutGroup layoutgroup)
    {
        Vector2 cellSize = rt.sizeDelta;
        layoutgroup.cellSize = new Vector2(cellSize.x / TargetColAmt, cellSize.y / TargetRowAmt);
        layoutgroup.constraint = GridLayoutGroup.Constraint.Flexible;
    }
    void Spawn()
    {
        while(NumberOfElements != GridElements.Count)
        {
            GameObject go = GameObject.Instantiate(prefab, transform);
            GridElements.Add(go);
        }
    }
}
