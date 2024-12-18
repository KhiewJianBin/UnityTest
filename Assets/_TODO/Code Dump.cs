//// Declared outside of the methods
//private string DebugId
//{
//    get { return Time.frameCount + ":" + this.ToString() + ":" + _playerName + ": "; }
//}
 
//    // this is the first line in each of the methods: Update, FixedUpdate and OnGUI
//    // In each method, I replaced the string "Awake!" with the respective method name, e.g., "Update"
//    if (debugMsgEvent)  Debug.Log (DebugId + "Awake!");

//
//
//
// // fomular to transform square points to circle 2d
// Vector2 circle;
// circle.x = square.x * Mathf.Sqrt(1f - square.y * square.y * 0.5f);
// circle.y = square.y * Mathf.Sqrt(1f - square.x * square.x * 0.5f);
//
//
    // Vector2 s;
    // s.x = v.x * Mathf.Sqrt(1f - y2 / 2f - z2 / 2f + y2 * z2 / 3f);
    // s.y = v.y * Mathf.Sqrt(1f - x2 / 2f - z2 / 2f + x2 * z2 / 3f);
    // s.z = v.z * Mathf.Sqrt(1f - x2 / 2f - y2 / 2f + x2 * y2 / 3f);
//

		// inverse-square law. 
		// Just divide the original force by the distance squared,
		// +1f. This guarantees that the force is at full strength 
		//when the distance is zero. 
		//float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);