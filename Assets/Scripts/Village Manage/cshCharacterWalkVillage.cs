using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshCharacterWalkVillage : MonoBehaviour
{
    private cshStructure structureInformation;
    public int posX;
    public int posY;
    public bool isStart = false;
    private int status = 1; // 0: 쉬기  1: 찾기  2: 움직임
    private int destination = -1;
    private float currentTime = 0f;
    private float moveTime = 0f;
    private float idleTime = 1f;
    private float moveDistanceX = 0f;
    private float moveDistanceY = 0f;
    private Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        structureInformation = GameObject.Find("Structure Manager").GetComponent<cshStructure>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if(status == 0)
            {
                // 주민이 가만히 있는 시간
                currentTime += Time.deltaTime;
                if(currentTime > moveTime + idleTime)
                {
                    currentTime = 0f;
                    status = 1;
                }
            }
            else if (status == 1)
            {
                // 이동할 곳을 탐색 : 건물이 없는 위치
                System.Random r = new System.Random();
                List<int> routeList = new List<int>();
                startPos = transform.position;
                if (structureInformation.structureArray[posX - 1, posY] == null)
                {
                    if(posX > 3) routeList.Add(0);
                }
                if (structureInformation.structureArray[posX + 1, posY] == null)
                {
                    if (posX < 17) routeList.Add(1); 
                }
                if (structureInformation.structureArray[posX, posY + 1] == null)
                {
                    if (posY < 17) routeList.Add(2);
                }
                if (structureInformation.structureArray[posX, posY - 1] == null)
                {
                    if (posY > 3) routeList.Add(3);
                }
                if (structureInformation.structureArray[posX - 1, posY + 1] == null)
                {
                    if (posX > 3 && posY < 17) routeList.Add(4);
                }
                if (structureInformation.structureArray[posX + 1, posY + 1] == null)
                {
                    if (posX < 17 && posY < 17) routeList.Add(5);
                }
                if (structureInformation.structureArray[posX - 1, posY - 1] == null)
                {
                    if (posX > 3 && posY > 3) routeList.Add(6);
                }
                if (structureInformation.structureArray[posX + 1, posY - 1] == null)
                {
                    if (posX < 17 && posY > 3) routeList.Add(7);
                }

                if(routeList.Count > 0)
                {
                    int n = r.Next(0, routeList.Count);
                    destination = routeList[n];
                    if(destination < 4)
                    {
                        if(destination == 0)
                        {
                            moveDistanceX = 1f;
                            moveDistanceY = 0.5f;
                            posX -= 1;
                        }
                        else if(destination == 1)
                        {
                            moveDistanceX = -1f;
                            moveDistanceY = -0.5f;
                            posX += 1;
                        }
                        else if(destination == 2)
                        {
                            moveDistanceX = 1f;
                            moveDistanceY = -0.5f;
                            posY += 1;
                        }
                        else
                        {
                            moveDistanceX = -1f;
                            moveDistanceY = 0.5f;
                            posY -= 1;
                        }
                        moveTime = 4f;
                    }
                    else if(destination == 4 || destination == 7)
                    {
                        if(destination == 4)
                        {
                            moveDistanceX = 2f;
                            moveDistanceY = 0f;
                            posX -= 1;
                            posY += 1;
                        }
                        else
                        {
                            moveDistanceX = -2f;
                            moveDistanceY = 0f;
                            posX += 1;
                            posY -= 1;
                        }
                        moveTime = 6f;
                    }
                    else
                    {
                        if(destination == 5)
                        {
                            moveDistanceX = 0f;
                            moveDistanceY = -1f;
                            posX += 1;
                            posY += 1;
                        }
                        else
                        {
                            moveDistanceX = 0f;
                            moveDistanceY = 1f;
                            posX -= 1;
                            posY -= 1;
                        }
                        moveTime = 3f;
                    }
                    idleTime = r.Next(5, 16);
                    status = 2;
                }
            }
            else
            {
                // 주민이 정한 위치로 n초 동안 이동
                currentTime += Time.deltaTime;
                if(currentTime > moveTime)
                {
                    status = 0;
                }
                else
                {
                    if(currentTime > moveTime / 2 && transform.GetComponent<SpriteRenderer>().sortingOrder != 2 * (posX + posY) + 1)
                    {
                        transform.GetComponent<SpriteRenderer>().sortingOrder = 2 * (posX + posY) + 1;
                    }
                    transform.position = Vector2.Lerp(startPos, new Vector2(startPos.x + moveDistanceX, startPos.y + moveDistanceY), currentTime / moveTime);
                }
            }
        }
    }

    public void startSetting(int x, int y)
    {
        posX = x;
        posY = y;
        transform.GetComponent<SpriteRenderer>().sortingOrder = 2 * (posX + posY) + 1;
        status = 1;
        isStart = true;
    }
}
