using UnityEngine;
using System.Collections.Generic;

public class DisposalTracker : MonoBehaviour
{
    public static DisposalTracker Instance;

    private Dictionary<string, int> disposalCounts = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RecordDisposal(string binTag)
    {
        if (disposalCounts.ContainsKey(binTag))
            disposalCounts[binTag]++;
        else
            disposalCounts[binTag] = 1;
    }

    public Dictionary<string, int> GetDisposalSummary()
    {
        return disposalCounts;
    }
}
