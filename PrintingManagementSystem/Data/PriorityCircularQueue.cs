using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintingManagementSystem.Data
{
    public class PriorityCircularQueue<T> where T : IComparable<T>
    {
        private readonly T[] _buffer;
        private readonly int _capacity;
        private int _head;
        private int _tail;
        private int _size;
        public int Count = 0;

        public int QueueSize => _size; // used in PrinterInfoPanel.cs

        public PriorityCircularQueue(int capacity)
        {
            _capacity = capacity;
            _buffer = new T[_capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        public bool IsFull => _size == _capacity;
        public bool IsEmpty => _size == 0;

        public void Enqueue(T item)
        {
            if (IsFull)
            {
                // Drop lowest-priority job if queue is full
                DropLowestPriorityJob();
            }

            _buffer[_tail] = item;
            _tail = (_tail + 1) % _capacity;
            _size++;
            Count++;

            // Sort queue by priority
            SortQueue();
        }

        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty.");

            T item = _buffer[_head];
            _head = (_head + 1) % _capacity;
            _size--;
            Count--;
            return item;
        }

        private void DropLowestPriorityJob()
        {
            // Find the lowest-priority job
            int minIndex = _head;
            for (int i = 1; i < _size; i++)
            {
                int index = (_head + i) % _capacity;
                if (_buffer[index].CompareTo(_buffer[minIndex]) < 0)
                {
                    minIndex = index;
                }
            }

            // Remove it by shifting
            for (int i = minIndex; i != _tail; i = (i + 1) % _capacity)
            {
                int next = (i + 1) % _capacity;
                _buffer[i] = _buffer[next]; }

            _tail = (_tail - 1 + _capacity) % _capacity;
            _size--;
        }

        private void SortQueue()
        {
            var temp = _buffer.Where(x => x != null).OrderByDescending(x => x).ToArray();
            for (int i = 0; i < temp.Length; i++)
            {
                _buffer[(_head + i) % _capacity] = temp[i];
            }
        }

        public List<T> GetQueueState()
        {
            List<T> queueState = new List<T>();
            for (int i = 0; i < _size; i++)
            {
                int index = (_head + i) % _capacity;
                queueState.Add(_buffer[index]);
            }
            return queueState;
        }
    }

}
