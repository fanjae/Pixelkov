## PixelKov

> PixelKov는 **Escape from Tarkov**에서 영감을 받아 픽셀아트로 재해석한 2D PvE Extraction Shooting Game입니다.
> 플레이어는 다양한 몬스터를 처치하여 골드와 재료를 획득하고, 상점과 제작 시스템을 활용해 장비를 성장시키며 최종 보스를 처치하는 것을 목표로 합니다.


## 프로젝트 개요

| 항목 | 내용 |
|---|---|
| 프로젝트명 | PixelKov |
| 개발 기간 | 2026.06.17 ~ 2026.07.03 (17일) |
| 개발 인원 | 4명 |
| 개발 환경 | Unity 6.3 (6000.3.7f1), C# |
| 실행 환경 | Windows |
| IDE | Unity Editor, Visual Studio 2022 |

---

## 팀 구성 및 역할
| 이름 | 역할 | 담당 내용 |
|---|---|---|
| [@fanjae](https://github.com/fanjae) | 팀장 | 프로젝트 관리 및 장비 · 인벤토리 · 상점 시스템 설계 |
| [@ShinJinSeop2536](https://github.com/ShinJinSeop2536) | 적 AI | 몬스터 및 보스 구현 |
| [@duaehdtjs20](https://github.com/duaehdtjs20) | UI | 인벤토리, 장비, 상점 / 제작 UI 및 사운드 시스템 구현 |
| [@YeoHaeng-J](https://github.com/YeoHaeng-J) | 플레이어 | 플레이어 및 전투 시스템 |

---

## 실행 방법

### Unity 실행
1. https://github.com/fanjae/Pixelkov/releases/tag/Ver_1.4
2. `Pixelkov_ver.1.4.zip` 압축해제
3. `Pixelkov.exe` 실행


## 구현 기능

| 기능 | 화면 | 설명 |
|:----:|:---:|:---:|
| 메인 화면 | <img width="600" height="400" alt="Image" src="https://github.com/user-attachments/assets/a44cfea4-a119-44ad-ba27-13b4c68eae1d" /> | 게임 시작 및 종료 |
| 플레이어 | <img width="600" height="400" alt="Image" src="https://github.com/user-attachments/assets/a9de5f8b-1a1e-4fb3-b380-2b70f7a59ae5" /> | 이동, 조준, 공격, 회피 |
| 전투 시스템 | <img width="600" height="400" alt="Image" src="https://github.com/user-attachments/assets/b4eab723-a2f4-43b9-af66-b53a6f36ce9e" /> | 일반 몬스터 및 보스 전투 |
| 인벤토리 | <img width="600" height="400" alt="Image" src="https://github.com/user-attachments/assets/0477197c-0d07-4f6d-9ebd-f065186c920c" /> | 아이템 관리 및 장착 |
| 상점 | <img width="600" height="400" alt="Image" src="https://github.com/user-attachments/assets/c0af8456-dbbd-447c-9d7a-8c634837f0f5" /> | 아이템 구매 및 판매 |
| 장비 강화 | <img width="600" height="400" alt="Image" src="https://github.com/user-attachments/assets/7770c6ec-ddfa-4aa7-9113-9fa801f51c90" /> | 방어구 강화 |
| 제작 시스템 | <img width="600" height="400" alt="Image" src="https://github.com/user-attachments/assets/40557ec2-943f-40ab-befa-ee4b0330d4ce" /> | 재료를 이용한 무기 제작 |
| 보스 전투 | <img width="600" height="400" alt="Image" src="https://github.com/user-attachments/assets/cb79afe4-b089-48b9-9316-6883938c9c91" /> | 강한 능력치와 고유 패턴을 보유한 보스 몬스터 |

---

# 주요 기능

## 플레이어

- WASD 기반 8방향 이동
- 마우스 조준 및 사격
- 장전 시스템
- 회피(Dodge) 시스템

---

## 전투

- 일반 몬스터 AI
- 보스 몬스터 패턴
- 피격 및 사망 처리
- 플레이어 HP 시스템

---

## 인벤토리

- 슬롯 기반 인벤토리
- 스택 아이템 지원
- 장비 슬롯 분리
- 아이템 장착 및 해제

---

## 장비 시스템

- 무기 장착
- 방어구 장착
- 장비 교체
- 장비 정보 UI

---

## 상점 시스템

- 아이템 구매
- 아이템 판매
- 방어구 강화

---

## 제작 시스템

- 레시피 기반 제작
- 재료 소모
- 상위 무기 제작

---

## 조작 방법

| 키 | 기능 |
|---|---|
| WASD | 이동 |
| Mouse | 조준 |
| 좌클릭 | 공격 |
| R | 장전 |
| Space | 회피 |
| I | 인벤토리 |
| ESC | 메뉴 |

---

## 플레이 영상
(추가 예정)

## 개선 예정 사항

- 다양한 무기 추가
- 신규 보스 패턴
- 장비 옵션 시스템
- 저장 및 불러오기
- 사운드 및 이펙트 개선

## 개발 기록
[Pixelkov 개발일지](https://fanjae.tistory.com/326)
