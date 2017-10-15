using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TokenResponse
{
    public string access_token;
}

[Serializable]
public class UserInfo
{
    public string name;
}

public class WWWScore : MonoBehaviour {

    [SerializeField]
    private string serverURL = "http://127.0.0.1:8000/";

	public IEnumerator addRecord(int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("score", GameController.score);

        WWW w;
		if (PlayerPrefs.HasKey("Token"))
        {
        	Dictionary<string, string> headers = new Dictionary<string, string>();
        	byte[] rawData = form.data;
        	headers.Add("Authorization", "Bearer " + PlayerPrefs.GetString("Token"));
        	w = new WWW(serverURL + "api/record", rawData, headers);
        } else {
        	w = new WWW(serverURL + "api/anonymrecord", form);
        }

        yield return w;

        if(!string.IsNullOrEmpty(w.error)) {
        	Debug.Log(w.error);
        } else {
        	Debug.Log("Рекорд добавлен!");
        }
    }

    public IEnumerator login(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "password");
        form.AddField("client_id", "3");
        form.AddField("client_secret", "W82LfjDg4DpN2gWlg8Y7eNIUrxkOcyPpA3BM0g3s");
        form.AddField("username", email);
        form.AddField("password", password);
        form.AddField("scope", "*");

        WWW w = new WWW(serverURL + "oauth/token", form);

        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.Log(w.error);
        }
        else
        {
            TokenResponse tokenResponse = JsonUtility.FromJson<TokenResponse>(w.text);

            if (tokenResponse == null)
            {
                Debug.Log("Конвертирование не удалось!");
            }
            else
            {
                // Сохраняем токен в настройках
                PlayerPrefs.SetString("Token", tokenResponse.access_token);
                Debug.Log("Токен установлен!");
                // Запрашиваем имя пользователя
                StartCoroutine(getUserInfo());
            }
        }
    }

    public IEnumerator getUserInfo()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + PlayerPrefs.GetString("Token"));
        WWW w = new WWW(serverURL + "api/user", null, headers);

        yield return w;

        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.Log(w.error);
        }
        else
        {
            UserInfo userInfo = JsonUtility.FromJson<UserInfo>(w.text);

            if (userInfo == null)
            {
                Debug.Log("Конвертирование не удалось!");
            }
            else
            {
                // Сохраняем токен в настройках
                PlayerPrefs.SetString("UserName", userInfo.name);
                Debug.Log("Имя пользователя установлено!");
            }
        }
    }

    public void logout()
    {
        PlayerPrefs.DeleteAll();
    }
}
