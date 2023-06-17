# Shooting Defence
<br>

## 프로젝트 소개

<div>
<img width="400" src="https://user-images.githubusercontent.com/71202869/246596017-e2de83f6-7218-4d3e-a12e-73b6f29dfdb4.PNG"/>
<img width="400" src="https://github.com/parkSihyeong46/shooting-defence/assets/71202869/6d5ccb61-1795-4f67-9262-f0e246b3c9a3"/>
<img width="400" src="https://user-images.githubusercontent.com/71202869/246596064-48deaa88-5f5f-485c-ae7c-c5b9473050da.PNG"/>
</div>

### 개요
```
- 탑뷰 슈팅 디펜스 장르
적들이 몰려오는 것을 막아내는 것이 이 게임의 목표

맵 에디터를 사용자들이 이용할 수 있게 만들어 본인이 원하는 스테이지를 제작할 수 있음
```

## 참여 인원 - (2인)
```
* parkSihyeong46

- 맵 에디터

- 스테이지

- 터렛
공격, 생성

```
```
* Jinhun-Noh

- 캐릭터
이동, 공격, 애니메이션

- 적
애니메이션

- 무기
라이플, 샷건

- 웨이브 끝난 후 추가 옵션

- 배경 사운드
```

<br>

## 기술 스택
```
* C#

* Visual Studio

* Unity
```

<br>

## 맵에디터 저장 데이터
<details>
  <summary>json</summary>
  <img width="500" src="https://user-images.githubusercontent.com/71202869/246597606-276262e5-9ad7-4a79-9944-decce02f6e44.PNG"/>
  <img width="500" src="https://user-images.githubusercontent.com/71202869/246597607-3daeede8-2458-45dc-b275-48bc33034880.PNG"/>
</details>
  

## 시연 영상
<details>
  <summary>-</summary>
  <img width="976" src=""/>
  </details>
<details>
  <summary>-</summary>
  <img width="976" src=""/>
</details>
<br>

## 기타 / 조작법
<details>
  <summary>조작법</summary>
  <table>
    <tr>
      <td><b>조작법</b></td>
      <td><b>W S A D</b></td>
    </tr>
    <tr>
      <td>공격</td>
      <td>클릭</td>
    </tr>
    <tr>
      <td>재장전</td>
      <td>R</td>
    </tr>
  </table>
</details>
<br/>

## 아쉬운점
```
* 처음부터 체계적인 구조를 잡아서 짜임새 있는 프로그램을 만들려 했으나
경험 부족으로 구조를 잘못 잡는 바람에 더 복잡한 코드로 되어버림

프리팹에 오브젝트 데이터를 넣어놓고 그걸 어드레서블로 가져왔어야 했지만,
오브젝트 데이터 가져오는 부분과 어드레서블로 프리팹을 로딩하는 부분을 따로 만들어 두는 바람에
동작 코드가 복잡해져 맵 에디터에서 오브젝트를 추가하려고 할 때
UI 추가하고, 오브젝트 데이터 따로 입력해서 저장하고, 어드레서블 생성해서 저장을 해야하는
복잡한 문제가 발생했음

다음번 프로젝트에서는 해당 문제가 생기지 않도록 신경을 쓸 것
```
