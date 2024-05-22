
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CharacterController))]

/// <summary>
/// Gère le mouvement du joueur en fonction des entrées clavier et de la rotation de la caméra.
/// Auteur(s): Maxime, Noé
/// </summary>
public class MovementInput : MonoBehaviour {

    public float Velocity;
	public float DeathDistance;
	public Vector3 StartPos;
    [Space]

	public float InputX;
	public float InputZ;
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    private Vector3 moveVector;


	void Start () {
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
	}

	void Update () {
        if (transform.position.y < DeathDistance)
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        InputMagnitude ();
    }

    /// <summary>
    /// Déplace le joueur et gère sa rotation en fonction des entrées clavier.
    /// </summary>
    void PlayerMoveAndRotation() {
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;

		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            transform.Translate(desiredMoveDirection * Time.deltaTime * Velocity, Space.World);
        }
	}

    /// <summary>
    /// Fait tourner le joueur pour regarder une position spécifique.
    /// </summary>
    /// <param name="pos">La position à regarder.</param>
    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    /// <summary>
    /// Fait tourner le joueur pour regarder dans la direction de la caméra.
    /// </summary>
    /// <param name="t">La transformée de la caméra.</param>
    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

    /// <summary>
    /// Gère l'ampleur de l'entrée utilisateur et le mouvement du joueur.
    /// </summary>
    void InputMagnitude() {
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		Speed = new Vector2(InputX, InputZ).sqrMagnitude;


		if (Speed > allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (Speed < allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StopAnimTime, Time.deltaTime);
		}
	}
}
