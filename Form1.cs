using System.Runtime.InteropServices;

namespace chatgpt_typer
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 1;

        private bool stopTyping = false;
        public Form1()
        {
            InitializeComponent();
            RegisterHotKey(this.Handle, HOTKEY_ID, 0x0000, 0x1B); // 0x0000 for no modifier, 0x1B is VK_ESCAPE
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                stopTyping = true;
            }
            base.WndProc(ref m);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = $"Character count: {textBox1.Text.Length}";
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            // As the msgbox is one time use i did it in reverse as no point of blocking code once the user has interacted with it
            DialogResult result = MessageBox.Show("Start typing sequence? (starts in 5 seconds) [Press ESC to cancel] ", "Confirmation prompt", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
            {
                // do nothing ??
            }


            stopTyping = false;
            await Task.Run(async () =>
            {
                await Task.Delay(5000);  // use task delay to Avoid blocking the UI thread
                await SimulateTypingAsync(textBox1.Text, (int)numericUpDown1.Value);
            });

        }

        private async Task SimulateTypingAsync(string text, int delay)
        {
            Random rng = new Random();

            foreach (char c in text)
            {
                if (stopTyping)
                    break;

                string key = c.ToString();
                // Check if the character is one of the special characters that SendKeys needs to have enclosed in braces
                if ("^%~+(){}".Contains(key))
                {
                    key = $"{{{key}}}";
                }
                SendKeys.SendWait(key);

                // Introduce a random delay within a certain range
                int randomDelay = rng.Next(delay / 2, delay * 3 / 2);
                await Task.Delay(randomDelay);

                // Introduce a small chance of making a mistake
                if (rng.Next(100) < 5)
                {
                    // Simulate a mistake by sending a backspace and retyping the character
                    SendKeys.SendWait("{BACKSPACE}");
                    SendKeys.SendWait(key);

                    // Optionally add a delay after correcting the mistake
                    await Task.Delay(randomDelay);
                }
            }
        }
    }
}