using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInput playerinput;
    private PlayerInput.OnFootActions onFoot;

    private Vector2 movementInput;
    private PlayerMotor motor;
    private PlayerLook look;
    // Start is called before the first frame update
    void Awake()
    {
        playerinput = new PlayerInput();
        onFoot = playerinput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tell the playermotor to move using the vaule from our movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());

    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}