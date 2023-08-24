using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum RequestType
{
    GET,
    POST,
    PUT,
    DELETE,
    TEXTURE
}

[Serializable]
public class JsonList<T>
{
    public List<T> data;
}

[Serializable]
public struct CommentInfo
{
    public int postId;
    public int id;
    public string name;
    public string email;
    public string body;
}

[Serializable]
public struct SignUpInfo
{
    public string userName;
    public string birthday;
    public int age;
}

//�� ����ϱ� ���� ����
public class HttpInfo
{
    public RequestType requestType;
    public string url = "";
    public string body;
    public Action<DownloadHandler> onReceive;

    public void Set(RequestType type, string u, Action<DownloadHandler> callback, bool useDefaultUrl = true)
    {
        requestType = type;
        if (useDefaultUrl) url = "https://jsonplaceholder.typicode.com";
        url += u;
        onReceive = callback;
    }
}

public class HttpManager : MonoBehaviour
{
    private static HttpManager Instance;

    public static HttpManager GetInstance()
    {
        if (Instance == null)
        {
            //���� ������Ʈ �����.
            GameObject go = new GameObject("HttpStudy");
            //������� ���� ������Ʈ�� httpManager ������Ʈ�� ������.
            go.AddComponent<HttpManager>();
        }

        return Instance;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //�������� REST API ��û(GET, POST, PUT, DELETE)
    public void SendRequest(HttpInfo httpInfo)
    {
        StartCoroutine(IECoSendRequest(httpInfo));
    }

    private IEnumerator IECoSendRequest(HttpInfo httpInfo)
    {
        UnityWebRequest unityWebRequest = null;

        //��û ��Ŀ� ���� �ٸ��� ó��
        switch (httpInfo.requestType)
        {
            case RequestType.GET:
                //Get ������� ���� ����
                unityWebRequest = UnityWebRequest.Get(httpInfo.url);
                break;

            case RequestType.POST:
                unityWebRequest = UnityWebRequest.Post(httpInfo.url, httpInfo.body);
                byte[] byteBody = Encoding.UTF8.GetBytes(httpInfo.body);
                unityWebRequest.uploadHandler = new UploadHandlerRaw(byteBody);
                //��� �߰�
                unityWebRequest.SetRequestHeader("Content-type", "application/json");
                break;

            case RequestType.PUT:
                unityWebRequest = UnityWebRequest.Put(httpInfo.url, "");
                break;

            case RequestType.DELETE:
                unityWebRequest = UnityWebRequest.Delete(httpInfo.url);
                break;

            case RequestType.TEXTURE:
                unityWebRequest = UnityWebRequestTexture.GetTexture(httpInfo.url);
                break;
        }
        //������ ��û�� ������ ������ �ö����� �纸�Ѵ�.
        yield return unityWebRequest.SendWebRequest();

        //���࿡ ������ �����ߴٸ�
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            //print("��Ʈ��ũ ����: " + unityWebRequest.downloadHandler.text);

            if (httpInfo.onReceive != null)
            {
                httpInfo.onReceive(unityWebRequest.downloadHandler);
            }
        }
        else
        {
            //���� ó��
            print("��Ʈ��ũ ����: " + unityWebRequest.error);
        }
    }
}