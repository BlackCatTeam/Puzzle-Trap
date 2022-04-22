using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Control {
	public interface IRaycastable 
	{
		CursorType GetCursorType();
		bool HandleRayCast(PlayerController callingController);
	}
	
}
