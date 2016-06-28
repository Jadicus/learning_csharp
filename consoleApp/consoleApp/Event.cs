using System;
using System.Collections.Concurrent;

namespace consoleApp
{
    public class Button
    {
        public delegate void ClickHandler(object sender, EventArgs e);
        public event ClickHandler Click;
        protected void OnClick()
        {
            // This is for safe event invokation in multithread env. However, this can cause object to receive
            // event even after unsubscribe from receiving it.
            ClickHandler clickHandler = Click;
            if (clickHandler != null)
            {
                clickHandler(this, EventArgs.Empty);
            }
        }
        public void SimulateClick()
        {
            OnClick();
        }

    }

    public class Button2
    {
        ConcurrentDictionary<object, EventHandler> m_delegateStore =
        new ConcurrentDictionary<object, EventHandler>();
        static object clickEventKey = new object();
        public event EventHandler Click
        {
            add
            {
                m_delegateStore.AddOrUpdate(
                clickEventKey,
                value,
                (key, oldValue) =>
                (EventHandler)Delegate.Combine(oldValue, value));
            }
            remove
            {
                m_delegateStore.AddOrUpdate(
                clickEventKey,
                (EventHandler) null,
                (key, oldValue) =>
                (EventHandler)Delegate.Remove(oldValue, value));
            }
        }
        protected void OnClick()
        {
            EventHandler handler;
            if (m_delegateStore.TryGetValue(clickEventKey, out handler))
            {
                handler(this, EventArgs.Empty);
            }
        }
        public void SimulateClick()
        {
            OnClick();
        }
    }

    class TestEvent
    {
        static public void ButtonHandler(object sender, EventArgs e)
        {
            Console.WriteLine("Button clicked");
        }

        static public void ButtonHandler2(object sender, EventArgs e)
        {
            Console.WriteLine("Button Clicked 2");
        }
        public static void Test()
        {
            Button button = new Button();
            button.Click -= ButtonHandler; // not null pointer exception!!!
            button.Click += ButtonHandler;
            button.SimulateClick();
            button.Click += ButtonHandler2;
            button.SimulateClick();

            Button2 btn2 = new Button2();
            btn2.Click += ButtonHandler;
            btn2.Click += ButtonHandler2;
            button.SimulateClick();
            btn2.Click -= ButtonHandler;
            button.SimulateClick();

        }
    }
}