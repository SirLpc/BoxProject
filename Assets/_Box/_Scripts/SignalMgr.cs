﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class SignalMgr : MonoBehaviour
{

    private class UnityMessage<T> : UnityEvent<T> { }



}
