/*

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class BuildingMaker : InfrastructureBehaviour
{
	public Material building;
	IEnumerator Start()
	{
		// Wait until the map is ready
		while (!map.IsReady)
		{
			yield return null;
		}

		// Iterate through all the buildings in the 'ways' list
		foreach (var way in map.ways.FindAll((w) => { return w.IsBuilding && w.NodeIDs.Count > 1; }))
		{
			// Create the object
			CreateObject(way, building, "Building");
			yield return null;
		}
	}


	protected override void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
	{
		// Get the centre of the roof
		Vector3 oTop = new Vector3(0, way.Height, 0);

		vectors.Add(oTop);
		normals.Add(Vector3.up);
		uvs.Add(new Vector2(0.5f, 0.5f));

		for (int i = 1; i < way.NodeIDs.Count; i++)
		{
			OsmNode p1 = map.nodes[way.NodeIDs[i - 1]];
			OsmNode p2 = map.nodes[way.NodeIDs[i]];

			Vector3 v1 = p1 - origin;
			Vector3 v2 = p2 - origin;
			Vector3 v3 = v1 + new Vector3(0, way.Height, 0);
			Vector3 v4 = v2 + new Vector3(0, way.Height, 0);

			vectors.Add(v1);
			vectors.Add(v2);
			vectors.Add(v3);
			vectors.Add(v4);

			uvs.Add(new Vector2(0, 0));
			uvs.Add(new Vector2(1, 0));
			uvs.Add(new Vector2(0, 1));
			uvs.Add(new Vector2(1, 1));

			normals.Add(-Vector3.forward);
			normals.Add(-Vector3.forward);
			normals.Add(-Vector3.forward);
			normals.Add(-Vector3.forward);

			int idx1, idx2, idx3, idx4;
			idx4 = vectors.Count - 1;
			idx3 = vectors.Count - 2;
			idx2 = vectors.Count - 3;
			idx1 = vectors.Count - 4;

			// first triangle v1, v3, v2
			indices.Add(idx1);
			indices.Add(idx3);
			indices.Add(idx2);

			// second         v3, v4, v2
			indices.Add(idx3);
			indices.Add(idx4);
			indices.Add(idx2);

			// third          v2, v3, v1
			indices.Add(idx2);
			indices.Add(idx3);
			indices.Add(idx1);

			// fourth         v2, v4, v3
			indices.Add(idx2);
			indices.Add(idx4);
			indices.Add(idx3);

			// And now the roof triangles
			indices.Add(0);
			indices.Add(idx3);
			indices.Add(idx4);

			// Don't forget the upside down one!
			indices.Add(idx4);
			indices.Add(idx3);
			indices.Add(0);
		}
	}
}

