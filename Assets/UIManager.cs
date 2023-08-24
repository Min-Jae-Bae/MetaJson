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
        //todos ��û
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.GET, "/todos", (DownloadHandler handler) =>
        {
            print("OnReceiveGet" + handler.text);
        });

        //info�� ������ ��û�� ������
        HttpManager.GetInstance().SendRequest(info);
    }

    public List<CommentInfo> comments;

    public void OnClickComment()
    {
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.GET, "/comments", (DownloadHandler handler) =>
        {
            print("�ڸ�Ʈ ����Ʈ" + handler.text);

            string jsonData = "{\"data\" : " + handler.text + "}";

            //���� ���� jsonData�� ������ �Ľ�����
            JsonList<CommentInfo> commentList = JsonUtility.FromJson<JsonList<CommentInfo>>(jsonData);

            comments = commentList.data;
        });

        //��û
        HttpManager.GetInstance().SendRequest(info);
    }

    public void PostTest()
    {
        HttpInfo info = new HttpInfo();
        info.Set(RequestType.POST, "/sign_up", (DownloadHandler handler) =>
        {
            //Post ������ �������� �� �����κ��� ���� �ɴϴ�~
        });

        SignUpInfo signUpInfo = new SignUpInfo();
        signUpInfo.userName = "�����";
        signUpInfo.age = 26;
        signUpInfo.birthday = "1994-01-01";

        string jsonData = JsonUtility.ToJson(signUpInfo);
        info.body = jsonData;
        //��� ����
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
        //�ٿ�ε��  Image �����͸� Sprite�� �����.
        //Texture2D --> Sprite
        Texture2D texture = ((DownloadHandlerTexture)handler).texture;
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}