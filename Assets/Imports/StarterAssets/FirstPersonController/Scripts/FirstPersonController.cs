﻿using Personnage;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;


		[Space(10)]
		[Header("Attack & Interact")]
		[Tooltip("Portee d'interaction")]
		public float interactRange = 4f;


		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private PersonnageController _personnage;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		//gestion des interactions
		private GameObject highlightedObject;
		private Material originalMaterial;
		private bool hasAnimationTriggered = false;
		private bool hasAnimationTriggered2 = false;

		//private Animator animatorPC2;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			//animatorPC2 = GetComponent<prison_cell2>();
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}

			// Attach EventTrigger to the interactable objects
			GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");
			foreach (GameObject interactableObject in interactableObjects)
			{
				EventTrigger eventTrigger = interactableObject.AddComponent<EventTrigger>();

				// Create PointerExit entry for removing the highlight
				EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
				pointerExitEntry.eventID = EventTriggerType.PointerExit;
				pointerExitEntry.callback.AddListener((eventData) => RemoveHighlight(interactableObject));
				eventTrigger.triggers.Add(pointerExitEntry);
			}
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
			Attack();
			Interact();
			UpdateHighlightedObject();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}





		#region inputMethods


		private void Interact()
		{
			if (_input.interact && highlightedObject != null)
			{
				if (highlightedObject.CompareTag("Interactable"))
				{
					Debug.Log("Interacting with " + highlightedObject.name);

					if (highlightedObject.name == "interrupteur1" && !hasAnimationTriggered)
					{
						Debug.Log("Sesame ouvre toi");
						Animator prisonCellAnimator = GameObject.Find("prison_cell").GetComponent<Animator>();
						prisonCellAnimator.SetTrigger("open");
						hasAnimationTriggered = true;
					}

					else if (highlightedObject.name == "interrupteur2" && !hasAnimationTriggered2)
					{
						Debug.Log("Sesame ouvre toi");
						Animator prisonCellAnimator2 = GameObject.Find("prison_cell2").GetComponent<Animator>();
						prisonCellAnimator2.SetTrigger("open");
						hasAnimationTriggered2 = true;
					}

					else if (highlightedObject.name == "finaldoor")
					{
						//reussite du niveau ici
						Debug.Log("Niveau 1 réussi");
					}
				}
			}
		}


		private void UpdateHighlightedObject()
		{
			Vector2 mousePosition = Mouse.current.position.ReadValue();
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, interactRange))
			{
				GameObject hitObject = hit.transform.gameObject;

				if (hitObject.CompareTag("Interactable"))
				{
					if (hitObject != highlightedObject)
					{
						// Remove highlight from the previously highlighted object
						if (highlightedObject != null)
						{
							RemoveHighlight(highlightedObject);
						}

						// Add highlight to the new highlighted object
						AddHighlight(hitObject);
						highlightedObject = hitObject;
					}
				}
				else
				{
					// Remove highlight from the previously highlighted object
					if (highlightedObject != null)
					{
						RemoveHighlight(highlightedObject);
						highlightedObject = null;
					}
				}
			}
			else
			{
				// Remove highlight from the previously highlighted object
				if (highlightedObject != null)
				{
					RemoveHighlight(highlightedObject);
					highlightedObject = null;
				}
			}
		}

		private void AddHighlight(GameObject interactableObject)
		{
			Renderer renderer = interactableObject.GetComponent<Renderer>();
			if (renderer != null)
			{
				// Store the original material of the object
				originalMaterial = renderer.material;

				// Create a new material instance for the highlight effect
				Material highlightMaterial = new Material(originalMaterial);
				highlightMaterial.color = Color.green;

				// Assign the highlight material to the object's renderer
				renderer.material = highlightMaterial;
			}
		}

		private void RemoveHighlight(GameObject interactableObject)
		{
			Renderer renderer = interactableObject.GetComponent<Renderer>();
			if (renderer != null)
			{
				// Restore the original material of the object
				renderer.material = originalMaterial;
			}
		}




		private void Attack()
		{
            if (_input.attack)
            {
				GetComponent<PersonnageController>().attaque();
            }
		}
		#endregion

		#region inputMethodsUnityNative
		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
			targetSpeed = targetSpeed * GetComponent<PersonnageController>().vitesseMouvement;
			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
    #endregion
}