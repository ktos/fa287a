namespace Ktos.Fa287a
{
    public delegate void KeyHandler(object sender, KeyHandlerEventArgs e);

    public class KeyHandlerEventArgs
    {
        public KeyHandlerEventArgs(Key key)
        {
            Key = key;
        }

        public Key Key { get; private set; }
    }
}