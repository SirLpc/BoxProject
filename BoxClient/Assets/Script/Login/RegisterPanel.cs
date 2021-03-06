﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using OneByOne;

public class RegisterPanel : MonoBehaviour {

    public InputField acc;

    public InputField pwd;

    public InputField pwdagin;
    public Button regBtn;

    public Button cBtn;

    public void open() {
        regBtn.gameObject.SetActive(true);
        cBtn.gameObject.SetActive(true);
        gameObject.SetActive(true);

    }

    public void AutoRegister()
    {
        AccountDTO dto = new AccountDTO();
        dto.account = LocalPlayerPrefabs.GetId();
        dto.password = LocalPlayerPrefabs.GetPwd();
        NetWorkScript.Instance.write(Protocol.TYPE_LOGIN, -1, LoginProtocol.CREATE_CREQ, dto);
    }

    public void registerClick() {
        if (acc.text == string.Empty || pwd.text == string.Empty) {
            GameData.errors.Add(new ErrorModel("请输入正确的帐号密码"));
            return;
        }
        if (pwd.text != pwdagin.text) {
            GameData.errors.Add(new ErrorModel("两次输入密码不一致"));
            return;
        }
       AccountDTO dto= new AccountDTO();
        dto.account=acc.text;
        dto.password=pwd.text;
        acc.text = string.Empty;
        pwd.text = string.Empty;
        pwdagin.text = string.Empty;
        regBtn.gameObject.SetActive(false);
        cBtn.gameObject.SetActive(false);
        NetWorkScript.Instance.write(Protocol.TYPE_LOGIN, -1, LoginProtocol.CREATE_CREQ, dto);
        
    }
    public void closeClick() {
        gameObject.SetActive(false);
    }

}
