using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;

using Rand = UnityEngine.Random;

struct VertBucket {
	public Vector3 position { get; set; }
	public List<int> verts { get; set; }

	public VertBucket(Vector3 position_, List<int> verts_)
	{
		position = position_;
		verts = verts_;
	}
}

public static class Outliner
{
	private static readonly Dictionary<int, Mesh> _cachedOutlines = new Dictionary<int, Mesh>();

	public static IEnumerable<GameObject> BuildOutline(GameObject objectToOutline, float size, Material outlineMaterial, bool recursive, float jitter = 0.0f, bool highQuality = false)
	{
		List<GameObject> results = new List<GameObject>();
		List<Transform> childrenToOutline;
		if (recursive)
		{
			childrenToOutline = new List<Transform>(objectToOutline.GetComponentsInChildren<Transform>());

			foreach (Transform child in childrenToOutline.ToArray())
			{
				if (child.GetComponent<Outline>())
				{
					childrenToOutline.Remove(child);
					GameObject.DestroyImmediate(child.gameObject);
				}
			}
		}
		else
		{
			childrenToOutline = new List<Transform>();
			childrenToOutline.Add(objectToOutline.transform);
		}

		bool hadEffect = false;
		int depth = 0;
		foreach (Transform child in childrenToOutline)
		{
			if (child.GetComponent<DoNotOutline>())
				continue;

			MeshFilter meshFilter = child.GetComponent<MeshFilter>();
			SkinnedMeshRenderer skinnedMesh = child.GetComponent<SkinnedMeshRenderer>();

			if (meshFilter || skinnedMesh)
			{
				GameObject outlineObj = MakeEmptyChildObject(child.name + "_outline", objectToOutline.transform);
				outlineObj.AddComponent<Outline>();
				results.Add(outlineObj);

				Renderer outlineRenderer = null;
				Mesh outlineMesh = null;
				bool usedCache = false;

				if (meshFilter)
				{
					if (!meshFilter.sharedMesh)
					{
						Debug.LogWarning("Empty MeshFilter. Error?", meshFilter);
						results.Remove(outlineObj);
						GameObject.Destroy(outlineObj);
						continue;
					}
					int hash = meshFilter.sharedMesh.GetHashCode() + size.GetHashCode();

					outlineRenderer = outlineObj.AddComponent<MeshRenderer>();
					MeshFilter outlineMeshFilter = outlineObj.AddComponent<MeshFilter>();

					Mesh cachedMesh;
					if (_cachedOutlines.TryGetValue(hash, out cachedMesh))
					{
						outlineMesh = cachedMesh;
						usedCache = true;
					}
					else
					{
						outlineMesh = CopyMesh(meshFilter.sharedMesh);
						_cachedOutlines[hash] = outlineMesh;
					}

					outlineMeshFilter.sharedMesh = outlineMesh;
				}
				else if (skinnedMesh)
				{
					SkinnedMeshRenderer outlineMeshRenderer = outlineObj.AddComponent<SkinnedMeshRenderer>();

					outlineMeshRenderer.bones = skinnedMesh.bones;
					outlineMeshRenderer.quality = skinnedMesh.quality;

					outlineMesh = CopyMesh(skinnedMesh.sharedMesh);

					outlineMeshRenderer.updateWhenOffscreen = true;

					outlineRenderer = outlineMeshRenderer;
					outlineMeshRenderer.sharedMesh = outlineMesh;
				}

				if (!usedCache)
				{
					FlipMesh(outlineMesh);
					PushMesh(outlineMesh, size, jitter, highQuality);
				}

				outlineRenderer.sharedMaterial = outlineMaterial;
				outlineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

				outlineObj.transform.parent = child;
				outlineObj.transform.position = child.position;
				outlineObj.transform.rotation = child.rotation;
				outlineObj.transform.localScale = Vector3.one;

				if (outlineMesh)
					outlineMesh.RecalculateBounds();

				if (child.GetComponent<Renderer>())
					outlineRenderer.enabled = child.GetComponent<Renderer>().enabled;

				if (depth > 65536)
					throw new Exception("Outliner recursed more than 65536, bailing out");

				hadEffect = true;
			}
		}

		if (!hadEffect)
			Debug.LogWarning("Unable to find any meshes in the selection");

		return results;
	}

	private static GameObject MakeEmptyChildObject(string name, Transform parent)
	{
		GameObject child = new GameObject(name);
		child.transform.parent = parent;
		child.transform.localPosition = parent.localPosition;
		child.transform.localRotation = parent.localRotation;
		child.transform.localScale = parent.localScale;
		return child;
	}

	private static GameObject PlaceMarker(Vector3 position)
	{
		GameObject markerObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		markerObj.GetComponent<Renderer>().sharedMaterial.color = Color.red;
		markerObj.transform.position = position;
		markerObj.transform.localScale = Vector3.one * 0.1f;
		return markerObj;
	}

	public static Mesh CopyMesh(Mesh mesh)
	{
		if (!mesh)
			return null;

		Mesh result = new Mesh();

		Vector3[] verts = new Vector3[mesh.vertices.Length];
		Array.Copy(mesh.vertices, verts, mesh.vertices.Length);
		result.vertices = verts;

		Vector3[] norms = new Vector3[mesh.normals.Length];
		Array.Copy(mesh.normals, norms, mesh.normals.Length);
		result.normals = norms;

		BoneWeight[] boneWeights = new BoneWeight[mesh.boneWeights.Length];
		Array.Copy(mesh.boneWeights, boneWeights, mesh.boneWeights.Length);
		result.boneWeights = boneWeights;

		Matrix4x4[] bindPoses = new Matrix4x4[mesh.bindposes.Length];
		Array.Copy(mesh.bindposes, bindPoses, mesh.bindposes.Length);
		result.bindposes = bindPoses;

		Vector2[] uv = new Vector2[mesh.uv.Length];
		Array.Copy(mesh.uv, uv, mesh.uv.Length);
		result.uv = uv;

		Vector2[] uv1 = new Vector2[mesh.uv2.Length];
		Array.Copy(mesh.uv2, uv1, mesh.uv2.Length);
		result.uv2 = uv1;

		Vector2[] uv2 = new Vector2[mesh.uv2.Length];
		Array.Copy(mesh.uv2, uv2, mesh.uv2.Length);
		result.uv2 = uv2;

		int[] tris = new int[mesh.triangles.Length];
		Array.Copy(mesh.triangles, tris, mesh.triangles.Length);
		result.triangles = tris;

		result.Optimize();
		result.RecalculateBounds();

		return result;
	}

	public static void FlipMesh(Mesh mesh)
	{
		if (!mesh)
			return;

		Vector3[] normals = new Vector3[mesh.normals.Length];
		Vector3[] meshNorms = mesh.normals;
		for (int i = 0; i < normals.Length; ++i)
			normals[i] = meshNorms[i] * -1.0f;

		mesh.normals = normals;
	}

	public static void PushMesh(Mesh mesh, float pushAmount, float jitter, bool avoidGaps)
	{
		if (!mesh)
			return;

		Vector3[] verts = new Vector3[mesh.vertices.Length];
		Vector3[] meshVerts = mesh.vertices;
		Vector3[] meshNorms = mesh.normals;

		if (meshNorms.Length == 0)
		{
			mesh.RecalculateNormals();
			meshNorms = mesh.normals;
		}

		float halfJitter = jitter * 0.5f;
		float actualPush = pushAmount * -1.0f;
		if (!avoidGaps)
		{
			for (int i = 0; i < verts.Length; ++i)
			{
				float min = actualPush - halfJitter;
				float max = actualPush + halfJitter;
				verts[i] = meshVerts[i] + (Mathf.Lerp(min, max, UnityEngine.Random.value) * meshNorms[i]);
			}
		}
		else
		{
			// Avoid gaps is currently O(n^2) so, if there's lots of verts, there's 
			// gonna be a problem...
			float epsilon = mesh.bounds.extents.magnitude * 0.00001f;
			float epsilonSqr = epsilon * epsilon;

			var buckets = new List<VertBucket>();
			for (int i = 0; i < meshVerts.Length; ++i)
			{
				Vector3 meshVert = meshVerts[i];
				bool foundBucket = false;
				foreach (VertBucket bucket in buckets)
				{
					if ((bucket.position - meshVert).sqrMagnitude < epsilonSqr)
					{
						foundBucket = true;
						bucket.verts.Add(i);
					}
				}

				if (!foundBucket)
				{
					buckets.Add(new VertBucket(meshVert, new List<int>() { i }));
				}
			}

			foreach (var bucket in buckets)
			{
				Vector3 avgOppNormal = Vector3.zero;
				foreach (int i in bucket.verts)
					avgOppNormal += meshNorms[i];
				avgOppNormal /= (float)bucket.verts.Count;

				foreach (int i in bucket.verts)
				{
					float min = actualPush - halfJitter;
					float max = actualPush + halfJitter;
					verts[i] = meshVerts[i] + (Mathf.Lerp(min, max, UnityEngine.Random.value) * avgOppNormal);
				}
			}
		}
		mesh.vertices = verts;
		mesh.triangles = ReverseWinding(mesh.triangles);
	}

	public static int[] ReverseWinding(int[] triangleIndices)
	{
		if (triangleIndices == null || triangleIndices.Length % 3 != 0)
			throw new ArgumentException("triangleIndicies must be a non null array with a length that's a multiple of three");

		int[] result = new int[triangleIndices.Length];
		for (int i = 0; i < triangleIndices.Length; i += 3)
		{
			result[i] = triangleIndices[i];
			result[i + 1] = triangleIndices[i + 2];
			result[i + 2] = triangleIndices[i + 1];
		}

		return result;
	}
}
