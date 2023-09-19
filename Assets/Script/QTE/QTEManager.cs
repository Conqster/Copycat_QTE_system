using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QTEStatus
{
    public QTESystem system;
    public ProgressionState progressionState;
    public int numberOfSequenceKeys = 0;

    public ProgressionState m_progressState
    {
        set
        {
            system.ChangeState(value);
            this.progressionState = value;
        }
    }

    public QTEStatus(QTESystem system, ProgressionState state, int numberOfSequenceKeys)
    {
        this.system = system;
        this.progressionState = state;
        this.numberOfSequenceKeys = numberOfSequenceKeys;
    }

    public void UpdateSystem()
    {
        this.progressionState = system.state();
    }

    public void ActiveQTESystem()
    {
        this.m_progressState = ProgressionState.Ready;
        system.gameObject.SetActive(true);
    }
}

public class QTEManager : MonoBehaviour
{
    [SerializeField] private List<QTEStatus> QTESystemStatus = new List<QTEStatus>();

    private void Start()
    {
        UpdateQTESystemWithState();

        ChooseASystemToUse();
    }


    private void Update()
    {
        CheckStatuesOfAllSystems();
    }


    private void UpdateQTESystemWithState()
    {
        QTESystemStatus.Clear();
        foreach (Transform child in transform)
        {
            QTESystem system = child.GetComponent<QTESystem>();
            if (system != null)
            {
                QTESystemStatus.Add(new QTEStatus(system, system.state(), system.NumberOfSequences()));
            }
        }
    }

    private void CheckStatuesOfAllSystems()
    {
        foreach(QTEStatus statues in QTESystemStatus)
        {
            statues.UpdateSystem();
        }
    }


    private void ChooseASystemToUse()
    {
        int rndIndex = 0;
        int maxIndex = QTESystemStatus.Count;

        rndIndex = Random.Range(0, maxIndex);

        foreach (QTEStatus statues in QTESystemStatus)
        {
            statues.progressionState = ProgressionState.Neutral;
        }

        QTESystemStatus[rndIndex].ActiveQTESystem();
    }


}
