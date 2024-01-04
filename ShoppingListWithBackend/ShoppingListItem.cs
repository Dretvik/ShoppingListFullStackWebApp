namespace ShoppingListWithBackend
{
    public class ShoppingListItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public ShoppingListItem(Guid id, string text)
        {
            Id = id;
            Text = text;
        }
        public ShoppingListItem()
        {
            
        }

    }
}
