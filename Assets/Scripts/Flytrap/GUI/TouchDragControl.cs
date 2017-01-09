using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class TouchDragControl : EventDispatcherBase, IPointerDownHandler, IPointerUpHandler {

    public GameObject TailPrefab;
    public GameObject PolygonPrefab;
    public float SampleFreq = 0.02f;
    public bool TouchEnabled = false;
    protected GameObject Tail;
    protected List<Vector2> Samples;
    protected GameObject SamplePolygon;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        if (TouchEnabled)
        {
            StartDraw();
        }
        else
        {
            eventDispatcher.dispatchEvent(new CaptureEvent(CaptureEvent.ON_TAP));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");
        
        if (TouchEnabled)
        {
            StopDraw();
        }
    }

    public void StartDraw()
    {
        Vector3 pos = GetWoldCurrentMousePoint();
        //Debug.Log("DOWN " + pos);
        Samples = new List<Vector2>();
        Samples.Add(pos);
        Tail = Instantiate(TailPrefab, pos, Quaternion.identity) as GameObject;
        Tail.name = "Tail";
        Tail.gameObject.GetComponent<TrailRenderer>().sortingLayerName = Tail.GetComponent<SpriteRenderer>().sortingLayerName;
        Tail.gameObject.GetComponent<TrailRenderer>().sortingOrder = 2;
        eventDispatcher.dispatchEvent(new CaptureEvent(CaptureEvent.ON_CAPTURE_START));
        InvokeRepeating("UpdateDrawing",SampleFreq, SampleFreq);
    }

    public void StopDraw()
    {
        Vector3 pos = GetWoldCurrentMousePoint();
        //Debug.Log("UP" + pos);
        CancelInvoke("UpdateDrawing");
        Destroy(Tail);
        CreateSampleCollider( Samples );      
    }

    private void CreateSampleCollider(List<Vector2> samples)
    {
        SamplePolygon = Instantiate(PolygonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        SamplePolygon.name = "Sample";
        Vector2[] vertices2D = new Vector2[samples.Count + 1];
        samples.CopyTo(vertices2D);
        vertices2D[vertices2D.Length - 1] = vertices2D[0];
        // Use the triangulator to get indices for creating triangles
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
        Array.Reverse(indices);
        //Debug.Log(vertices2D.Length);
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        // Set up game object with mesh;
        MeshRenderer meshRenderer = SamplePolygon.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        MeshFilter filter = SamplePolygon.AddComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = msh;
        PolygonCollider2D PolygonCollider = SamplePolygon.AddComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
        PolygonCollider.SetPath(0, vertices2D);

        AssetManager.Instance.CheckMatches( TestTrappedCreatures(PolygonCollider) );
        Destroy(SamplePolygon.gameObject);
    }

    protected List<Creature> TestTrappedCreatures(PolygonCollider2D col)
    {
        Creature[] creatures = FindObjectsOfType<Creature>();
        List<Creature> creaturesCaptured = new List<Creature>();
        string currentType = "";

        for (int i = 0; i < creatures.Length; i++)
        {
            if (col.OverlapPoint(creatures[i].gameObject.transform.position))
            {
                Creature creature = creatures[i].gameObject.GetComponent<Creature>();
                if (creature.type == currentType || currentType == "")
                {
                    creaturesCaptured.Add(creature);
                    currentType = creature.type;
                }
                else
                {
                    creaturesCaptured.Clear();
                    break;
                }
                    
            }
            
        }

        return creaturesCaptured;
    }

    protected void UpdateDrawing()
    {
        Vector3 pos = GetWoldCurrentMousePoint();
        //Debug.Log(pos);
        Tail.transform.position = pos;
        Samples.Add(pos);
    }

    protected Vector3 GetWoldCurrentMousePoint()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(pos.x, pos.y, 0f);
    }
}
