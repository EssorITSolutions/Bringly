namespace Bringly.Domain
{
    /// <summary>
    /// Custom Selected List Item wrapper to wrap object list 
    /// </summary>
    public class CustomSelectListItem
    {
        /// <summary>
        /// Text field for the list
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Value filed for the list
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// true for the selected value
        /// </summary>
        public bool IsSelected { get; set; } = false;
    }
}
