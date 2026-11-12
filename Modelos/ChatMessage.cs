namespace Menushell401.Modelos;

public class ChatMessage
{
    public string Content { get; set; } = "";
    public string SenderName { get; set; } = "";
    public LayoutOptions HorizontalAlignment { get; set; }
    public Color BackgroundColor { get; set; } = Colors.Gray;
}