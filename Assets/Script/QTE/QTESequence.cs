using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Test
{
    public KeyCode key;
    public bool passed;
}

[CreateAssetMenu(fileName = "QTE Sequence Data", menuName = "ScriptableObjects/QTE System", order = 1)]
public class QTESequence : ScriptableObject
{
    public List<KeyCode> keys = new List<KeyCode>();
    public Dictionary<KeyCode, bool> keySuccess = new Dictionary<KeyCode, bool>();

    public List<Test> testKey = new List<Test>();   //going to convert to a struct later, because using a dictionary error occurs when having mulipy keys appear twice

    //public List<GameObject> keysDisplayObject = new List<GameObject>();     //find a work around

    public void SetUpQTESuccess(List<KeyCode> keys)
    {
        keySuccess.Clear();
        foreach (KeyCode key in keys)
        {
            keySuccess.Add(key, false);
        }
    }


    public void UpdateQTESuccess(KeyCode key)
    {
        keySuccess[key] = true;
    }

    public bool IsValid()
    {
        return keySuccess.Count > 0;
    }

}
