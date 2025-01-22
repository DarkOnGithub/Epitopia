using System;
using UnityEngine;

namespace Menus.Parallax
{
    public class ParallaxLayer : MonoBehaviour
    {
        private float _length, _startPos;
        public GameObject cam;
        public float parallaxEffect;
        public float offset;

        private void Start()
        {
            _startPos = transform.position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update()
        {
            var temp = cam.transform.position.x * (1 - parallaxEffect);
            var dist = cam.transform.position.x * parallaxEffect;

            transform.position = new Vector3(_startPos + dist, cam.transform.position.y - offset, transform.position.z);

            if (temp > _startPos + _length) _startPos += _length;
            else if (temp < _startPos - _length) _startPos -= _length;
        }
    }
}