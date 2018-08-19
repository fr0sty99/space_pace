﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;
    [SerializeField]
    private LayerMask mask;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        // transform.right means forward in our case. the red axis is the axis which our player is facing

        Vector2 _startPos = transform.position;
        Vector2 _destPos = transform.right * weapon.range;

        // Raycast from _startPos to _destPos with the length of weapon.range, we only hit objects in the Layermask "mask"
        RaycastHit2D _hit = Physics2D.Raycast(_startPos, _destPos, weapon.range, mask);
        Debug.DrawRay(_startPos, _destPos, Color.red);

        // if we hit a player
        if (_hit.collider.tag == PLAYER_TAG) 
        {
            // tell server that we hit that player with its netID in its name
            CmdPlayerShot(_hit.collider.name, transform.name, weapon.damage);
        }
    }

    [Command]
    void CmdPlayerShot(string _deadPlayerID, string _playerId, int damage)
    {
        Debug.Log(_deadPlayerID + " has been shot from " + _playerId + " with a damage of " + damage);

        Player _player = GameManager.GetPlayer(_playerId);
        _player.TakeDamage(damage);
    }

}
