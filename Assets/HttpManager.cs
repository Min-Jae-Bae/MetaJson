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

//웹 통신하기 위한 정보
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
            //게임 오브젝트 만든다.
            GameObject go = new GameObject("HttpStudy");
            //만들어진 게임 오브젝트에 httpManager 컴포넌트를 붙이자.
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

    //서버에게 REST API 요청(GET, POST, PUT, DELETE)
    public void SendRequest(HttpInfo httpInfo)
    {
        StartCoroutine(IECoSendRequest(httpInfo));
    }

    private IEnumerator IECoSendRequest(HttpInfo httpInfo)
    {
        UnityWebRequest unityWebRequest = null;

        //요청 방식에 따라서 다르게 처리
        switch (httpInfo.requestType)
        {
            case RequestType.GET:
                //Get 방식으로 정보 셋팅
                unityWebRequest = UnityWebRequest.Get(httpInfo.url);
                break;

            case RequestType.POST:
                unityWebRequest = UnityWebRequest.Post(httpInfo.url, httpInfo.body);
                byte[] byteBody = Encoding.UTF8.GetBytes(httpInfo.body);
                unityWebRequest.uploadHandler = new UploadHandlerRaw(byteBody);
                //헤더 추가
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
        //서버에 요청을 보내고 응답이 올때까지 양보한다.
        yield return unityWebRequest.SendWebRequest();

        //만약에 응답이 성공했다면
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            //print("네트워크 응답: " + unityWebRequest.downloadHandler.text);

            if (httpInfo.onReceive != null)
            {
                httpInfo.onReceive(unityWebRequest.downloadHandler);
            }
        }
        else
        {
            //에러 처리
            print("네트워크 에러: " + unityWebRequest.error);
        }
    }
}