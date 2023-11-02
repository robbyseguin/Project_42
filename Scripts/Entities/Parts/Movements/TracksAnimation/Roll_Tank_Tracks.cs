using UnityEngine;

namespace Entities.Parts.Movements
{
	public class Roll_Tank_Tracks : MonoBehaviour
	{
		private Material rendMaterial;

		private void Awake()
		{
			rendMaterial = GetComponent<Renderer>().material;
		}
	
		public void RollTracks(float speed)
		{
			Vector2 Offset = rendMaterial.mainTextureOffset;
			Offset.y += (Time.deltaTime * speed) % 1;
			rendMaterial.mainTextureOffset = Offset;
		}
	}
}
