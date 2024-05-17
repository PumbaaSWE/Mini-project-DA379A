using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PedestrianHandler : MonoBehaviour
{
    [SerializeField]
    private int minimum;

    [SerializeField] 
    private int maximum;

    [SerializeField]
    private Graph graph;

    [SerializeField]
    private Pedestrian pedestrian;

    private ObjectPool<Pedestrian> pool;

    public ObjectPool<Pedestrian> Pool { get => pool; }

    private Node[] walkableNodes;

    // Start is called before the first frame update
    void Start()
    {
        pool = new ObjectPool<Pedestrian>(CreatePedestrian, OnGetPedestrian, OnReleasePedestrian, OnDestroyPedestrian, true, minimum, maximum);

        List<Node> walkables = new();

        for(int i = 0; i < graph.XLength; i++)
        {
            for(int j = 0; j < graph.YLength; j++)
            {
                Node node = graph.GetNode(i, i);

                if(!node.IsBlocked)
                {
                    walkables.Add(node);
                }
            }
        }

        walkableNodes = walkables.ToArray();

        for(int i = 0; i < minimum; i++)
        {
            Node[] goals = new Node[3];
            Node start = walkableNodes[Random.Range(0, walkableNodes.Length - 1)];

            goals[2] = start;

            while (goals[0] == null || goals[1] == null)
            {
                Node next = walkableNodes[Random.Range(0, walkableNodes.Length - 1)];

                if (next != start && next != goals[0] && next != goals[1])
                {
                    if (goals[0] == null)
                    {
                        goals[0] = next;
                        continue;
                    }

                    if (goals[1] == null)
                    {
                        goals[1] = next;
                        continue;
                    }
                }
            }

            Pedestrian pedestrian = pool.Get();
            pedestrian.InitPedestrian(start, goals, graph, this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Pedestrian CreatePedestrian()
    {
        Pedestrian pedestrian = Instantiate(this.pedestrian);

        pedestrian.PedestrianStart();

        return pedestrian;
    }

    private void OnGetPedestrian(Pedestrian pedestrian)
    {
        pedestrian.gameObject.SetActive(true);
    }

    private void OnReleasePedestrian(Pedestrian pedestrian)
    {
        pedestrian.gameObject.SetActive(false);
    }

    private void OnDestroyPedestrian(Pedestrian pedestrian)
    {
        Destroy(pedestrian.gameObject);
    }
}