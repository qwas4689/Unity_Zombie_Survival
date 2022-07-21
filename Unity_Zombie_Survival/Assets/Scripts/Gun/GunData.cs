using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "Gun Data")]
public class GunData : ScriptableObject
{
    public AudioClip ShotClip; // 발사 소리
    public AudioClip ReloadClip; // 재장전 소리

    public float Damage = 25; // 공격력

    public int InitialAmmoCount = 100; // 처음에 주어질 전체 탄약
    public int MagazineCapacity = 25; // 탄창 용량

    public float FireCooltime = 0.12f; // 총알 발사 간격
    public float ReladTime = 1.8f; // 재장전 소요 시간
}