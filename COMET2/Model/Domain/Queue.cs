using System;

namespace COMET.Model.Domain {
    /// <summary>
    /// Linear Queue FIFO
    /// </summary>
    public class Queue<T> : IQueueADT<COMET.Model.Domain.Node<T>> {
        public Queue() {
            this.First = null;
        }

        public Node<T> First { 
            get; 
            private set; 
        }

        public Node<T> Last {
            get;
            private set;
        }

        public void enqueue(Node<T> element) {
            if (isEmpty())
                this.First = this.Last = element;
            else {
                element.Front = this.Last;
                this.Last.Back = element;
                this.Last = element;
            }
        }

        public Node<T> dequeue() {
            if (isEmpty())
                throw new NullReferenceException("Queue is empty.");

            if (this.First == this.Last) {
                Node<T> node = this.First;
                this.First = null;
                this.Last = null;
                return node;
            }

            Node<T> n = this.First;
            this.First = this.First.Back;
            this.First.Front = null;
            return n;
        }

        public bool isEmpty() {
            return this.First == null;
        }

        public int Size {
            get {
                if (isEmpty())
                    return 0;

                int size = 1;
                Node<T> node = this.First;
                while (node.Back != null) {
                    size++;
                    node = node.Back;
                }
                return size;
            }
        }

        public override string ToString() {
            Node<T> node = this.First;
            string printout = "";
            while (node.Back != null) {
                printout += node.Value + ",";
                node = node.Back;
            }
            return printout.Substring(0, printout.Length - 1);
        }
    }
}