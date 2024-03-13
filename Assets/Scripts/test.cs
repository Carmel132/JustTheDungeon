using UnityEngine;

public class test : MonoBehaviour, IPlayerMessages
{

    public GameObject EM;
    // Start is called before the first frame update
    void Start()
    {

        EM.GetComponent<EventManager>().registerEvent(EventGroup.Player, gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerMove(Transform player)
    {
        //Debug.Log("messages are so cool!");
    }
}
