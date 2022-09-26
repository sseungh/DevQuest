using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    private static float HEIGHT = 2f;
    //간단한 fsm state방식으로 동작하는 Player Controller입니다. Fsm state machine에 대한 더 자세한 내용은 세션 3회차에서 배울 것입니다!
    //지금은 state가 3개뿐이지만 3회차 세션에서 직접 state를 더 추가하는 과제가 나갈 예정입니다.
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float jumpAmount = 6f;

    public enum State {
        none,
        idle,
        jump
    }

    [Header("Debug")]
    public State state = State.none;
    public State nextState = State.none;
    private float stateTime;

    public bool landed = false;
    private Rigidbody rigid;
    private Collider col;
    private Transform camt;

    private void Start() {
        camt = FindObjectOfType<Camera>().transform;
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        state = State.none;
        nextState = State.idle;
        stateTime = 0f;
    }

    private void Update() {
        //0. 글로벌 상황 판단
        stateTime += Time.deltaTime;
        CheckLanded();
        //insert code here...

        //1. 스테이트 전환 상황 판단
        if (nextState == State.none) {
            switch (state) {
                case State.idle:
                    if (landed) {
                        if (Input.GetKey(KeyCode.Space)) {
                            nextState = State.jump;
                        }
                    }
                    break;
                case State.jump:
                    if (landed) nextState = State.idle;
                    break;
                //insert code here...
            }
        }


        //2. 스테이트 초기화
        if (nextState != State.none) {
            state = nextState;
            nextState = State.none;
            switch (state) {
                case State.jump:
                    Vector3 vel = rigid.velocity;
                    vel.y = jumpAmount;
                    rigid.velocity = vel;
                    break;
                //insert code here...
            }
            stateTime = 0f;
        }

        //3. 글로벌 & 스테이트 업데이트
        UpdateInput();
        //insert code here...
    }

    //땅에 닿았는지 여부를 확인하고 landed를 설정해주는 함수
    private void CheckLanded() {
        //발 위치에 작은 구를 하나 생성에 그 구에 땅이 닿는지 검사한다.
        //1 << 6은 Ground의 레이어가 6이기 때문.
        landed = Physics.CheckSphere(new Vector3(col.bounds.center.x, col.bounds.center.y - ((HEIGHT - 1f) / 2 + 0.15f), col.bounds.center.z), 0.45f, 1 << 6, QueryTriggerInteraction.Ignore);
    }

    //WASD 인풋을 처리하는 함수
    private void UpdateInput() {
        Vector3 vel = rigid.velocity;
        if (Input.GetKey(KeyCode.W)) {
            vel += ForwardVector() * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            vel += ForwardVector() * moveSpeed * -1 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            vel += RightVector() * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            vel += RightVector() * moveSpeed * -1 * Time.deltaTime;
        }
        rigid.velocity = vel;
    }

    //카메라 기준으로 앞과 우측 벡터를 계산해주는 함수
    private Vector3 ForwardVector() {
        Vector3 v = camt.forward;
        v.y = 0;
        v.Normalize();
        return v;
    }

    private Vector3 RightVector() {
        Vector3 v = camt.right;
        v.y = 0;
        v.Normalize();
        return v;
    }
}
