using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

[DisallowMultipleComponent]
public class Outline : MonoBehaviour
{
    public enum Mode
    {
        OutlineAll,
        OutlineVisible,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly
    }

    public Mode OutlineMode
    {
        get { return _OutlineMode; }
        set
        {
            _OutlineMode = value;
            _NeedsUpdate = true;
        }
    }

    public Color OutlineColor
    {
        get { return _OutlineColor; }
        set
        {
            _OutlineColor = value;
            _NeedsUpdate = true;
        }
    }

    public float OutlineWidth
    {
        get { return _OutlineWidth; }
        set
        {
            _OutlineWidth = value;
            _NeedsUpdate = true;
        }
    }

    [Serializable]
    private class ListVector3
    {
        public List<Vector3> _Data;
    }

    [Tooltip ("The type of outlining to apply to the mesh. \nAll: Outline entire object even when partly obscured\nVisible: Outline only the parts of the object that can be seen")]
    [SerializeField] private Mode _OutlineMode;
    [Tooltip ("The color of the outline to display.")]
    [SerializeField] private Color _OutlineColor = Color.white;
    [Tooltip ("How thick the outline of the object appears.")]
    [SerializeField, Range (0f, 10f)] private float _OutlineWidth = 2f;
    [Tooltip ("The material used to mask the texture of the model.")]
    [SerializeField] private Material _OutlineMaskMaterial = null;
    [Tooltip ("The material used to create the outline fill around the object.")]
    [SerializeField] private Material _OutlineFillMaterial = null;

    [Header ("Optional")]

    [Tooltip ("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
    + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
    [SerializeField] private bool _PrecomputeOutline = false;
    [HideInInspector]
    [SerializeField] private List<Mesh> _BakeKeys = new List<Mesh> ();
    [HideInInspector]
    [SerializeField] private List<ListVector3> _BakeValues = new List<ListVector3> ();

    private bool _NeedsUpdate;
    private Renderer[] _Renderers;
    private static HashSet<Mesh> _RegisteredMeshes = new HashSet<Mesh> ();

    void Awake ()
    {
        // Cache renderers
        _Renderers = GetComponentsInChildren<Renderer> ();

        // Instantiate outline materials
        //_OutlineMaskMaterial = Instantiate (Resources.Load<Material> (@"Materials/OutlineMask"));
        //_OutlineFillMaterial = Instantiate (Resources.Load<Material> (@"Materials/OutlineFill"));

        _OutlineMaskMaterial.name = "OutlineMask (Instance)";
        _OutlineFillMaterial.name = "OutlineFill (Instance)";

        // Retrieve or generate smooth normals
        LoadSmoothNormals ();

        // Apply material properties immediately
        _NeedsUpdate = true;
    }

    void OnEnable ()
    {
        foreach (var renderer in _Renderers)
        {
            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList ();

            materials.Add (_OutlineMaskMaterial);
            materials.Add (_OutlineFillMaterial);

            renderer.materials = materials.ToArray ();
        }
    }

    void OnValidate ()
    {
        // Update material properties
        _NeedsUpdate = true;

        // Clear cache when baking is disabled or corrupted
        if (!_PrecomputeOutline && _BakeKeys.Count != 0 || _BakeKeys.Count != _BakeValues.Count)
        {
            _BakeKeys.Clear ();
            _BakeValues.Clear ();
        }

        // Generate smooth normals when baking is enabled
        if (_PrecomputeOutline && _BakeKeys.Count == 0)
        {
            Bake ();
        }
    }

    void Update ()
    {
        if (_NeedsUpdate)
        {
            _NeedsUpdate = false;

            UpdateMaterialProperties ();
        }
    }

    void OnDisable ()
    {
        foreach (var renderer in _Renderers)
        {
            // Remove outline shaders
            var materials = renderer.sharedMaterials.ToList ();

            materials.Remove (_OutlineMaskMaterial);
            materials.Remove (_OutlineFillMaterial);

            renderer.materials = materials.ToArray ();
        }
    }

    void OnDestroy ()
    {
        //// Destroy material instances
        //Destroy (_OutlineMaskMaterial);
        //Destroy (_OutlineFillMaterial);
    }

    void Bake ()
    {
        // Generate smooth normals for each mesh
        var bakedMeshes = new HashSet<Mesh> ();

        foreach (var meshFilter in GetComponentsInChildren<MeshFilter> ())
        {

            // Skip duplicates
            if (!bakedMeshes.Add (meshFilter.sharedMesh))
            {
                continue;
            }

            // Serialize smooth normals
            var smoothNormals = SmoothNormals (meshFilter.sharedMesh);

            _BakeKeys.Add (meshFilter.sharedMesh);
            _BakeValues.Add (new ListVector3 () { _Data = smoothNormals });
        }
    }

    void LoadSmoothNormals ()
    {
        // Retrieve or generate smooth normals
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter> ())
        {

            // Skip if smooth normals have already been adopted
            if (!_RegisteredMeshes.Add (meshFilter.sharedMesh))
            {
                continue;
            }

            // Retrieve or generate smooth normals
            var index = _BakeKeys.IndexOf (meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? _BakeValues[index]._Data : SmoothNormals (meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs (3, smoothNormals);
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer> ())
        {
            if (_RegisteredMeshes.Add (skinnedMeshRenderer.sharedMesh))
            {
                skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
            }
        }
    }

    List<Vector3> SmoothNormals (Mesh mesh)
    {
        // Group vertices by location
        var groups = mesh.vertices.Select ((vertex, index) => new KeyValuePair<Vector3, int> (vertex, index)).GroupBy (pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3> (mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {

            // Skip single vertices
            if (group.Count () == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += mesh.normals[pair.Value];
            }

            smoothNormal.Normalize ();

            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    void UpdateMaterialProperties ()
    {
        // Apply properties according to mode
        _OutlineFillMaterial.SetColor ("_OutlineColor", _OutlineColor);

        switch (_OutlineMode)
        {
            case Mode.OutlineAll:
                _OutlineMaskMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                _OutlineFillMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                _OutlineFillMaterial.SetFloat ("_OutlineWidth", _OutlineWidth);
                break;

            case Mode.OutlineVisible:
                _OutlineMaskMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                _OutlineFillMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                _OutlineFillMaterial.SetFloat ("_OutlineWidth", _OutlineWidth);
                break;

            case Mode.OutlineHidden:
                _OutlineMaskMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                _OutlineFillMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                _OutlineFillMaterial.SetFloat ("_OutlineWidth", _OutlineWidth);
                break;

            case Mode.OutlineAndSilhouette:
                _OutlineMaskMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                _OutlineFillMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                _OutlineFillMaterial.SetFloat ("_OutlineWidth", _OutlineWidth);
                break;

            case Mode.SilhouetteOnly:
                _OutlineMaskMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                _OutlineFillMaterial.SetFloat ("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                _OutlineFillMaterial.SetFloat ("_OutlineWidth", 0);
                break;
        }
    }
}
