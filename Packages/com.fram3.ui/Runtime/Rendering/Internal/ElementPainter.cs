#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Input;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using UnityEngine.UIElements;
using UITextField = UnityEngine.UIElements.TextField;
using UIScrollView = UnityEngine.UIElements.ScrollView;
using UIProgressBar = UnityEngine.UIElements.ProgressBar;
using UIFloatField = UnityEngine.UIElements.FloatField;
using UIMinMaxSlider = UnityEngine.UIElements.MinMaxSlider;
namespace Fram3.UI.Rendering.Internal
{
    /// <summary>
    /// Responsible for creating and updating native UIToolkit <see cref="VisualElement"/>
    /// instances to match a given <see cref="Element"/> description.
    /// </summary>
    internal static partial class ElementPainter
    {
        private static UnityEngine.Color ToUnity(FrameColor c) => new(c.R, c.G, c.B, c.A);

        /// <summary>
        /// Creates the appropriate native <see cref="VisualElement"/> for the given element
        /// and applies all initial style properties to it.
        /// </summary>
        /// <param name="element">The framework element to produce a native element for.</param>
        /// <param name="theme"></param>
        /// <returns>
        /// A <c>Label</c> for <see cref="Text"/>, a <c>Button</c> for <see cref="Elements.Input.Button"/>,
        /// a <c>TextField</c> for <see cref="Elements.Input.TextField"/>, a <c>Toggle</c> for <see cref="FrameToggle"/>,
        /// a <c>Slider</c> for <see cref="FrameSlider"/>, a <c>DropdownField</c> for <see cref="Dropdown"/>,
        /// a <c>ProgressBar</c> for <see cref="Elements.Content.ProgressBar"/>, a <c>ScrollView</c> for <see cref="Elements.Content.ScrollView"/>,
        /// an <c>Image</c> for <see cref="FrameImage"/> or <see cref="Icon"/>,
        /// an <c>SpinnerElement</c> for <see cref="Spinner"/>,
        /// or a plain <c>VisualElement</c> for all layout/container/gesture elements.
        /// </returns>
        internal static VisualElement CreateNative(Element element, Theme theme)
        {
#if !FRAM3_PURE_TESTS
            if (element is Spinner spinner)
            {
                return CreateSpinner(spinner);
            }
#endif
            switch (element)
            {
                case Text text:
                    return CreateLabel(text);
                case PasswordField passwordField:
                    return CreatePasswordField(passwordField, theme);
                case Elements.Input.TextField textField:
                    return CreateTextField(textField, theme);
                case Checkbox checkbox:
                    return CreateCheckbox(checkbox, theme);
                case RadioGroup radioGroup:
                    return CreateRadioGroup(radioGroup, theme);
                case Modal modal:
                    return CreateModal(modal);
#if !FRAM3_PURE_TESTS
                case ContextMenu contextMenu:
                    return CreateContextMenu(contextMenu, theme);
                case DraggablePanel draggablePanel:
                    return CreateDraggablePanel(draggablePanel, theme);
#endif
                case FrameToggle toggle:
                    return CreateToggle(toggle, theme);
                case IntField intField:
                    return CreateIntField(intField, theme);
                case Elements.Input.FloatField floatField:
                    return CreateFloatField(floatField, theme);
                case Elements.Input.MinMaxSlider minMaxSlider:
                    return CreateMinMaxSlider(minMaxSlider, theme);
                case IEnumFieldDescriptor enumField:
                    return CreateEnumField(enumField);
                case FrameSlider slider:
                    return CreateSlider(slider, theme);
                case Dropdown dropdown:
                    return CreateDropdown(dropdown, theme);
                case Elements.Content.ProgressBar progressBar:
                    return CreateProgressBar(progressBar, theme);
                case Elements.Content.ScrollView scrollView:
                    return CreateScrollView(scrollView, theme);
                case FrameImage image:
                    return CreateImage(image);
                case Icon icon:
                    return CreateIcon(icon);
                case Badge badge:
                    return CreateBadge(badge, theme);
                default:
                    switch (element)
                    {
                        case IGridElement gridElement:
                            return CreateGrid(gridElement, theme);
                        case IListViewDescriptor listView:
                            return CreateListView(listView, theme);
                        case IEnumFieldDescriptor enumFieldDesc:
                            return CreateEnumField(enumFieldDesc);
                    }

                    var native = new VisualElement();
                    ApplyLayout(element, native, theme);
                    RegisterGestureCallbacks(element, native);
                    return native;
            }
        }

        /// <summary>
        /// Applies absolute positioning to a native element so that it behaves as a
        /// layer inside an <see cref="Stack"/> container. Called by the renderer after
        /// attaching the child to a stack parent.
        /// </summary>
        internal static void ApplyAsStackChild(VisualElement native)
        {
            native.style.position = Position.Absolute;
        }

        /// <summary>
        /// Returns the slot that a framework child should be added to for a given
        /// parent element. For most elements this is the parent native itself.
        /// For <see cref="DraggablePanel"/> the child goes into the body container
        /// at index 1, not the root panel native.
        /// </summary>
        internal static VisualElement GetChildSlot(Element parentElement, VisualElement parentNative)
        {
#if !FRAM3_PURE_TESTS
            if (parentElement is DraggablePanel && parentNative.childCount >= 2)
                return parentNative[1];
#endif
            return parentNative;
        }

        /// <summary>
        /// Updates an existing native <see cref="VisualElement"/> to reflect the latest
        /// property values from the given element description.
        /// </summary>
        /// <param name="element">The current element description.</param>
        /// <param name="native">The existing native element to update.</param>
        /// <param name="theme">The active theme used when applying colors and style values.</param>
        internal static void Paint(Element element, VisualElement native, Theme theme)
        {
#if !FRAM3_PURE_TESTS
            if (element is Spinner spinner && native is SpinnerElement spinnerEl)
            {
                spinnerEl.Apply(spinner);
                return;
            }
#endif
            switch (element)
            {
                case Text text when native is Label label:
                    PaintText(text, label);
                    break;
                case PasswordField passwordField when native is UITextField ptf:
                    PaintPasswordField(passwordField, ptf);
                    break;
                case Elements.Input.TextField textField when native is UITextField tf:
                    PaintTextField(textField, tf);
                    break;
                case Checkbox checkbox when native is Toggle chkTgl:
                    PaintCheckbox(checkbox, chkTgl);
                    break;
                case RadioGroup radioGroup when native is RadioButtonGroup rbg:
                    PaintRadioGroup(radioGroup, rbg);
                    break;
                case Modal:
                    PaintModal(native);
                    break;
#if !FRAM3_PURE_TESTS
                case ContextMenu contextMenu:
                    PaintContextMenu(contextMenu, native, theme);
                    break;
                case DraggablePanel draggablePanel:
                    PaintDraggablePanel(draggablePanel, native, theme);
                    break;
#endif
                case FrameToggle toggle when native is Toggle tgl:
                    PaintToggle(toggle, tgl);
                    break;
                case IntField intField when native is IntegerField intf:
                    PaintIntField(intField, intf);
                    break;
                case Elements.Input.FloatField floatField when native is UIFloatField ff:
                    PaintFloatField(floatField, ff);
                    break;
                case Elements.Input.MinMaxSlider minMaxSlider when native is UIMinMaxSlider mms:
                    PaintMinMaxSlider(minMaxSlider, mms);
                    break;
                case FrameSlider slider when native is Slider sld:
                    PaintSlider(slider, sld);
                    break;
                case Dropdown dropdown when native is DropdownField dd:
                    PaintDropdown(dropdown, dd);
                    break;
                case Elements.Content.ProgressBar progressBar when native is UIProgressBar pb:
                    PaintProgressBar(progressBar, pb);
                    break;
                case Elements.Content.ScrollView scrollView when native is UIScrollView sv:
                    PaintScrollView(scrollView, sv);
                    break;
                case FrameImage image when native is Image img:
                    PaintImage(image, img);
                    break;
                case Icon icon when native is Image iconImg:
                    PaintIcon(icon, iconImg);
                    break;
                case Badge badge when native.userData is BadgePipHolder pipHolder:
                    PaintBadge(badge, pipHolder, theme);
                    break;
                default:
                    if (element is IGridElement gridElementPaint)
                    {
                        PatchNativeGrid(gridElementPaint, native, theme);
                        break;
                    }

                    if (element is IListViewDescriptor listView && native is ListView lv)
                    {
                        PaintListView(listView, lv);
                        break;
                    }

                    if (element is IEnumFieldDescriptor enumFieldDesc && native is EnumField ef)
                    {
                        PaintEnumField(enumFieldDesc, ef);
                        break;
                    }

                    if (element is GestureDetector updatedGesture && native.userData is GestureCallbackHolder holder)
                    {
                        holder.OnTap = updatedGesture.OnTap;
                        holder.OnDoubleTap = updatedGesture.OnDoubleTap;
                        holder.OnLongPress = updatedGesture.OnLongPress;
                        holder.OnPointerEnter = updatedGesture.OnPointerEnter;
                        holder.OnPointerExit = updatedGesture.OnPointerExit;
                        holder.OnSecondaryTap = updatedGesture.OnSecondaryTap;
                    }

                    ApplyLayout(element, native, theme);
                    break;
            }
        }

        /// <summary>
        /// Recursively builds a native <see cref="VisualElement"/> subtree for the given
        /// element and all of its descendants, then attaches the root of that subtree to
        /// <paramref name="parent"/>. Used by <see cref="CreateListView"/> to populate
        /// each list item without a node tree.
        /// </summary>
        /// <param name="element">The element to create a native subtree for.</param>
        /// <param name="parent">The native element to attach the new subtree to.</param>
        /// <param name="theme">The active theme used for styling.</param>
        private static void BuildNativeTree(Element element, VisualElement parent, Theme theme)
        {
            var native = CreateNative(element, theme);
            Paint(element, native, theme);

            var children = element.GetChildren();
            var isStack = element is Stack;

            foreach (var child in children)
            {
                BuildNativeTree(child, native, theme);
            }

            if (isStack)
            {
                foreach (var childNative in native.Children())
                {
                    ApplyAsStackChild(childNative);
                }
            }

            parent.Add(native);
        }
    }
}
