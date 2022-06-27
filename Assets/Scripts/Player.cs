using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Define delegate types and events here
    public delegate void DirectionalInputDelegate(char c);

    public event DirectionalInputDelegate InputEvent = delegate { };

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    private bool moving = false;
    private Vector3 currentDir;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if(node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == false)
        {
            //Implement inputs and event-callbacks here
            if (Input.GetAxis("Vertical") != 0)
            {
                float axis = Input.GetAxis("Vertical");
                if (axis > 0)
                {
                    InputEvent.Invoke('u');
                }
                else if (axis < 0)
                {
                    InputEvent.Invoke('d');
                }

            }
            else if (Input.GetAxis("Horizontal") != 0)
            {
                float axis = Input.GetAxis("Horizontal");
                if (axis > 0)
                {
                    InputEvent.Invoke('r');
                }
                else if (axis < 0)
                {
                    InputEvent.Invoke('l');
                }

            }
        }
        else
        {
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
            }
        }
    }

    //Implement mouse interaction method here
    public void MouseInput(string s)
    {
        if(moving == false)
        {
            InputEvent.Invoke(s[0]);
        }
    }

    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    public void MoveToNode(Node node)
    {
        if (moving == false)
        {
            TargetNode = node;
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }
}
