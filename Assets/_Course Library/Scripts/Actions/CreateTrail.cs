using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script creates a trail at the location of a gameobject with a particular width and color.
/// </summary>

public class CreateTrail : MonoBehaviour
{
    public GameObject trailPrefab = null;

    private float width = 0.05f;
    private Color color = Color.white;

    private GameObject currentTrail = null;

    [SerializeField] private Transform CurrentDrawParent;
    public void StartTrail()
    {
        if (!currentTrail)
        {
            currentTrail = Instantiate(trailPrefab, transform.position, transform.rotation, transform);
            ApplySettings(currentTrail);
        }
    }

    private void ApplySettings(GameObject trailObject)
    {
        TrailRenderer trailRenderer = trailObject.GetComponent<TrailRenderer>();
        trailRenderer.widthMultiplier = width;
        trailRenderer.startColor = color;
        trailRenderer.endColor = color;
    }

    public void EndTrail()
    {
        if (currentTrail)
        {
            GameObject newTrailObj = new GameObject();
            newTrailObj.transform.parent = CurrentDrawParent;
            newTrailObj.AddComponent<MeshRenderer>();
            newTrailObj.AddComponent<MeshFilter>();
            var mesh = new Mesh();
            currentTrail.GetComponent<TrailRenderer>().BakeMesh(mesh);
            newTrailObj.GetComponent<MeshRenderer>().material = currentTrail.GetComponent<TrailRenderer>().material;
            newTrailObj.GetComponent<MeshFilter>().mesh = mesh;
            newTrailObj.tag = currentTrail.tag;
            //currentTrail.transform.parent = null;
            //currentTrail.transform.parent = CurrentDrawParent;
            //currentTrail = null;
            Destroy(currentTrail);
        }
    }

    public void SetWidth(float value)
    {
        width = value;
    }

    public void SetColor(Color value)
    {
        color = value;
    }
}
