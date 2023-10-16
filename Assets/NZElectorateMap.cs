using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using JetBrains.Annotations;

public class NZElectorateMap : MonoBehaviour
{
    public Material lineMaterial;
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("general-electorates-2020-edited");
        var parsedJSON = JSON.Parse(jsonFile.text);
        var features = parsedJSON["features"].AsArray;

        foreach (JSONNode feature in features)
        {
            var geometry = feature["geometry"];
            var coordinates = geometry["coordinates"].AsArray;
            var electorateName = feature["properties"]["GED2020__2"];


            GameObject electorateGO = new GameObject(electorateName);
            electorateGO.name = electorateName;
            GameObject mapContainer = GameObject.Find("MapContainer");
            electorateGO.transform.parent = mapContainer.transform;

            if (geometry["type"] == "MultiPolygon")
            {
                int polyIndex = 0;
                foreach (JSONNode multiPolygon in coordinates)
                {
                    foreach (JSONNode polygon in multiPolygon.AsArray)
                    {
                        GameObject polygonGO = new GameObject("Polygon_" + polyIndex);
                        polygonGO.transform.parent = electorateGO.transform;
                        CreateLine(polygon.AsArray, polygonGO);
                        polyIndex++;
                    }
                }
            }
            else if (geometry["type"] == "Polygon")
            {
                CreateLine(coordinates[0].AsArray, electorateGO);
            }
        }
    }

    void CreateLine(JSONArray coordinateSet, GameObject parentGO)
    {
        LineRenderer lineRenderer = parentGO.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.widthMultiplier = 0.01f;
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

        PolygonCollider2D polyCollider = parentGO.AddComponent<PolygonCollider2D>();

        Vector2[] points = new Vector2[coordinateSet.Count];
        Vector3[] positions = new Vector3[coordinateSet.Count];

        for (int i = 0; i < coordinateSet.Count; i++)
        {
            float x = coordinateSet[i][0].AsFloat;
            float y = coordinateSet[i][1].AsFloat;
            positions[i] = new Vector3(x, y, 0);
            points[i] = new Vector2(x, y);
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        lineRenderer.useWorldSpace = false;

        polyCollider.pathCount++;
        polyCollider.SetPath(polyCollider.pathCount - 1, points);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit)
            {

                Debug.Log("Clicked on " + hit.transform.name);
            }
        }
    }
}
