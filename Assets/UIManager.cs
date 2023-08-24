using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        HttpManager.GetInstance();
    }

    public void OnClickGet()
    {
        //todos 요청
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.GET, "/todos", (DownloadHandler handler) =>
        {
            print("OnReceiveGet" + handler.text);
        });

        //info의 정보로 요청을 보내자
        HttpManager.GetInstance().SendRequest(info);
    }

    public List<CommentInfo> comments;

    public void OnClickComment()
    {
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.GET, "/comments", (DownloadHandler handler) =>
        {
            print("코멘트 리스트" + handler.text);

            string jsonData = "{\"data\" : " + handler.text + "}";

            //응답 받은 jsonData를 변수에 파싱하자
            JsonList<CommentInfo> commentList = JsonUtility.FromJson<JsonList<CommentInfo>>(jsonData);

            comments = commentList.data;
        });

        //요청
        HttpManager.GetInstance().SendRequest(info);
    }

    public void PostTest()
    {
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.POST, "/sign_up", (DownloadHandler handler) =>
        {
            //Post 데이터 전송했을 때 서버로부터 응답 옵니다~
        });

        SignUpInfo signUpInfo = new SignUpInfo();
        signUpInfo.userName = "배민재";
        signUpInfo.age = 26;
        signUpInfo.birthday = "1994-01-01";

        string jsonData = JsonUtility.ToJson(signUpInfo);
        info.body = jsonData;
        //통신 시작
        HttpManager.GetInstance().SendRequest(info);
    }

    public void OnClickDownladImage()
    {
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.TEXTURE, "https://via.placeholder.com/150/92c952", OnCompleteDownloadTexture, false);
        HttpManager.GetInstance().SendRequest(info);
    }

    private void OnCompleteDownloadTexture(DownloadHandler handler)
    {
        //다운로드된  Image 데이터를 Sprite로 만든다.
        //Texture2D --> Sprite
        Texture2D texture = ((DownloadHandlerTexture)handler).texture;
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}