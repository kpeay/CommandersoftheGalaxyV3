using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimController : MonoBehaviour {

    Animator npcAnim;

    // Use this for initialization
    void Start()
    {
        npcAnim = this.gameObject.GetComponent<Animator>();
        Debug.Log(npcAnim);
        Debug.Log(this.gameObject);
        Debug.Log(" ");
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.parent.gameObject.GetComponent<NPCMove>().animateMove)
        {
            npcAnim.SetBool("Running", true);
            Debug.Log(transform.parent.gameObject.GetComponent<NPCMove>().animateMove);
        }

        if (!transform.parent.gameObject.GetComponent<NPCMove>().animateMove)
        {
            npcAnim.SetBool("Running", false);
            //Debug.Log(transform.parent.gameObject.GetComponent<NPCMove>().animateMove);
        }

        if (transform.parent.gameObject.GetComponent<NPCMove>().animateAttack)
        {
            npcAnim.SetBool("Attacking", true);
            Debug.Log(transform.gameObject.GetComponent<NPCMove>().animateAttack);
        }
        else
        {
            npcAnim.SetBool("Attacking", false);
            //Debug.Log(transform.parent.gameObject.GetComponent<NPCMove>().animateAttack);
        }
    }
}
