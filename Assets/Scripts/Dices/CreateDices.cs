using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateDices : MonoBehaviour
{

    [SerializeField]
    GameObject Dice;

    [SerializeField]
    public bool CanRollDices = true;

    public void RollDices()
    {
        if (CanRollDices)
        {
            FindObjectOfType<DisableEndTurnButton>().DisableButton(true);

            DestroyOldDices();
            CreateSomeDices();

            CanRollDices = false;
        }
    }

    private static void DestroyOldDices()
    {
        var array = FindObjectsOfType<Dice>();

        for (int i = 0; i < array.Length; i++)
        {
            Destroy(array[i].gameObject);
        }
    }

    private void CreateSomeDices()
    {
        CreateDice(Vector3.left);
        CreateDice(Vector3.right);
    }

    private void CreateDice(Vector3 vector3)
    {
        var DiceClone = Instantiate(Dice);
        DiceClone.transform.position = transform.position + Vector3.down + vector3;
        DiceClone.GetComponent<Rigidbody>().MoveRotation(Random.rotation);

        Vector3 direction = new Vector3((-transform.position.x), (-transform.position.y), (-transform.position.z));
        direction.y *= -1f;

        if (transform.position.y > 2)
            DiceClone.GetComponent<Rigidbody>().AddForce(direction * 25f);

        else
        {
            direction.y *= 3;
            DiceClone.GetComponent<Rigidbody>().AddForce(direction * 35f);
        }
    }
}
