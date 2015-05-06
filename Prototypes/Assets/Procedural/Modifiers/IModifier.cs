using UnityEngine;
using System.Collections;

namespace DSS.Procedural
{
	public interface IModifier
	{
		// inline modifies the existing mesh object
		void Modify(Mesh mesh);
	}
}