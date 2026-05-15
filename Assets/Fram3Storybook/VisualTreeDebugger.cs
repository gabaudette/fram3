#nullable enable
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fram3.UI.Storybook
{
    /// <summary>
    /// Press D in Play mode to dump the full UIToolkit visual tree to the console.
    /// Includes element type, name, position style, layout rect, and z-order index.
    /// </summary>
    public sealed class VisualTreeDebugger : MonoBehaviour
    {
        private UIDocument? _document;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                DumpTree();
            }
        }

        private void DumpTree()
        {
            if (_document == null)
            {
                Debug.Log("[VisualTreeDebugger] No UIDocument found.");
                return;
            }

            var root = _document.rootVisualElement;
            var sb = new StringBuilder();
            sb.AppendLine("[VisualTreeDebugger] === Visual Tree Dump ===");
            sb.AppendLine($"rootVisualElement children count: {root.childCount}");
            AppendElement(sb, root, 0);
            Debug.Log(sb.ToString());
        }

        private static void AppendElement(StringBuilder sb, VisualElement el, int depth)
        {
            var indent = new string(' ', depth * 2);
            var pos = el.resolvedStyle.position;
            var rect = el.layout;
            var top = el.resolvedStyle.top;
            var left = el.resolvedStyle.left;
            var right = el.resolvedStyle.right;
            var bottom = el.resolvedStyle.bottom;
            var w = el.resolvedStyle.width;
            var h = el.resolvedStyle.height;
            var zi = "(n/a)";

            sb.AppendLine(
                $"{indent}[{el.GetType().Name}] name={el.name} " +
                $"pos={pos} layout={rect} " +
                $"t={top} l={left} r={right} b={bottom} " +
                $"w={w} h={h} zIndex={zi} " +
                $"childIdx={el.parent?.IndexOf(el)} of {el.parent?.childCount}"
            );

            foreach (var child in el.Children())
            {
                AppendElement(sb, child, depth + 1);
            }
        }
    }
}
