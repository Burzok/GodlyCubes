#pragma strict

private var p : Vector3;
private var r : Quaternion;
private var m : int = 0;
 
function Start() {
	networkView.observed = this;
}
 
function OnSerializeNetworkView(stream : BitStream) {
	p = rigidbody.position;
	r = rigidbody.rotation;
	m = 0;
	stream.Serialize(p);
	stream.Serialize(r);
	stream.Serialize(m);
}