using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f; // 앞뒤 움직임의 속도
    public float RotateSpeed = 180f; // 좌우 회전 속도

    private PlayerInput _input; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody _rigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator _animator; // 플레이어 캐릭터의 애니메이터

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        // 사용할 컴포넌트들의 참조를 가져오기
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        move();
        rotate();

        _animator.SetFloat(PlayerAnimID.Move, _input.MoveDirection);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void move()
    {
        //// 거리 = 속력 * 시간
        //float movementAmount = MoveSpeed * Time.fixedDeltaTime;
        //// 방향은 캐릭터 기준이다
        //Vector3 directoin = _input.MoveDirection * transform.forward;
        //Vector3 offset = movementAmount * directoin;

        // 유니티의 최적화 중 연산순서가 중요하다. 왼쪽부터 오른쪽으로 되는데
        // 아래 식에서는 float 와 Vector3 의 연산때문에 그렇다. 백터연산의 횟수 때문이다.
        Vector3 deltaPosition = transform.forward * MoveSpeed * _input.MoveDirection * Time.fixedDeltaTime;

        _rigidbody.MovePosition(_rigidbody.position + deltaPosition);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void rotate()
    {
        float rotationAmount = _input.RotateDirection * RotateSpeed * Time.fixedDeltaTime;

        Quaternion deltaRotation = Quaternion.Euler(0f, rotationAmount, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }
}