using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    /// <summary>
    /// Linear Queue FIFO
    /// </summary>
    /// <typeparam name="T">The abstract type</typeparam>
    public interface IQueueADT<T> {
        /// <summary>
        /// Adds one element to the rear of this queue.
        /// </summary>
        /// <param name="element">the element to be added to the rear of this queue</param>
        void enqueue(T element);
    
        /// <summary>
        /// Removes and returns the element at the front of this queue
        /// <tj
        /// </summary>
        /// <returns>the element at the front of this queue</returns>
        /// <exception cref="NullReferenceExecption">When queue is empty</exception>
        T dequeue();
    
        /// <summary>
        ///  Returns without removing the element at the front of this queue
        /// </summary>
        /// <returns>the first element in this queue</returns>
        T First { get; }
    
        /// <summary>
        /// Returns without removing the element at the end of this queue
        /// </summary>
        /// <returns>the last element in this queue</returns>
        T Last { get; }
    
        /// <summary>
        /// Returns true if this queue contains no elements.
        /// </summary>
        /// <returns>true if this queue is empty</returns>
        bool isEmpty();
    
        /// <summary>
        /// Returns the number of elements in this queue.
        /// </summary>
        /// <returns>the integer representation of the size of this queue.</returns>
        int Size { get; }
        
        /// <summary>
        /// Returns a string representation of this queue.
        /// </summary>
        /// <returns>The string representation of this queue.</returns>
        string ToString();
    }
}