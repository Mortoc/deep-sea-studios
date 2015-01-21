using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour 
{
	private static Mesh s_sharedHexMesh = null;
	private static Vector2[] s_colliderPoints = null;

	public int i { get; set; }
	public int j { get; set; }

	public static Hex MakeHex()
	{
		GameObject hex = new GameObject("Hex");
		hex.AddComponent<MeshRenderer>();
		var meshFilter = hex.AddComponent<MeshFilter>();
		meshFilter.sharedMesh = MakeHexMesh();
		var collider = hex.AddComponent<PolygonCollider2D>();
		collider.points = s_colliderPoints;

		return hex.AddComponent<Hex>();
	}


	private static Mesh MakeHexMesh()
	{
		if( s_sharedHexMesh != null ) return s_sharedHexMesh;

		var mesh = new Mesh();
		mesh.name = "Generated Hex Mesh";

		var verts = new Vector3[7];
		var norms = new Vector3[7];
		var uvs = new Vector2[7];
		var colliderPoints = new Vector2[6];

		var oneSixth = 1.0f / 6.0f;
		var t = 0.0f;
		for(var i = 0; i < 6; ++i)
		{
			var tPi = t * Mathf.PI * 2.0f;
			var x = Mathf.Cos (tPi) * 0.5f;
			var y = Mathf.Sin (tPi) * 0.5f;
			verts[i].x = x;
			verts[i].y = y;
			norms[i].z = 1.0f;
			uvs[i].x = (1.0f + (x * 2.0f)) * 0.5f;
			uvs[i].y = (1.0f + (y * 2.0f)) * 0.5f;
			colliderPoints[i].x = x;
			colliderPoints[i].y = y;

			t += oneSixth;
		}
		norms[6].z = 1.0f;
		uvs[6] = Vector2.one * 0.5f;
		
		var tris = new int[]
		{
			6, 1, 0, 
			6, 2, 1, 
			6, 3, 2, 
			6, 4, 3, 
			6, 5, 4, 
			6, 0, 5, 
		};

		mesh.vertices = verts;
		mesh.normals = norms;
		mesh.uv = uvs;
		mesh.triangles = tris;
		mesh.Optimize();

		s_sharedHexMesh = mesh;
		s_colliderPoints = colliderPoints;
		return mesh;
	}
}
