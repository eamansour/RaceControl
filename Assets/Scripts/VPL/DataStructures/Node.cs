using System.Collections.Generic;
using System.Linq;

public class Node<T> : IDataStructure<T>
{
    public Node<T> Next { get; set; } = null;
    public T Data { get; set; } = default(T);

    public Node()
    {
        Next = null;
        Data = default(T);
    }

    // Creates a linked node from a given collection
    public Node(IEnumerable<T> collection)
    {
        List<T> collectionList = collection.ToList();
        Node<T> head = null;
        for (int i = collectionList.Count - 1; i >= 0; i--)
        {
            head = Insert(head, collectionList[i]);
        }
        Next = head.Next;
        Data = head.Data;
    }

    // Insert a new linked node with a given value before a given node, returning the new node
    public Node<T> Insert(Node<T> node, T value)
    {
        Node<T> head = new Node<T>();
        head.Data = value;
        head.Next = node;
        node = head;
        return node;
    }

    public T GetContainedValue()
    {
        return Data;
    }
}
