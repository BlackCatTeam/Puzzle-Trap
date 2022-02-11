using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cripts.Core {
	public class RotateCamera : MonoBehaviour
	{
		public enum Directions { Left , Right, None}
		void Start()
		{
			
		}
	
		public void RotatingCamera(Directions direction)
        {
            switch (direction)
            {
				case Directions.Left:
					this.transform.Rotate(0f, 1.5f, 0f, Space.World);
					break;
				case Directions.Right:
					this.transform.Rotate(0f, -1.5f, 0f, Space.World);
					break;
				default: break;
            }
        }

		public Directions GetKey()
        {
			if (Input.GetKey(KeyCode.E))
            {
				return Directions.Right;
            }
			else if (Input.GetKey(KeyCode.Q))
            {
				return Directions.Left;
            }
			return Directions.None;
        }
		// Update is called once per frame
		void Update()
		{
			this.RotatingCamera(GetKey());			
		}
	}
}
