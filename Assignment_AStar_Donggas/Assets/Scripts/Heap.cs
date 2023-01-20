using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace MyHeap
{
    public class MapNode
    {
        public (int x, int z) Position { get; private set; }
        public int Cost { get; private set; }

        public int Evaluate(int count, int heuristic)
        {
            Cost = count + heuristic;

            return Cost;
        }

        public int Evaluate(int count, UnityEngine.Vector3 destination)
        {
            int heuristic = Mathf.Abs((int)destination.x - Position.x) + Mathf.Abs((int)destination.z - Position.z);
            
            Cost = Evaluate(count, heuristic);

            return Cost;
        }
    }

    // 최소힙
    public class MapHeap
    {
        protected List<MapNode> container = new List<MapNode>();

        public virtual int Top()
        {
            return container[0].Cost;
        }

        public virtual bool Empty()
        {
            return container.Count == 0;
        }

        public virtual int Size()
        {
            return container.Count;
        }

        public virtual void Push(MapNode value)
        {
            // 1. 맨 끝에 데이터를 삽입
            container.Add(value);

            // 2. 힙의 불변성을 만족할 때까지 데이터 교환
            // 첫 번째 노드의 인덱스를 1로 생각한다.
            int currentIndex = Size();

            while (currentIndex > 1)
            {
                // 2-1. 부모 노드 인덱스를 구한다.
                int parentIndex = currentIndex / 2;

                // 2-2. 부모 노드와 비교
                if (container[parentIndex - 1].Cost < container[currentIndex - 1].Cost)
                {
                    // 2-2-1. 부모가 나보다 더 작다면 힙의 불변성을 만족, 종료
                    break;
                }

                // 2-3. 부모 노드와 바꾸고 현재 위치를 갱신
                Swap(parentIndex - 1, currentIndex - 1);
                currentIndex = parentIndex;
            }
        }

        public virtual int Pop()
        {
            // 1. 반환할 값 저장
            int top = Top();

            // 2. 마지막 노드를 루트 노드로 복사
            container[0] = container[Size() - 1];

            // 3. 마지막 노드를 제거
            container.RemoveAt(Size() - 1);

            // 4. 힙의 불변성을 만족할 때까지 데이터 교환
            // 첫 번째 노드의 인덱스를 1로 생각한다.
            int currentSize = Size();
            int currentIndex = 1;

            while (currentIndex < currentSize)
            {
                // 4-1. 자식 노드의 인덱스를 구한다.
                int left = currentIndex * 2;
                int right = left + 1;

                // 4-1-1. 자식이 존재해야 한다.
                if (left > currentSize)
                {
                    break;
                }

                // 4-2. 자식 노드 중 더 작은 노드를 찾는다. 
                int child = left;
                if (right <= currentSize && container[left - 1].Cost > container[right - 1].Cost)
                {
                    child = right;
                }

                // 4-3. 자식 노드와 비교
                if (container[currentIndex - 1].Cost >= container[child - 1].Cost)
                {
                    // 4-3-1. 자식이 나보다 작다면 힙의 불변성을 만족, 종료
                    break;
                }

                // 4-4. 자식 노드와 바꾸고 현재 위치를 갱신
                Swap(currentIndex - 1, child - 1);
                currentIndex = child;
            }

            // 5. 삭제한 값 반환
            return top;
        }

        // 인덱스를 받아 container 값 교환
        protected virtual void Swap(int parIndex, int curIndex)
        {
            MapNode temp = container[parIndex];
            container[parIndex] = container[curIndex];
            container[curIndex] = temp;
        }

        // 힙을 비운다.
        public virtual void Clear()
        {
            container.Clear();
        }
    }

    // 최소힙
    public class Heap
    {
        protected List<int> container = new List<int>();

        public virtual int Top()
        {
            return container[0];
        }

        public virtual bool Empty()
        {
            return container.Count == 0;
        }

        public virtual int Size()
        {
            return container.Count;
        }

        public virtual void Push(int value)
        {
            // 1. 맨 끝에 데이터를 삽입
            container.Add(value);

            // 2. 힙의 불변성을 만족할 때까지 데이터 교환
            // 첫 번째 노드의 인덱스를 1로 생각한다.
            int currentIndex = Size();

            while (currentIndex > 1)
            {
                // 2-1. 부모 노드 인덱스를 구한다.
                int parentIndex = currentIndex / 2;

                // 2-2. 부모 노드와 비교
                if (container[parentIndex - 1] < container[currentIndex - 1])
                {
                    // 2-2-1. 부모가 나보다 더 작다면 힙의 불변성을 만족, 종료
                    break;
                }

                // 2-3. 부모 노드와 바꾸고 현재 위치를 갱신
                Swap(parentIndex - 1, currentIndex - 1);
                currentIndex = parentIndex;
            }
        }

        public virtual int Pop()
        {
            // 1. 반환할 값 저장
            int top = Top();

            // 2. 마지막 노드를 루트 노드로 복사
            container[0] = container[Size() - 1];

            // 3. 마지막 노드를 제거
            container.RemoveAt(Size() - 1);

            // 4. 힙의 불변성을 만족할 때까지 데이터 교환
            // 첫 번째 노드의 인덱스를 1로 생각한다.
            int currentSize = Size();
            int currentIndex = 1;

            while (currentIndex < currentSize)
            {
                // 4-1. 자식 노드의 인덱스를 구한다.
                int left = currentIndex * 2;
                int right = left + 1;

                // 4-1-1. 자식이 존재해야 한다.
                if (left > currentSize)
                {
                    break;
                }

                // 4-2. 자식 노드 중 더 작은 노드를 찾는다. 
                int child = left;
                if (right <= currentSize && container[left - 1] > container[right - 1])
                {
                    child = right;
                }

                // 4-3. 자식 노드와 비교
                if (container[currentIndex - 1] >= container[child - 1])
                {
                    // 4-3-1. 자식이 나보다 작다면 힙의 불변성을 만족, 종료
                    break;
                }

                // 4-4. 자식 노드와 바꾸고 현재 위치를 갱신
                Swap(currentIndex - 1, child - 1);
                currentIndex = child;
            }

            // 5. 삭제한 값 반환
            return top;
        }

        // 인덱스를 받아 container 값 교환
        protected virtual void Swap(int parIndex, int curIndex)
        {
            int temp = container[parIndex];
            container[parIndex] = container[curIndex];
            container[curIndex] = temp;
        }

        // 힙을 비운다.
        public virtual void Clear()
        {
            container.Clear();
        }
    }

    #region 일반화시도
    /*
        public class Heap<T> where T : IComparable
        {
            protected List<T> container = new List<T>();

            public virtual T Top()
            {
                return container[0];
            }

            public virtual bool Empty()
            {
                return container.Count == 0;
            }

            public virtual int Size()
            {
                return container.Count;
            }

            public virtual void Push(T value)
            {
                // 1. 먼저 맨 끝에 데이터를 삽입한다.
                container.Add(value);

            // 2. 힙의 불변성을 만족할 때까지 데이터를 바꿔준다.
                // 첫 번째 노드를 1로 생각한다.
                int currentIndex = Size();

                while (currentIndex > 1)
                {
                    // 2-1. 부모 노드를 찾는다.
                    int parentIndex = currentIndex / 2;

                    // 2-2. 부모 노드와 비교한다.
                    // MaxHeap
                    //if (container[parentIndex - 1] >= container[currentIndex - 1])
                    //{
                    //    // 2-2-1. 부모가 나보다 더 크다면 힙의 불변성을 만족하는 것이므로 종료
                    //    break;
                    //}

                    // 2-3. 부모 노드와 바꾸고 현재 위치를 갱신한다.
                    Swap(parentIndex - 1, currentIndex - 1);
                    currentIndex = parentIndex;
                }
            }

            protected virtual void Swap(int parIndex, int curIndex)
            {
                int temp = container[parIndex];
                container[parIndex] = container[curIndex];
                container[curIndex] = temp;
            }

            public virtual T Pop()
            {
                T top = Top();



                return top;
            }

            public virtual void Clear()
            {
                container.Clear();
            }
        }
    */
    #endregion
}
