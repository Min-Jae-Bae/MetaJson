using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct UserInfo
{
    //�̸�
    public string name;

    //����
    public int age;

    //Ű
    public float height;

    //����(true: ����, false: ����)
    public bool isFemale;

    //�����ϴ� ���� ����Ʈ
    public List<string> favoriteFoodList;
}

[System.Serializable]
public struct JsonDataTest
{
    public UserInfo jsonData;
}

[System.Serializable]
public struct FriendInfo
{
    public List<UserInfo> data;
}

public class JsonStudy : MonoBehaviour
{
    //���� ����
    public UserInfo myInfo;

    //���� ������ ������ ��� �ִ� ����
    public List<UserInfo> friendList = new List<UserInfo>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            myInfo = new UserInfo();

            myInfo.name = "�����" + i;
            myInfo.age = 26;
            myInfo.height = 180.5f;
            myInfo.isFemale = false;
            myInfo.favoriteFoodList = new List<string>();
            myInfo.favoriteFoodList.Add("ġŲ");
            myInfo.favoriteFoodList.Add("����");
            myInfo.favoriteFoodList.Add("�ܹ���");

            friendList.Add(myInfo);
        }

        FriendInfo info = new FriendInfo();
        info.data = friendList;

        /*        JsonDataTest test = new JsonDataTest();
                test.jsonData = myInfo;*/

        string s = JsonUtility.ToJson(info, true);
        print(s);
    }

    private void Update()
    {
        //1�� Ű�� ������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //myInfo�� Json ���·� ������.
            string jsonData = JsonUtility.ToJson(myInfo, true);
            print(jsonData);
            //jsonData�� ���Ϸ� ����.
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Create);

            //json string �����͸� byte �迭�� �����.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            //byteData�� file�� ����.
            file.Write(byteData, 0, byteData.Length);

            //file�� ����.
            file.Close();
        }

        //2�� Ű�� ������ myinfo.txt�� �о����.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Open);

            //file�� ũ�⸸ŭ byte �迭�� �Ҵ��Ѵ�
            byte[] byteData = new byte[file.Length];
            //byteData�� file�� ������ �о�´�.
            file.Read(byteData, 0, byteData.Length);
            //������ �ݾ�����.
            file.Close();

            //byteData�� string���� ��ȯ�Ѵ�.
            string jsonData = Encoding.UTF8.GetString(byteData);

            //���ڿ��� �Ǿ��ִ� jsonData�� myInfo�� parsing �Ѵ�.
            myInfo = JsonUtility.FromJson<UserInfo>(jsonData);
        }
    }
}