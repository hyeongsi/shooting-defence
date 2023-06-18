# Shooting Defence
<br>

## 프로젝트 소개

<div>
<img width="400" src="https://user-images.githubusercontent.com/71202869/246596017-e2de83f6-7218-4d3e-a12e-73b6f29dfdb4.PNG"/>
<img width="400" src="https://github.com/parkSihyeong46/shooting-defence/assets/71202869/6d5ccb61-1795-4f67-9262-f0e246b3c9a3"/>
<img width="400" src="https://user-images.githubusercontent.com/71202869/246596064-48deaa88-5f5f-485c-ae7c-c5b9473050da.PNG"/>
</div>

## 개요
+ 탑뷰 슈팅 디펜스 장르의 게임을 만들고자 개발을 시작하게 됨
 
+ 맵 에디터를 활용해 자신이 원하는 맵을 만들어 플레이 할 수 있도록 구현

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
* C#

* Visual Studio

* Unity

<br>

## 맵에디터 저장 데이터
<details>
  <summary><h4>.json 양식</h4></summary>
  <img width="500" src="https://user-images.githubusercontent.com/71202869/246597606-276262e5-9ad7-4a79-9944-decce02f6e44.PNG"/>
  <img width="500" src="https://user-images.githubusercontent.com/71202869/246597607-3daeede8-2458-45dc-b275-48bc33034880.PNG"/>
</details>
  

## 시연 영상
<details>
  <summary><h5>시작</h5></summary>
  <video src="https://user-images.githubusercontent.com/71202869/246639787-1008e188-e993-40cf-9493-3ba5d349675f.mp4"></video>
</details>
<details>
  <summary><h5>맵 에디터</h5></summary>
  <video src="https://user-images.githubusercontent.com/71202869/246639843-330d5dc9-d9bf-4b60-8d87-693737215762.mp4"></video>
</details>
<details>
  <summary><h5>커스텀 맵</h5></summary>
  <video src="https://user-images.githubusercontent.com/71202869/246639911-05badc89-aa11-4355-85d5-99c68f6c6de9.mp4"></video>
</details>
<br>

## 아쉬운점
처음부터 체계적인 구조를 잡아서 짜임새 있는 프로그램을 만들려 했으나<br/>
경험 부족으로 구조를 잘못 잡는 바람에 더 복잡한 코드로 되어버렸음<br/>
<br/>
위와 같은 구조가 된 배경에는 프리팹에 오브젝트 데이터를 넣어놓고 그걸 어드레서블로 가져왔어야 했지만,<br/>
오브젝트 데이터 가져오는 부분과 어드레서블로 프리팹을 로딩하는 부분을 따로 만들어 두는 바람에<br/>
동작 코드가 복잡해져 맵 에디터에서 오브젝트를 추가하려고 할 때<br/>
UI 추가하고, 오브젝트 데이터 따로 입력해서 저장하고, 어드레서블 생성해서 저장을 해야하는<br/>
복잡한 문제가 발생했음<br/>

다음번 프로젝트에서는 해당 문제가 생기지 않도록 신경을 쓸 것

<br/>

## 기타 / 조작법
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

<h4>맵 에디터 참고사항</h4>

* 노란색 발판 오브젝트: 캐릭터 스폰 위치

* 빨간색 발판 오브젝트: 적 스폰 위치

* 파란색 발판 오브젝트: 적 이동 경로

- 적은 파란색 발판 오브젝트를 따라가며 마지막 파란색 발판 오브젝트에 도달하면 소멸처리
  
<br/>
