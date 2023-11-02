using System.Collections;
using Datas;
using Entities.Parts;
using Entities.Parts.HeavyWeapons;
using Managers;
using UnityEngine;

public class PlasmaThrower : HeavyWeaponPart
{
    public override PartIdentifier PartID => PartIdentifier.HEAVY_WEAPON_PART_PLASMA_THROWER;
    public override string Name => GetLocalizedString("Lance plasma", "Plasma thrower");
    public override string Description => GetLocalizedString("Brûle tout ce qui s'approche trop", 
        "Burn everything that's gets too close");

    [SerializeField] private float _burstTime;
    [SerializeField] SoundDefinition _soundDefinition;

    private AudioSource _audioSource;
    private ParticleSystem _particleSystem;
    
    public override float Damage => base.Damage * Random.Range(0, 2);

    private float BurstTime => _burstTime * _currentMultiplier;
    
    public override string[] Info => new[]
    {
        base.Info[0] +
        GetLocalizedString("Temps de souffle", "Burst time") + "\n",
        base.Info[1] +
        _burstTime.ToString("F1") + "\n"
    };
    
    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = _shootPoint.GetComponent<ParticleSystem>();

        if(_particleSystem != null)
        {
            ParticleSystem.MainModule main = _particleSystem.main;
            ParticleSystem.ShapeModule shape = _particleSystem.shape;

            main.startSpeed = _speed;
            main.startLifetime = _range / _speed;
            main.duration = BurstTime;
            shape.angle = _spreadAngle * _currentMultiplier;
        }
    }

    protected override IEnumerator Shooting()
    {
        if (!DelayPassed())
            yield break;
        _soundDefinition.PlayAsSFX(_audioSource, 0);
        Shoot();
        _timeOfLastShot = Time.time;
        
        if(Delay > 0.2f && _entity.IsPlayer) 
            PublishActivation(PartsEvents.ACTIVATED);
        
        yield return new WaitForSeconds(BurstTime);
        _particleSystem.Stop();
        _audioSource.Stop();
        _shooting = null;
    }

    protected override void Shoot()
    {
        _particleSystem.Play();
    }

    protected override void StopActiveAbility()
    {
    }
}
