using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ProgressionState
{
    Neutral,
    Ready,
    InProgress,
    Passed,
    Failed
}

public class QTESystem : MonoBehaviour
{

    [Header("QTE System Properties")]
    [SerializeField, Range(0.0f, 20.0f)] private float QTETimer;
    private float timer;

    [Space][Header("QTE System")]
    [SerializeField] private ProgressionState QTEProgressState = ProgressionState.Neutral;
    [SerializeField] private int numOfQTEPassed = 0;

    [Space][Header("Display Progression")]
    [SerializeField] private GameObject failText;
    [SerializeField] private GameObject passText;
    [SerializeField] private Image timerImage;

    [Space][Header("QTE Sequence Configuration")]
    [SerializeField] private QTESequence m_QTESequence;
    [Tooltip("Add GameObject that represents the keys from QTE Sequence the same order.")]
    [SerializeField] private List<GameObject> keyDisplayObject = new List<GameObject>();


    private void Start()
    {

    }

    public void Initilize()
    {
        if(QTEProgressState == ProgressionState.Ready)
        {
            StartCoroutine(QTELogic());
            numOfQTEPassed = 0;
            timer = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Invoke("Initilize", 1f);
        }

        QTEProgression();
        VisualDisplay();
    }


    IEnumerator QTELogic()
    {
        m_QTESequence.SetUpQTESuccess(m_QTESequence.keys);
        QTEProgressState = ProgressionState.InProgress;

        do
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(m_QTESequence.keys[numOfQTEPassed]))
                {
                    m_QTESequence.UpdateQTESuccess(m_QTESequence.keys[numOfQTEPassed]);
                    numOfQTEPassed++;

                    if (numOfQTEPassed == m_QTESequence.keys.Count)
                    {
                        QTEProgressState = ProgressionState.Passed;
                    }
                }
                else
                {
                    QTEProgressState = ProgressionState.Failed;
                }

            }

            yield return null;
        }
        while (ContinueWithQTE());  
    }

    bool ContinueWithQTE()
    {
        //CHANGE check numOfQTEPassed < m_QTESequence.keys.Count
        return numOfQTEPassed < m_QTESequence.keys.Count &&
                QTEProgressState == ProgressionState.InProgress &&
                m_QTESequence.keys != null;
    }

    void QTEProgression()
    {

        switch(QTEProgressState)
        {
            case ProgressionState.Neutral:

                gameObject.SetActive(false);

                break;
            case ProgressionState.Ready:


                break;
            case ProgressionState.InProgress:

                timer += Time.deltaTime;
                failText.SetActive(false);
                passText.SetActive(false);

                if (timer > QTETimer)
                    QTEProgressState = ProgressionState.Failed;

                break;
            case ProgressionState.Passed:

                failText.SetActive(false);
                passText.SetActive(true);

                break;
            case ProgressionState.Failed:

                failText.SetActive(true);
                passText.SetActive(false);

                break;
                    
        }

    }



    void VisualDisplay()
    {

        if (m_QTESequence.IsValid())
        {
            //need to find a proper way too expensive
            foreach (var key in m_QTESequence.keys)
            {
                int elementIndex = m_QTESequence.keys.IndexOf(key);
                TextMeshProUGUI objText = keyDisplayObject[elementIndex].GetComponent<TextMeshProUGUI>();
                if (objText != null)
                {
                    objText.color = (m_QTESequence.keySuccess[m_QTESequence.keys[elementIndex]]) ? Color.blue : Color.red;
                }
            }
        }

        timerImage.fillAmount = timer / QTETimer;
    }

    public bool PassedSequence()
    {
        bool value = false;

        if(QTEProgressState == ProgressionState.Passed)
            value = true;

        return value;
    }

    public bool FailedSequence()
    {
        bool value = false;

        if(QTEProgressState == ProgressionState.Failed)
            value = true;

        return value;
    }

    public ProgressionState state()
    {
        return QTEProgressState;
    }

    public void ChangeState(ProgressionState state)
    {
        QTEProgressState = state;
    }

    public int NumberOfSequences()
    {
        return m_QTESequence.keys.Count;
    }




}
