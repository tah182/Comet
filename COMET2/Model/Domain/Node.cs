
namespace COMET.Model.Domain {
    /// <summary>
    /// Node with head and tail accepting abstract data.
    /// </summary>
    /// <typeparam name="T">The abstract data type</typeparam>
    public class Node<T> {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="val">the value to store in the node.</param>
        public Node(T val) {
            this.Value = val;
        }

        /// <summary>
        /// Property: the value in the node.
        /// </summary>
        public T Value {
            get;
            private set;
        }

        /// <summary>
        /// The front pointer of this node.
        /// </summary>
        public Node<T> Front { get; set; }

        /// <summary>
        /// The back poniter of this node.
        /// </summary>
        public Node<T> Back { get; set; }
    }
}