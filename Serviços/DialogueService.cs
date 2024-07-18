namespace Cambios.Serviços
{
    using System.Windows.Forms;
    public class DialogueService
    {
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
