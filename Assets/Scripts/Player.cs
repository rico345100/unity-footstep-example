using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private AudioSource audio;
	private float moveSpeed = 50f;
	private float jumpSpeed = 8f;
	private float distToGround;
	private Rigidbody rigidbody;

	public AudioClip[] footstepSounds;
	public bool isMoving;

	void Start() {
		audio = GetComponent<AudioSource>();
		rigidbody = GetComponent<Rigidbody>();
		distToGround = GetComponent<Collider>().bounds.extents.y;
	}

	bool IsGrounded() {
		return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
	}

	void FixedUpdate() {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

		transform.Translate(0, 0, z);
		transform.Rotate(0, x, 0);

		if(IsGrounded() && Input.GetKeyDown("space")) {
			rigidbody.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
		}
	}

	private Vector3 oldPos;
	private bool isFootstepPlaying = false;

	void Update() {
		Vector3 newPos = transform.position;

		if(newPos != oldPos) {
			isMoving = true;

			if(!isFootstepPlaying && IsGrounded()) {
				StartCoroutine("PlayFootstep");
			}
		}
		else {
			isMoving = false;
		}

		oldPos = newPos;
	}

	IEnumerator PlayFootstep() {
		isFootstepPlaying = true;

		int textureIdx = TerrainSurface.GetMainTexture(transform.position);
		
		audio.clip = footstepSounds[textureIdx];
		audio.Play();
	
		yield return new WaitForSeconds(.6f);

		isFootstepPlaying = false;
	}
}
