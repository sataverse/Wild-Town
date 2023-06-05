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
                // ui ��ġ�� ����
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
                // ��ġ�� ���۵Ǹ� �� ��ġ�� ������ ����
                preDistance = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;
            }
            else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
            {
                // ��ġ�� ä�� �����̸� �� ��ġ�� ������ �����ϰ� ���� ���ݰ� ���Ͽ� ī�޶� ����/�ܾƿ�
                nowDistance = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;
                if (preDistance > nowDistance)
                {
                    sizeOrtho += 0.1f;
                }
                else if (preDistance < nowDistance)
                {
                    sizeOrtho -= 0.1f;
                }
                sizeOrtho = Mathf.Clamp(sizeOrtho, 3.0f, 8.0f); // ����/�ܾƿ� ���� ����
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
        // ��ġ ����
        if (Input.GetMouseButtonDown(0))
        {
            clickPointA = Input.mousePosition;
            clickPointB = Input.mousePosition;
        }

        // ��ġ��
        if (Input.GetMouseButton(0))
        {
            clickPointC = Input.mousePosition;
            // �ֱ� ��ġ ��ǥ�� ���� ��ġ ��ǥ�� �ٸ���
            if (clickPointC.x != clickPointB.x || clickPointC.y != clickPointB.y)
            {
                // ī�޶� �̵�
                prePos = (new Vector2(clickPointC.x, clickPointC.y) - clickPointB) * (-0.2f) * Time.deltaTime;
                Camera.main.transform.Translate(prePos);

                Vector2 pos = Camera.main.transform.position;
                pos.x = Mathf.Clamp(pos.x, -14, 14);
                pos.y = Mathf.Clamp(pos.y, -7, 7);
                Camera.main.transform.position = new Vector3(pos.x, pos.y, -10);

                if (uiManager.GetComponent<cshUIControl>().status == "build")
                {
                    // ���õ� �ǹ��� ���� ����
                    uiManager.GetComponent<cshUIControl>().deselectStructureToModify();
                }
                else if (uiManager.GetComponent<cshUIControl>().status == "build-move")
                {
                    // �ǹ��� �ű�� ���� ��� ī�޶��� ��ġ�� �ٸ� ������ �̵��ߴ��� �˻�
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

        // ��ġ ���
        if (Input.GetMouseButtonUp(0))
        {
            // ��������� ��ǥ�� ���� ��ǥ�� ������ ������ Picking ����
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
                                // â�� ��ư�� �����Ͱ� ����
                                uiManager.GetComponent<cshUIControl>().openStorage();
                            }
                            else if (name == "Apartment")
                            {
                                // �ֹ� ��ư�� �����Ͱ� ����
                                uiManager.GetComponent<cshUIControl>().openResident();
                            }
                            else if (name != "Cloud")
                            {
                                // ������ ���� �ð� Ȯ��
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
                                // ������ ��ġ�ϸ� ���� ���� �Ǿ��� �������� ���� ������
                                uiManager.GetComponent<cshUIControl>().deselectStructureToModify();
                            }
                            else
                            {
                                // �������� ��ġ�ϸ� �������� ���õǾ� �̵� ��ư, ö�� ��ư ������
                                Camera.main.transform.position = new Vector3(posX, posY, -10);
                                uiManager.GetComponent<cshUIControl>().selectStructureToModify(par);
                            }
                        }
                        else
                        {
                            // �ƹ��͵� �������� ���� ���� ��ġ�ϸ� ���� ���� �Ǿ��� �������� ���� ������
                            uiManager.GetComponent<cshUIControl>().deselectStructureToModify();
                        }
                    }
                }
            }
        }

    }
}

