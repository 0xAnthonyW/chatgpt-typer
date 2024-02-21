namespace chatgpt_typer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = $"Character count: {textBox1.Text.Length}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = "Start typing sequence? (starts in 5 seconds) ";
            string title = "Confirmation prompt";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                Thread.Sleep(500);
                SimulateTyping(textBox1.Text, (int)numericUpDown1.Value);
            }
            else
            {
                //does nothing
            }
        }

        private async void SimulateTyping(string text, int delay)
        {
            foreach (char c in text)
            {
                SendKeys.SendWait(c.ToString());
                await Task.Delay(delay);
            }
        }
    }
}
