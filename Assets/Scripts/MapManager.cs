using SimpleJSON;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Public singleton class that manages data/info related to the electorates and the electorate maps/meshes
/// </summary>
public sealed class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public GameObject generalElectoratesMap;
    public GameObject māoriElectoratesMap;
    public Material lineMaterial;
    public TextAsset generalElectoratesMeshJSON;
    public TextAsset māoriElectoratesMeshJSON;
    public GameObject insets;

    public TextAsset electorateDataJSON;

    private List<GameObject> electorates = new List<GameObject>();

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
    public static MapManager Instance
    {
        get { return instance; }
    }

    public List<GameObject> Electorates
    {
        get { return electorates; }
        set { electorates = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        ParseMapData(generalElectoratesMeshJSON, "GeneralElectoratesMesh", false);
        ParseMapData(māoriElectoratesMeshJSON, "MāoriElectoratesMesh", true);

        foreach (Transform electorateObject in generalElectoratesMap.transform)
        {
            string electorateName = electorateObject.name;
            if (electorateObject.GetComponent<MeshCollider>() == null)
            {
                MeshCollider collider = electorateObject.AddComponent<MeshCollider>();
                electorateObject.AddComponent<Electorate>();
                electorateObject.tag = "Electorate";
                electorates.Add(electorateObject.gameObject);
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

        foreach (Transform electorateObject in māoriElectoratesMap.transform)
        {
            string electorateName = electorateObject.name;
            if (electorateObject.GetComponent<MeshCollider>() == null)
            {
                MeshCollider collider = electorateObject.AddComponent<MeshCollider>();
                electorateObject.AddComponent<Electorate>();
                electorateObject.tag = "Electorate";
                electorates.Add(electorateObject.gameObject);
            }
        }

        var parsedElectorateDataJSON = JSON.Parse(electorateDataJSON.text);
        var electoratesData = parsedElectorateDataJSON["Electorates"].AsArray;
        foreach (JSONNode electorate in electoratesData)
        {
            var candidates = electorate["Candidates"];
            var electorateName = electorate["Name"];

            foreach (JSONNode candidate in candidates)
            {
                var candidateName = candidate["Name"];
                var candidateParty = candidate["PartyAffiliation"];

                GameObject associatedElectorate = electorates.Find(electorate => electorate.name.Equals(electorateName));
                Party associatedParty = GameManager.Instance.GameState.AvailableParties.Find(party => party.PartyName.Equals(candidateParty));

                if (associatedParty != null)
                {
                    associatedElectorate.GetComponent<Electorate>().Candidates.Add(new Candidate(candidateName, associatedParty));
                }
            }
        }

        ToggleMāoriElectoratesVisibility();

    }
    void ParseMapData(TextAsset JSONData, string meshContainerName, bool māoriElectorates)
    {
        var parsedMeshJSON = JSON.Parse(JSONData.text);
        var features = parsedMeshJSON["features"].AsArray;
        int counter = 0;
        foreach (JSONNode feature in features)
        {
            var geometry = feature["geometry"];
            var coordinates = geometry["coordinates"].AsArray;

            var electorateName = "";
            if (māoriElectorates != true)
            {
                electorateName = feature["properties"]["GED2020__1"];
            }
            else
            {
                electorateName = feature["properties"]["MED2020__1"];
            }

            GameObject mapContainer = GameObject.Find(meshContainerName);

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
    }

    void DuplicateElectorate(GameObject electorateObject, string insetName)
    {
        GameObject inset = GameObject.Find(insetName);
        GameObject duplicateElectorate = Instantiate(electorateObject.gameObject, electorateObject.transform.position, electorateObject.transform.rotation);

        duplicateElectorate.transform.SetParent(inset.transform, false);
        duplicateElectorate.transform.localScale = Vector3.one;
        duplicateElectorate.name = duplicateElectorate.name.Replace("(Clone)", "");
        duplicateElectorate.GetComponent<Electorate>().OriginalElectorate = electorateObject;
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

    public void HighlightElectorate(GameObject originalElectorate, GameObject duplicateElectorate)
    {
        duplicateElectorate.GetComponent<Renderer>().material.color = Color.white;

        if (originalElectorate != null)
        {
            originalElectorate.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void UnhighlightElectorate(GameObject originalElectorate, GameObject duplicateElectorate)
    {
        duplicateElectorate.GetComponent<Renderer>().material.color = Color.gray;

        if (originalElectorate != null)
        {
            originalElectorate.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    public void ToggleGeneralElectoratesVisibility()
    {
        generalElectoratesMap.SetActive(!generalElectoratesMap.activeSelf);
        insets.SetActive(!insets.activeSelf);

        foreach (Transform electorateObject in generalElectoratesMap.transform)
        {
            if (electorateObject.transform.childCount > 0)
            {
                foreach (Transform child in electorateObject.transform)
                {
                    child.GetComponent<LineRenderer>().enabled = !child.GetComponent<LineRenderer>().enabled;
                }
            }
            else
            {
                electorateObject.GetComponent<LineRenderer>().enabled = !electorateObject.GetComponent<LineRenderer>().enabled;
            }
        }

        foreach (Transform inset in insets.transform)
        {
            foreach (Transform insetElectorate in inset)
            {

                if (insetElectorate.childCount > 0)
                {
                    foreach (Transform child in insetElectorate.transform)
                    {
                        child.GetComponent<LineRenderer>().enabled = !child.GetComponent<LineRenderer>().enabled;
                    }
                }
                else
                {
                    insetElectorate.GetComponent<LineRenderer>().enabled = !insetElectorate.GetComponent<LineRenderer>().enabled;
                }
            }
        }
    }

    public void ToggleMāoriElectoratesVisibility()
    {
        if (māoriElectoratesMap.activeSelf == true)
        {
            māoriElectoratesMap.SetActive(false);
        }
        else
        {
            māoriElectoratesMap.SetActive(true);
        }

        foreach (Transform electorateObject in māoriElectoratesMap.transform)
        {
            if (electorateObject.transform.childCount > 0)
            {
                foreach (Transform child in electorateObject.transform)
                {
                    child.GetComponent<LineRenderer>().enabled = !child.GetComponent<LineRenderer>().enabled;
                }
            }
            else
            {
                electorateObject.GetComponent<LineRenderer>().enabled = !electorateObject.GetComponent<LineRenderer>().enabled;
            }
        }
    }
}