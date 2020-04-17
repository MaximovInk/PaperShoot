using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteAlways]
public class SpriteTarget : MonoBehaviour
{
	public List<Transform> Sprites = new List<Transform>();

	public List<Transform> Bones = new List<Transform>();

	private void LateUpdate()
	{
		if (Bones.Count-1 != Sprites.Count)
			return;

		for (int i = 0; i < Bones.Count; i++)
		{
			if (Bones[i] == null) return;
		}
		for (int i = 0; i < Sprites.Count; i++)
		{
			if (Sprites[i] == null) return;
		}

		for (int i = 0; i < Bones.Count-1; i++)
		{
			if (Bones[i] == null || Sprites[i] == null || Bones[i + 1] == null)
				continue;

			Sprites[i].position = (Bones[i].position + Bones[i+1].position) / 2f;
			Sprites[i].localPosition = new Vector3(Sprites[i].localPosition.x,Sprites[i].localPosition.y, transform.position.z);
			Sprites[i].rotation = Quaternion.Euler(0,0,Mathf.Atan2(Bones[i].position.y - Bones[i + 1].position.y, Bones[i].position.x - Bones[i + 1].position.x) * Mathf.Rad2Deg+90);
		}
	}

}

