using SimpleJSON;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NZElectorateMap : MonoBehaviour
{
    public GameObject electorateMap;
    public Material lineMaterial;
    public TextAsset jsonFile;

    readonly List<string> aucklandElectorates = new List<string>
        {
            "Auckland Central", "Botany", "East Coast Bays", "Epsom", "Kelston", "Māngere", "Manurewa", "Maungakiekie", "Mt Albert", "Mt Roskill",
            "New Lynn", "North Shore", "Northcote", "Pakuranga", "Panmure-Ōtāhuhu", "Takanini", "Tāmaki", "Te Atatū", "Upper Harbour","Whangaparāoa"
        };

    List<string> christchurchElectorates = new List<string>
        {
            "Christchurch Central", "Christchurch East", "Ilam", "Wigram"
        };
    List<string> wellingtonElectorates = new List<string>
        {
            "Hutt South", "Mana", "Ōhāriu", "Remutaka", "Rongotai", "Wellington Central"
        };
    List<string> hamiltonElectorates = new List<string>
        {
            "Hamilton East", "Hamilton West"
        };

    void Start()
    {
        var parsedJSON = JSON.Parse(jsonFile.text);
        var features = parsedJSON["features"].AsArray;
        int counter = 0;
        foreach (JSONNode feature in features)
        {
            var geometry = feature["geometry"];
            var coordinates = geometry["coordinates"].AsArray;
            var electorateName = feature["properties"]["GED2020__1"];

            GameObject mapContainer = GameObject.Find("ElectoratesMesh");

            GameObject electorateGO = mapContainer.transform.GetChild(counter).gameObject;
            electorateGO.name = electorateName;
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
            counter++;
        }

        foreach (Transform electorateObject in electorateMap.transform)
        {
            string electorateName = electorateObject.name;
            if (electorateObject.GetComponent<MeshCollider>() == null)
            {
                MeshCollider collider = electorateObject.AddComponent<MeshCollider>();
                electorateObject.AddComponent<Electorate>();
                electorateObject.tag = "Electorate";
            }

            if (electorateName.Equals("Tauranga") || electorateName.Equals("Palmerston North"))
            {
                DuplicateElectorate(electorateObject.gameObject, electorateName + " Inset");
            }

            foreach (string electorate in hamiltonElectorates)
            {
                if (electorate.Equals(electorateName))
                {
                    DuplicateElectorate(electorateObject.gameObject, "Hamilton Inset");
                }
            }

            foreach (string electorate in aucklandElectorates)
            {
                if (electorate.Equals(electorateName))
                {
                    DuplicateElectorate(electorateObject.gameObject, "Auckland Inset");
                }
            }

            foreach (string electorate in christchurchElectorates)
            {
                if (electorate.Equals(electorateName))
                {
                    DuplicateElectorate(electorateObject.gameObject, "Christchurch Inset");
                }
            }

            foreach (string electorate in wellingtonElectorates)
            {
                if (electorate.Equals(electorateName))
                {
                    DuplicateElectorate(electorateObject.gameObject, "Wellington Inset");
                }
            }
        }
    }

    void DuplicateElectorate(GameObject electorateObject, string insetName)
    {
        GameObject inset = GameObject.Find(insetName);
        GameObject duplicateElectorate = Instantiate(electorateObject.gameObject, electorateObject.transform.position, electorateObject.transform.rotation);

        duplicateElectorate.transform.SetParent(inset.transform, false);
        duplicateElectorate.transform.localScale = Vector3.one;
    }


    void CreateLine(JSONArray coordinateSet, GameObject parentGO)
    {
        LineRenderer lineRenderer = parentGO.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.widthMultiplier = 0.015f;
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

        Vector2[] points = new Vector2[coordinateSet.Count];
        Vector3[] positions = new Vector3[coordinateSet.Count];

        float xPositionOffset = -168f;
        float yPositionOffset = 48.8f;
        for (int i = 0; i < coordinateSet.Count; i++)
        {
            float x = coordinateSet[i][0].AsFloat + xPositionOffset;
            float y = coordinateSet[i][1].AsFloat + yPositionOffset;
            positions[i] = new Vector3(x, y, 0);
            points[i] = new Vector2(x, y);
        }
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameManager.Instance.GameState.SelectedElectorate = hit.collider.gameObject;
            }
            else
            {
                GameManager.Instance.GameState.SelectedElectorate = null;
            }
        }
    }
}