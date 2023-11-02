using UnityEngine;

namespace Entities
{
    public interface IWarpable
    {
        public void WarpTo(Vector3 newPosition, Quaternion newRotation);
        public void WarpTo(Transform target) => WarpTo(target.position,target.rotation);
    }
}