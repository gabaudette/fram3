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
            var rect = el.layout;
            var rw = el.resolvedStyle.width;
            var rh = el.resolvedStyle.height;
            var pos = el.resolvedStyle.position;
            var sw = el.style.width;
            var sh = el.style.height;
            var childIdx = el.parent?.IndexOf(el);
            var childOf = el.parent?.childCount;

            sb.AppendLine(
                $"{indent}[{el.GetType().Name}] " +
                $"layout={rect} resolvedW={rw} resolvedH={rh} pos={pos} " +
                $"styleW={sw} styleH={sh} " +
                $"idx={childIdx}/{childOf} name={el.name}"
            );

            foreach (var child in el.Children())
            {
                AppendElement(sb, child, depth + 1);
            }
        }
    }
}
