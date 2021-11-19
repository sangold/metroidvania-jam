using UnityEngine;

namespace MV.Player
{
    public class PlayerInputs
    {
        private bool _jumpButton;
        private bool _jumpButtonSwitch;
        private bool _slideButton;
        private bool _slideButtonSwitch;
        private bool _attackButton;
        private bool _attackButtonSwitch;
        private bool _ghostDashButton;
        private bool _ghostDashButtonSwitch;


        public bool JumpButton { get => _jumpButton; }
        public bool JumpButtonPressed { get =>_jumpButton && _jumpButtonSwitch; }
        public bool SlideButtonPressed { get => _slideButton && _slideButtonSwitch; }
        public bool AttackButtonPressed { get => _attackButton && _attackButtonSwitch; }
        public bool GhostDashButtonPressed { get => _ghostDashButton && _ghostDashButtonSwitch; }
        public float MovementX { get; private set; }
        public float MovementY { get; private set; }

        public void GetInputs()
        {
            MovementX = Input.GetAxis("Horizontal");
            MovementY = Input.GetAxis("Vertical");

            if (_jumpButton != Input.GetButton("Jump"))
            {
                _jumpButtonSwitch = true;
            }
            else
            {
                _jumpButtonSwitch = false;
            }
            _jumpButton = Input.GetButton("Jump");

            if (_slideButton != Input.GetButton("Slide"))
            {
                _slideButtonSwitch = true;
            }
            else
            {
                _slideButtonSwitch = false;
            }
            _slideButton = Input.GetButton("Slide");

            if (_attackButton != Input.GetButton("Attack"))
            {
                _attackButtonSwitch = true;
            }
            else
            {
                _attackButtonSwitch = false;
            }
            _attackButton = Input.GetButton("Attack");

            if (_ghostDashButton != Input.GetButton("GhostDash"))
            {
                _ghostDashButtonSwitch = true;
            }
            else
            {
                _ghostDashButtonSwitch = false;
            }
            _ghostDashButton = Input.GetButton("GhostDash");

        }
    }

}
