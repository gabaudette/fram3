#nullable enable
using System;
using Fram3.UI.Core;
using NUnit.Framework;

namespace Fram3.UI.Tests.Core
{
    [TestFixture]
    internal sealed class ValueNotifierTests
    {
        [Test]
        public void Value_ReturnsInitialValue()
        {
            var notifier = new ValueNotifier<int>(42);

            Assert.That(notifier.Value, Is.EqualTo(42));
        }

        [Test]
        public void Value_Set_UpdatesValue()
        {
            var notifier = new ValueNotifier<int>(0);

            notifier.Value = 7;

            Assert.That(notifier.Value, Is.EqualTo(7));
        }

        [Test]
        public void Value_Set_NotifiesListeners()
        {
            var notifier = new ValueNotifier<int>(0);
            var callCount = 0;
            notifier.AddListener(() => callCount++);

            notifier.Value = 1;

            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Value_Set_ToSameValue_DoesNotNotifyListeners()
        {
            var notifier = new ValueNotifier<int>(5);
            var callCount = 0;
            notifier.AddListener(() => callCount++);

            notifier.Value = 5;

            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void AddListener_MultipleListeners_AllNotified()
        {
            var notifier = new ValueNotifier<string>("a");
            var calls = new System.Collections.Generic.List<string>();
            notifier.AddListener(() => calls.Add("first"));
            notifier.AddListener(() => calls.Add("second"));

            notifier.Value = "b";

            Assert.That(calls, Is.EqualTo(new[] { "first", "second" }));
        }

        [Test]
        public void RemoveListener_StopsNotification()
        {
            var notifier = new ValueNotifier<int>(0);
            var callCount = 0;
            Action listener = () => callCount++;
            notifier.AddListener(listener);

            notifier.RemoveListener(listener);
            notifier.Value = 1;

            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void RemoveListener_NotRegistered_DoesNotThrow()
        {
            var notifier = new ValueNotifier<int>(0);

            Assert.DoesNotThrow(() => notifier.RemoveListener(() => { }));
        }

        [Test]
        public void AddListener_NullListener_ThrowsArgumentNullException()
        {
            var notifier = new ValueNotifier<int>(0);

            Assert.Throws<ArgumentNullException>(() => notifier.AddListener(null!));
        }

        [Test]
        public void RemoveListener_NullListener_ThrowsArgumentNullException()
        {
            var notifier = new ValueNotifier<int>(0);

            Assert.Throws<ArgumentNullException>(() => notifier.RemoveListener(null!));
        }

        [Test]
        public void Dispose_ClearsListeners()
        {
            var notifier = new ValueNotifier<int>(0);
            var callCount = 0;
            notifier.AddListener(() => callCount++);

            notifier.Dispose();

            Assert.Throws<ObjectDisposedException>(() => notifier.Value = 1);
            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void Dispose_CalledTwice_DoesNotThrow()
        {
            var notifier = new ValueNotifier<int>(0);
            notifier.Dispose();

            Assert.DoesNotThrow(() => notifier.Dispose());
        }

        [Test]
        public void Value_Get_AfterDispose_ThrowsObjectDisposedException()
        {
            var notifier = new ValueNotifier<int>(0);
            notifier.Dispose();

            Assert.Throws<ObjectDisposedException>(() => _ = notifier.Value);
        }

        [Test]
        public void Value_Set_AfterDispose_ThrowsObjectDisposedException()
        {
            var notifier = new ValueNotifier<int>(0);
            notifier.Dispose();

            Assert.Throws<ObjectDisposedException>(() => notifier.Value = 1);
        }

        [Test]
        public void AddListener_AfterDispose_ThrowsObjectDisposedException()
        {
            var notifier = new ValueNotifier<int>(0);
            notifier.Dispose();

            Assert.Throws<ObjectDisposedException>(() => notifier.AddListener(() => { }));
        }

        [Test]
        public void Value_WorksWithReferenceType()
        {
            var notifier = new ValueNotifier<string?>(null);
            var callCount = 0;
            notifier.AddListener(() => callCount++);

            notifier.Value = "hello";

            Assert.That(notifier.Value, Is.EqualTo("hello"));
            Assert.That(callCount, Is.EqualTo(1));
        }

        [Test]
        public void Value_Set_MultipleChanges_NotifiesEachTime()
        {
            var notifier = new ValueNotifier<int>(0);
            var callCount = 0;
            notifier.AddListener(() => callCount++);

            notifier.Value = 1;
            notifier.Value = 2;
            notifier.Value = 3;

            Assert.That(callCount, Is.EqualTo(3));
        }
    }
}
