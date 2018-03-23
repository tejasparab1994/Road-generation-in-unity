using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapReader))]
abstract class InfrastructureBehaviour : MonoBehaviour
{

	protected MapReader map;


	void Awake()
	{
		map = GetComponent<MapReader>();
	}


	protected Vector3 GetCentre(OsmWay way)
	{
		Vector3 total = Vector3.zero;

		foreach (var id in way.NodeIDs)
		{
			total += map.nodes[id];
		}

		return total / way.NodeIDs.Count;
	}


	protected void CreateObject(OsmWay way, Material mat, string objectName)
	{
		
		objectName = string.IsNullOrEmpty(objectName) ? "OsmWay" : objectName;


		GameObject go = new GameObject(objectName);
		Vector3 localOrigin = GetCentre(way);
		go.transform.position = localOrigin - map.bounds.Centre;


		MeshFilter mf = go.AddComponent<MeshFilter>();
		MeshRenderer mr = go.AddComponent<MeshRenderer>();


		mr.material = mat;


		List<Vector3> vectors = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> indices = new List<int>();


		OnObjectCreated(way, localOrigin, vectors, normals, uvs, indices);


		mf.mesh.vertices = vectors.ToArray();
		mf.mesh.normals = normals.ToArray();
		mf.mesh.triangles = indices.ToArray();
		mf.mesh.uv = uvs.ToArray();
	}

	protected abstract void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices);
}
