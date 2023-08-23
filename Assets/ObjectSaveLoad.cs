using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public int type;
    public Transform tr;
}

[System.Serializable]
public class SaveInfo
{
    public int type;
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
}

[System.Serializable]
public class JsonList
{
    public List<SaveInfo> data;
}

public class ObjectSaveLoad : MonoBehaviour
{
    public List<ObjectInfo> objectList = new List<ObjectInfo>();

    private void Update()
    {
        //1번키 누르면 랜덤한 모양, 크기, 위치, 회전이 된 오브젝트 만들자
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //모양을 랜덤하게 뽑자 (0 ~ 3)
            int type = Random.Range(0, 4);
            //type 모양으로 GameObject를 만들자.
            GameObject go = GameObject.CreatePrimitive((PrimitiveType)type);
            //크기, 위치, 회전 랜덤하게 하자.
            go.transform.localScale = Vector3.one * Random.Range(0.5f, 2f);
            go.transform.position = Random.insideUnitSphere * Random.Range(1.0f, 20.0f);
            go.transform.rotation = Random.rotation;

            //만들어진 오브젝트의 정보를 List에 담자.
            ObjectInfo info = new ObjectInfo();
            info.type = type;
            info.tr = go.transform;

            objectList.Add(info);
        }

        //2번키 누르면 objectList의 정보를 json으로 저장
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //objectList를 기반으로 저장할 정보를 빼오자

            List<SaveInfo> saveInfoList = new List<SaveInfo>();
            for (int i = 0; i < objectList.Count; i++)
            {
                SaveInfo saveInfo = new SaveInfo();
                saveInfo.type = objectList[i].type;
                saveInfo.pos = objectList[i].tr.position;
                saveInfo.rot = objectList[i].tr.rotation;
                saveInfo.scale = objectList[i].tr.localScale;

                saveInfoList.Add(saveInfo);
            }

            //saveInfoList을 이용해서 JsonData로 만들자.
            JsonList jsonList = new JsonList();
            jsonList.data = saveInfoList;
            string jsonData = JsonUtility.ToJson(jsonList, true);
            print(jsonData);

            //jsonData를 파일로 저장.
            FileStream file = new FileStream(Application.dataPath + "/objectInfo.txt", FileMode.Create);

            //json string 데이터를 byte 배열로 만든다.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            //byteData를 file에 쓰자.
            file.Write(byteData, 0, byteData.Length);

            //file을 닫자.
            file.Close();
        }

        //3번키 누르면 objectInfo.txt에서 데이터를 읽어서 오브젝트를 만들자.
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FileStream file = new FileStream(Application.dataPath + "/objectInfo.txt", FileMode.Open);

            //file의 크기만큼 byte 배열을 할당한다
            byte[] byteData = new byte[file.Length];
            //byteData에 file의 내용을 읽어온다.
            file.Read(byteData, 0, byteData.Length);
            //파일을 닫아주자.
            file.Close();

            //byteData를 json 형태의 문자열로 만들자.
            string jsonData = Encoding.UTF8.GetString(byteData);

            //jsonData를 이용해서 jsonList에 Parsing 하자
            JsonList jsonList = JsonUtility.FromJson<JsonList>(jsonData);

            //JsonList.data의 갯수 만큼 오브젝트를 생성하자.
            for (int i = 0; i < jsonList.data.Count; i++)
            {
                GameObject go = GameObject.CreatePrimitive((PrimitiveType)jsonList.data[i].type);
                //크기, 위치, 회전 랜덤하게 하자.
                go.transform.localScale = jsonList.data[i].scale;
                go.transform.position = jsonList.data[i].pos;
                go.transform.rotation = jsonList.data[i].rot;
            }
        }
    }
}