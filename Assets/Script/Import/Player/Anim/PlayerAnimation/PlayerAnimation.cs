using Fusion;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    [Networked] private float _movementBlendValue { get; set; }
    [Networked] private bool _playerDefence { get; set; }
    [Networked] private int _playerAttack { get; set; }



    private Animator _animator;
    private int _playerMovementAnimationID;
    private int _playerAttackAnimationID;
    private int _playerGetDamageAnimationID;
    private int _playerDieAnimationID;
    private int _playerRespawnAnimationID;
    private int _playerDefenceAnimationID;
    private ChangeDetector _changeDetector;

    private void SetupAnimationIDs()
    {
        _playerMovementAnimationID = Animator.StringToHash("Movement");
        _playerAttackAnimationID = Animator.StringToHash("Attack");
        _playerGetDamageAnimationID = Animator.StringToHash("Hit");
        _playerDieAnimationID = Animator.StringToHash("OnDie");
        _playerRespawnAnimationID = Animator.StringToHash("Respawn");
        _playerDefenceAnimationID = Animator.StringToHash("Def");
    }
    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        _animator.SetBool(_playerAttackAnimationID, false);
        foreach (var change in _changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(_movementBlendValue):
                    _animator.SetFloat(_playerMovementAnimationID, _movementBlendValue);
                    break;

                case nameof(_playerDefence):
                    _animator.SetBool(_playerDefenceAnimationID, _playerDefence);
                    break;
                case nameof(_playerAttack):

                    var reader = GetPropertyReader<int>(nameof(_playerAttack));
                    var values = reader.Read(previousBuffer, currentBuffer);

                    if (values.Item2 > values.Item1)
                    {
                        _animator.SetBool(_playerAttackAnimationID, true);
                    }
                    break;
            }
        }
    }

    public void SetupPlayerAnimation()
    {
        _animator = GetComponentInChildren<Animator>();
        SetupAnimationIDs();
    }

    public void UpdateMovementAnimation(float movementBlendValue)
    {
        _movementBlendValue = movementBlendValue;
        //_animator.SetFloat(_playerMovementAnimationID, movementBlendValue);
    }

    public void PlayAttackAnimation()
    {
        _playerAttack++;
        //_animator.SetTrigger(_playerAttackAnimationID);
    }

    public void PlayDieAnimation()
    {
        _animator.SetTrigger(_playerDieAnimationID);
    }

    public void PlayGetDamageAnimation()
    {
        _animator.SetTrigger(_playerGetDamageAnimationID);
    }

    public void PlayRespawnAnimation()
    {
        _animator.SetTrigger(_playerRespawnAnimationID);
    }

    public void PlayDefenceAnimation(bool result)
    {
        _playerDefence = result;
        //_animator.SetBool(_playerDefenceAnimationID, result);
    }




}
