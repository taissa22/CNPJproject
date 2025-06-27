using System.Xml.Serialization;

namespace Oi.Juridico.WebApi.V2.DTOs.ControleDeAcesso.Menu
{

    [XmlRoot(ElementName = "MenuItem")]
    public class MenuItem
    {
        [XmlAttribute(AttributeName = "Text")]
        public string Text { get; set; } = string.Empty;
        [XmlAttribute(AttributeName = "Url")]
        public string Url { get; set; } = string.Empty;
        [XmlAttribute(AttributeName = "ToolTip")]
        public string ToolTip { get; set; } = string.Empty;
        [XmlAttribute(AttributeName = "Grants")]
        public string Grants { get; set; } = string.Empty;
        [XmlElement(ElementName = "MenuItem")]
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        [XmlAttribute(AttributeName = "GrantMode")]
        public string GrantMode { get; set; } = string.Empty;
        [XmlIgnore]
        public string Parent { get; set; } = string.Empty;
    }

    [XmlRoot(ElementName = "MenuItems")]
    public class MenuItems
    {
        [XmlElement(ElementName = "MenuItem")]
        public List<MenuItem> MenuItem { get; set; } = new List<MenuItem>();
    }
}
