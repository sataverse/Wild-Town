using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class cshTouchManager : MonoBehaviour
{
    public GameObject uiManager;
    public GameObject text;

    private Vector2 prePos;
    private float preDistance = 0f;
    private float sizeOrtho = 4f;

    private Vector2 clickPointA;
    private Vector2 clickPointB;
    private Vector2 clickPointC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount < 2)
        {
            if (uiManager.GetComponent<cshUIControl>().status == "village" || uiManager.GetComponent<cshUIControl>().status == "build" || uiManager.GetComponent<cshUIControl>().status == "build-move")
            {
                // ui 터치는 무시
                if (!IsPointerOverUIObject(Input.mousePosition))
                {
                    touchControl();
                }
            }
        }

        if (Input.touchCount == 2 && uiManager.GetComponent<cshUIControl>().status != "ui")
        {
            float nowDistance = 0f;
            if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began)
            {
                // 터치가 시작되면 두 터치의 간격을 저장
                preDistance = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
            {
                // 터치된 채로 움직이면 두 터치의 간격을 저장하고 시작 간격과 비교하여 카메라 줌인/줌아웃
                nowDistance = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;
                if (preDistance > nowDistance)
                {
                    sizeOrtho += 0.1f;
                }
                else if (preDistance < nowDistance)
                {
                    sizeOrtho -= 0.1f;
                }
                sizeOrtho = Mathf.Clamp(sizeOrtho, 3.0f, 8.0f); // 줌인/줌아웃 범위 제한
                Camera.main.GetComponent<Camera>().orthographicSize = sizeOrtho;
            }
        }
    }

    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = touchPos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    public void touchControl()
    {
        // 터치 시작
        if (Input.GetMouseButtonDown(0))
        {
            clickPointA = Input.mousePosition;
            clickPointB = Input.mousePosition;
        }

        // 터치중
        if (Input.GetMouseButton(0))
        {
            clickPointC = Input.mousePosition;
            // 최근 터치 좌표와 현재 터치 좌표가 다르면
            if (clickPointC.x != clickPointB.x || clickPointC.y != clickPointB.y)
            {
                // 카메라 이동
                prePos = (new Vector2(clickPointC.x, clickPointC.y) - clickPointB) * (-0.2f) * Time.deltaTime;
                Camera.main.transform.Translate(prePos);

                Vector2 pos = Camera.main.transform.position;
                pos.x = Mathf.Clamp(pos.x, -14, 14);
                pos.y = Mathf.Clamp(pos.y, -7, 7);
                Camera.main.transform.position = new Vector3(pos.x, pos.y, -10);

                if (uiManager.GetComponent<cshUIControl>().status == "build")
                {
                    // 선택된 건물을 선택 해제
                    uiManager.GetComponent<cshUIControl>().deselectStructureToModify();
                }
                else if (uiManager.GetComponent<cshUIControl>().status == "build-move")
                {
                    // 건물을 옮기고 있을 경우 카메라의 위치가 다른 땅으로 이동했는지 검사
                    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                    if (hit.collider != null)
                    {
                        if (hit.collider.name.Contains("Ground"))
                        {
                            uiManager.GetComponent<cshUIControl>().checkStructurePosition();
                        }
                    }
                }
            }
            clickPointB = clickPointC;
        }

        // 터치 떼어냄
        if (Input.GetMouseButtonUp(0))
        {
            // 떼어냈을때 좌표와 시작 좌표가 같으면 구조물 Picking 실행
            if (clickPointA.x == clickPointB.x && clickPointA.y == clickPointB.y)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                if (hit.collider != null && hit.collider.name.Contains("Ground"))
                {
                    Transform par = hit.transform.parent;
                    if (uiManager.GetComponent<cshUIControl>().status == "village")
                    {
                        int posX = par.gameObject.GetComponent<cshGroundPosition>().posX;
                        int posY = par.gameObject.GetComponent<cshGroundPosition>().posY;
                        if (par.GetChild(1).transform.childCount > 0)
                        {
                            string name = par.GetChild(1).GetChild(0).name;
                            if (name == "Storage")
                            {
                                // 창고 버튼을 누른것과 같음
                                uiManager.GetComponent<cshUIControl>().openStorage();
                            }
                            else if (name == "Apartment")
                            {
                                // 주민 버튼을 누른것과 같음
                                uiManager.GetComponent<cshUIControl>().openResident();
                            }
                            else if (name != "Cloud")
                            {
                                // 구조물 생산 시간 확인
                                uiManager.GetComponent<cshUIControl>().openStructure(posX, posY);
                            }
                        }
                    }
                    else if (uiManager.GetComponent<cshUIControl>().status == "build")
                    {
                        float posX = hit.transform.position.x;
                        float posY = hit.transform.position.y;
                        if (par.GetChild(1).transform.childCount > 0)
                        {
                            if (par.GetChild(1).transform.GetChild(0).name == "Cloud")
                            {
                                // 구름을 터치하면 원래 선택 되었던 구조물이 선택 해제됨
                                uiManager.GetComponent<cshUIControl>().deselectStructureToModify();
                            }
                            else
                            {
                                // 구조물을 터치하면 구조물이 선택되어 이동 버튼, 철거 버튼 보여짐
                                Camera.main.transform.position = new Vector3(posX, posY, -10);
                                uiManager.GetComponent<cshUIControl>().selectStructureToModify(par);
                            }
                        }
                        else
                        {
                            // 아무것도 지어지지 않은 땅을 터치하면 원래 선택 되었던 구조물이 선택 해제됨
                            uiManager.GetComponent<cshUIControl>().deselectStructureToModify();
                        }
                    }
                }
            }
        }

    }
}

