using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<T> data;
    private IComparer<T> comparer;

    public PriorityQueue()
    {
        this.data = new List<T>();
        this.comparer = Comparer<T>.Default;
    }

    public PriorityQueue(IComparer<T> comparer)
    {
        this.data = new List<T>();
        this.comparer = comparer;
    }

    public int Count => data.Count;

    public void Enqueue(T item)
    {
        data.Add(item);
        int childIndex = data.Count - 1;

        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (comparer.Compare(data[parentIndex], data[childIndex]) <= 0)
                break;

            Swap(parentIndex, childIndex);
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("PriorityQueue is empty");

        int lastIndex = data.Count - 1;
        T frontItem = data[0];
        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);

        lastIndex--;

        int parentIndex = 0;
        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;
            if (leftChildIndex > lastIndex)
                break;

            int rightChildIndex = leftChildIndex + 1;
            int childIndex = (rightChildIndex <= lastIndex && comparer.Compare(data[leftChildIndex], data[rightChildIndex]) > 0) ? rightChildIndex : leftChildIndex;

            if (comparer.Compare(data[parentIndex], data[childIndex]) <= 0)
                break;

            Swap(parentIndex, childIndex);
            parentIndex = childIndex;
        }

        return frontItem;
    }

    public T Peek()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("PriorityQueue is empty");

        return data[0];
    }

    private void Swap(int indexA, int indexB)
    {
        T temp = data[indexA];
        data[indexA] = data[indexB];
        data[indexB] = temp;
    }
}