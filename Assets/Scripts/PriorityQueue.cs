using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<T> data;
    private IComparer<T> comparer;

    public PriorityQueue(IComparer<T> comparer)
    {
        this.data = new List<T>();
        this.comparer = comparer;
    }

    public void Enqueue(T item)
    {
        data.Add(item);
        int i = data.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (comparer.Compare(data[parent], data[i]) <= 0)
                break;

            T tmp = data[parent];
            data[parent] = data[i];
            data[i] = tmp;
            i = parent;
        }
    }

    public T Dequeue()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("PriorityQueue is empty");

        T frontItem = data[0];
        int lastIndex = data.Count - 1;
        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);

        int current = 0;
        while (true)
        {
            int childIndex = current * 2 + 1;
            if (childIndex > lastIndex)
                break;

            int rightChildIndex = childIndex + 1;
            if (rightChildIndex <= lastIndex && comparer.Compare(data[childIndex], data[rightChildIndex]) > 0)
                childIndex = rightChildIndex;

            if (comparer.Compare(data[current], data[childIndex]) <= 0)
                break;

            T tmp = data[current];
            data[current] = data[childIndex];
            data[childIndex] = tmp;
            current = childIndex;
        }

        return frontItem;
    }

    public int Count
    {
        get { return data.Count; }
    }
}