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
        //1��Ű ������ ������ ���, ũ��, ��ġ, ȸ���� �� ������Ʈ ������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //����� �����ϰ� ���� (0 ~ 3)
            int type = Random.Range(0, 4);
            //type ������� GameObject�� ������.
            GameObject go = GameObject.CreatePrimitive((PrimitiveType)type);
            //ũ��, ��ġ, ȸ�� �����ϰ� ����.
            go.transform.localScale = Vector3.one * Random.Range(0.5f, 2f);
            go.transform.position = Random.insideUnitSphere * Random.Range(1.0f, 20.0f);
            go.transform.rotation = Random.rotation;

            //������� ������Ʈ�� ������ List�� ����.
            ObjectInfo info = new ObjectInfo();
            info.type = type;
            info.tr = go.transform;

            objectList.Add(info);
        }

        //2��Ű ������ objectList�� ������ json���� ����
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //objectList�� ������� ������ ������ ������

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

            //saveInfoList�� �̿��ؼ� JsonData�� ������.
            JsonList jsonList = new JsonList();
            jsonList.data = saveInfoList;
            string jsonData = JsonUtility.ToJson(jsonList, true);
            print(jsonData);

            //jsonData�� ���Ϸ� ����.
            FileStream file = new FileStream(Application.dataPath + "/objectInfo.txt", FileMode.Create);

            //json string �����͸� byte �迭�� �����.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            //byteData�� file�� ����.
            file.Write(byteData, 0, byteData.Length);

            //file�� ����.
            file.Close();
        }

        //3��Ű ������ objectInfo.txt���� �����͸� �о ������Ʈ�� ������.
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FileStream file = new FileStream(Application.dataPath + "/objectInfo.txt", FileMode.Open);

            //file�� ũ�⸸ŭ byte �迭�� �Ҵ��Ѵ�
            byte[] byteData = new byte[file.Length];
            //byteData�� file�� ������ �о�´�.
            file.Read(byteData, 0, byteData.Length);
            //������ �ݾ�����.
            file.Close();

            //byteData�� json ������ ���ڿ��� ������.
            string jsonData = Encoding.UTF8.GetString(byteData);

            //jsonData�� �̿��ؼ� jsonList�� Parsing ����
            JsonList jsonList = JsonUtility.FromJson<JsonList>(jsonData);

            //JsonList.data�� ���� ��ŭ ������Ʈ�� ��������.
            for (int i = 0; i < jsonList.data.Count; i++)
            {
                GameObject go = GameObject.CreatePrimitive((PrimitiveType)jsonList.data[i].type);
                //ũ��, ��ġ, ȸ�� �����ϰ� ����.
                go.transform.localScale = jsonList.data[i].scale;
                go.transform.position = jsonList.data[i].pos;
                go.transform.rotation = jsonList.data[i].rot;
            }
        }
    }
}