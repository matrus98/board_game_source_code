using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class GlobalSettingsPlayerColorArray
{
    public GameObject Material;
    public bool IsMaterialInUse;
}

public class GlobalSettingsPlayerColor : MonoBehaviour
{
    [SerializeField]
    private GlobalSettingsPlayerColorArray[] Materials;

    public GlobalSettingsPlayerColorArray[] GetListOFPlayerPawnColors()
    {
        return Materials;
    }

    /// <summary>
    /// Return a Random Material for Pawn
    /// </summary>
    /// <returns>RandomMaterial</returns>
    public GameObject GetRandomPawn()
    {
        bool IsColorChoosen = false;
        int index = Random.Range(0, 5);
        //Debug.Log("Index jest równy: " + index);
        while (!IsColorChoosen)
        {
            index = Random.Range(0, 5);

            if (!Materials[index].IsMaterialInUse)
            {
                IsColorChoosen = true;
                CmdChangeAvaibilityOfColor(index);
            }
        }
        Debug.Log("Index jest równy: " + index);
        return Materials[index].Material;
    }

    public GameObject GetParticularPawn(int index)
    {
        if (!Materials[index].IsMaterialInUse)
        {
            Materials[index].IsMaterialInUse = true;
            return Materials[index].Material;
        }
        return null;
    }

    private void CmdChangeAvaibilityOfColor(int index)
    {
        Materials[index].IsMaterialInUse = true;
    }

}