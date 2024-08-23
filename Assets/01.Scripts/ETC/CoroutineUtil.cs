using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtil
{
    private static GameObject _coroutineObj;
    private static CoroutineExecutor _coroutineExecutor;

    static CoroutineUtil()
    {
        CreateCoroutineExecutor();
    }

    private static void CreateCoroutineExecutor()
    {
        if (_coroutineObj != null)
        {
            UnityEngine.Object.Destroy(_coroutineObj);
        }

        _coroutineObj = new GameObject("CoroutineObj");
        UnityEngine.Object.DontDestroyOnLoad(_coroutineObj);
        _coroutineExecutor = _coroutineObj.AddComponent<CoroutineExecutor>();
    }

    private static void EnsureCoroutineExecutor()
    {
        if (_coroutineExecutor == null)
        {
            CreateCoroutineExecutor();
        }
    }

    public static void CallWaitForOneFrame(Action action)
    {
        EnsureCoroutineExecutor();
        _coroutineExecutor.StartCoroutine(DoCallWaitForOneFrame(action));
    }

    public static void CallWaitForSeconds(float seconds, Action afterAction)
    {
        EnsureCoroutineExecutor();
        _coroutineExecutor.StartCoroutine(DoCallWaitForSeconds(seconds, afterAction));
    }

    public static void CallWaitForAction(Func<bool> predicate, Action afterAction = null)
    {
        EnsureCoroutineExecutor();
        _coroutineExecutor.StartCoroutine(DoCallWaitForAction(predicate, afterAction));
    }

    public static void CallWaitForActionUntilTrue(Func<bool> predicate, Action action, float heartBeat = 0.02f, Action afterAction = null)
    {
        EnsureCoroutineExecutor();
        _coroutineExecutor.StartCoroutine(DoCallWaitForActionUntilTrue(heartBeat, predicate, action, afterAction));
    }

    private static IEnumerator DoCallWaitForOneFrame(Action action)
    {
        yield return null;
        action?.Invoke();
    }

    private static IEnumerator DoCallWaitForSeconds(float seconds, Action afterAction)
    {
        yield return new WaitForSeconds(seconds);
        afterAction?.Invoke();
    }

    private static IEnumerator DoCallWaitForAction(Func<bool> predicate, Action afterAction)
    {
        yield return new WaitUntil(predicate);
        afterAction?.Invoke();
    }

    private static IEnumerator DoCallWaitForActionUntilTrue(float heartBeat, Func<bool> predicate, Action action, Action afterAction)
    {
        while (!predicate())
        {
            yield return new WaitForSeconds(heartBeat);
            action?.Invoke();
        }

        afterAction?.Invoke();
    }

    private class CoroutineExecutor : MonoBehaviour { }
}
