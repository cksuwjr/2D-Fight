using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPLine_Admin : MonoBehaviour
{
    int now_Count;
    GameObject HPLineChild;
    Status status;

    // Update is called once per frame
    void Start()
    {
        status = GetComponentInParent<Status>();
        HPLineChild = Resources.Load<GameObject>("HP/HPLineChild");


        now_Count = 0;
        SpawnNewHPLine((int)(status.Max_HP / 100));


    }
    void Update()
    {
        int maxHPcount;
        maxHPcount = (int)status.Max_HP / 100;
        if (now_Count != maxHPcount) {
            if (now_Count < maxHPcount)
                SpawnNewHPLine(maxHPcount - now_Count);
            else
                DeSpawnHPLine(now_Count - maxHPcount);

        }
    }
    void SpawnNewHPLine(int count)
    {

        GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        for (int i = 0; i < count; i++)
        {
            GameObject spawned;
            spawned = Instantiate(HPLineChild, Vector2.zero, Quaternion.identity);
            spawned.transform.SetParent(transform);
            now_Count++;
            
        }
        SetLocalPositionofChild();
        GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }
    void DeSpawnHPLine(int count)
    {
        GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        for (int i = 0; i < count; i++)
        {
            Destroy(transform.GetChild(now_Count).gameObject);
            now_Count--;
        }
        SetLocalPositionofChild();
        GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }

    void SetLocalPositionofChild()
    {
        float scaleX = 500f / status.Max_HP;
        for (int i = 0; i < now_Count; i++)
            transform.GetChild(i).localScale = new Vector2(scaleX, 1);
        Debug.Log("현재체력: " + status.Max_HP);
    }
}
