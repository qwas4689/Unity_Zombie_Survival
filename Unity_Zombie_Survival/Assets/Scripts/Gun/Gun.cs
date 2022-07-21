using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour
{
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }

    public State CurrentState { get; private set; } // 현재 총의 상태

    public Transform FireTransform; // 탄알이 발사될 위치

    public ParticleSystem MuzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem ShellEjectEffect; // 탄피 배출 효과

    private LineRenderer _bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러

    public GunData Data;

    private AudioSource _audioSource; // 총 소리 재생기

    private float _fireDistance = 50f; // 사정거리


    private int _remainAmmo; // 남은 전체 탄알
    private int _ammoInMagazine; // 현재 탄알집에 남아 있는 탄알

    private float _lastFireTime; // 총을 마지막으로 발사한 시점

    private void Awake()
    {
        // 사용할 컴포넌트의 참조 가져오기
        _bulletLineRenderer = GetComponent<LineRenderer>();
        _bulletLineRenderer.positionCount = 2;
        _bulletLineRenderer.enabled = false;

        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // 총 상태 초기화
        _remainAmmo = Data.InitialAmmoCount;
        _ammoInMagazine = Data.MagazineCapacity;
        CurrentState = State.Ready;
        _lastFireTime = 0f;
    }

    // 발사 시도
    public void Fire()
    {
        // 발사가 가능할 때?
        // 1. 상태가 레디일 때 state == State.Ready일 때
        // 2. 쿨타임이 다 찼을 때
        if (CurrentState != State.Ready || Time.time < _lastFireTime + Data.FireCooltime)
        {
            return;
        }
        else
        {
            _lastFireTime = Time.time;

            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot()
    {
        Vector3 hitPosition;

        RaycastHit hit;
        if (Physics.Raycast(FireTransform.position, transform.forward, out hit, _fireDistance))
        {
            // 맞은 상대의 IDamageable을 가져옴
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                target.OnDamage(Data.Damage, hit.point, hit.normal);
            }
            // target?.OnDamage(Data.Damage, hit.point, hit.normal);

            hitPosition = hit.point;
        }
        else
        {
            hitPosition = FireTransform.position + transform.forward * _fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));

        --_ammoInMagazine;
        if (_ammoInMagazine <= 0)
        {
            CurrentState = State.Empty;
        }
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 이펙트 실행
        MuzzleFlashEffect.Play();
        ShellEjectEffect.Play();

        // 총알궤적의 시작점과 끝점 설정
        _bulletLineRenderer.SetPosition(0, FireTransform.position);
        _bulletLineRenderer.SetPosition(1, hitPosition);

        _audioSource.PlayOneShot(Data.ShotClip);

        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        _bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
        _bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool TryReload()
    {
        // 1. 이미 재장전 중이거나
        // 2. 탄알집이 꽉 차있다면
        // 3. 장전할 총알이 없거나
        if (CurrentState == State.Reloading || _remainAmmo == Data.MagazineCapacity || _remainAmmo <= 0)
        {
            return false;

        }
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환
        CurrentState = State.Reloading;

        // 재장전 소리 재생
        _audioSource.PlayOneShot(Data.ReloadClip);

        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(Data.ReladTime);

        // 총알을 잘 채워야 함.
        int ammoToFill = Mathf.Min(Data.MagazineCapacity - _ammoInMagazine, _remainAmmo);
        _ammoInMagazine += ammoToFill;
        _remainAmmo -= ammoToFill;

        // 2. 총알이 부족하다면

        // 총의 현재 상태를 발사 준비된 상태로 변경
        CurrentState = State.Ready;
    }
}