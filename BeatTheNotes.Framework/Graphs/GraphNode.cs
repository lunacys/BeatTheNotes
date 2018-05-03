using Microsoft.Xna.Framework;

namespace BeatTheNotes.Framework.Graphs
{
    public class GraphNode
    {
        /// <summary>
        /// Pointer to the next graph node
        /// </summary>
        public GraphNode Next { get; }

        /// <summary>
        /// Graph node value
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Current graph node offset. Used for offsetting the other nodes.
        /// </summary>
        public float XOffset { get; }

        public GraphNode(GraphNode nextNode, float value, float xOffset)
        {
            Next = nextNode;
            Value = value;
            XOffset = xOffset;
        }
    }
}