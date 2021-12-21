using UnityEngine;

public class UniqueID
{
    private string ID;
    public override string ToString()
    {
        return ID;
    }
    public void Init(GameObject go)
    {
        ID = go.transform.position.sqrMagnitude + "-" + go.name + "-" + go.transform.GetSiblingIndex();
    }
}
