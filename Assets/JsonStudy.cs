using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[System.Serializable]
public struct UserInfo
{
    //이름
    public string name;

    //나이
    public int age;

    //키
    public float height;

    //성별(true: 여성, false: 남성)
    public bool isFemale;

    //좋아하는 음식 리스트
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
    //나의 정보
    public UserInfo myInfo;

    //유저 정보를 여러개 들고 있는 변수
    public List<UserInfo> friendList = new List<UserInfo>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            myInfo = new UserInfo();

            myInfo.name = "배민재" + i;
            myInfo.age = 26;
            myInfo.height = 180.5f;
            myInfo.isFemale = false;
            myInfo.favoriteFoodList = new List<string>();
            myInfo.favoriteFoodList.Add("치킨");
            myInfo.favoriteFoodList.Add("피자");
            myInfo.favoriteFoodList.Add("햄버거");

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
        //1번 키를 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //myInfo를 Json 형태로 만들자.
            string jsonData = JsonUtility.ToJson(myInfo, true);
            print(jsonData);
            //jsonData를 파일로 저장.
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Create);

            //json string 데이터를 byte 배열로 만든다.
            byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

            //byteData를 file에 쓰자.
            file.Write(byteData, 0, byteData.Length);

            //file을 닫자.
            file.Close();
        }

        //2번 키를 누르면 myinfo.txt를 읽어오자.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FileStream file = new FileStream(Application.dataPath + "/myInfo.txt", FileMode.Open);

            //file의 크기만큼 byte 배열을 할당한다
            byte[] byteData = new byte[file.Length];
            //byteData에 file의 내용을 읽어온다.
            file.Read(byteData, 0, byteData.Length);
            //파일을 닫아주자.
            file.Close();

            //byteData를 string으로 변환한다.
            string jsonData = Encoding.UTF8.GetString(byteData);

            //문자열로 되어있는 jsonData를 myInfo에 parsing 한다.
            myInfo = JsonUtility.FromJson<UserInfo>(jsonData);
        }
    }
}